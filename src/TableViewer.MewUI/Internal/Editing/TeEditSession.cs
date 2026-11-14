using System.ComponentModel;
using System.Text.RegularExpressions;
using TableViewer.MewUI.Options.Property;

namespace TableViewer.MewUI.Internal.Editing;

/// <summary>Owns source values, converters, validation, and notifications independently of native controls.</summary>
public sealed class TeEditSession
{
    private readonly Type _rootType;
    private readonly List<TeEditNode> _nodes = [];
    private readonly List<ICustomEditorArgument> _customArguments = [];
    private readonly Dictionary<ICustomEditorArgument, TeEditNode> _customNodes = [];
    public object? Data { get; private set; }
    public TeOptions Options { get; }
    public TeEditNode Root { get; private set; } = null!;
    public IReadOnlyList<TeEditNode> Nodes => _nodes;
    public string[] Errors => _nodes.SelectMany(node => node.Errors).ToArray();
    public bool IsValid => Errors.Length == 0;
    public Func<object?, Task>? DataChanged { get; set; }
    public Func<bool, Task>? IsValidChanged { get; set; }
    public Func<string[], Task>? ErrorsChanged { get; set; }
    public event Action? StructureChanged;
    public event Action? ValidationChanged;

    public TeEditSession(object? data, TeOptions? options = null, Type? declaredType = null)
    {
        Data = data;
        Options = options ?? new();
        _rootType = declaredType ?? data?.GetType() ?? typeof(object);
        Rebuild();
    }

    public void Load(object? data)
    {
        Data = data;
        Rebuild();
        StructureChanged?.Invoke();
    }

    public void Rebuild()
    {
        _nodes.Clear();
        _customArguments.Clear();
        _customNodes.Clear();
        Root = BuildNode(Data, _rootType, null, null, "", Options.Title ?? "Value", value => Data = value, new(ReferenceEqualityComparer.Instance), 0);
    }

    private TeEditNode BuildNode(object? value, Type type, object? parent, MemberInfo? member, string path, string label,
        Action<object?> setter, HashSet<object> ancestors, int depth)
    {
        var node = new TeEditNode(this, value, type, parent, member, path, label, setter);
        _nodes.Add(node);
        if (Options.TryGetCustomEditor(member, value?.GetType() ?? type, out var custom)) node.Option = custom;
        else Options.TryGetFieldOption(member, new TeEditorBase { Data = value, Options = Options }, out node.Option);
        node.Option ??= DefaultOption(Nullable.GetUnderlyingType(type) ?? type);
        if (node.Option is TeCustomEditorOption) return node;
        if (node.Option is ITeListEditorOption listOption)
        {
            node.List = value is null ? new ArrayList() : listOption.Converter.ToField(value) ?? new ArrayList();
            BuildList(node, ancestors, depth);
            return node;
        }
        if (value is IList list)
        {
            node.List = list;
            BuildList(node, ancestors, depth);
            return node;
        }
        if (node.Option is not null || value is null) return node;
        if (depth >= 64 || !ancestors.Add(value))
        {
            node.Diagnostic = depth >= 64 ? "Maximum editing depth reached." : "Circular reference.";
            return node;
        }
        try
        {
            foreach (var info in EditableMembers(type))
            {
                var childValue = ReadMember(info, value);
                var childPath = path.Length == 0 ? info.Name : $"{path}.{info.Name}";
                node.Children.Add(BuildNode(childValue, MemberType(info), value, info, childPath, info.Name, newValue =>
                {
                    WriteMember(info, value, newValue);
                    setter(value);
                }, ancestors, depth + 1));
            }
        }
        finally { ancestors.Remove(value); }
        return node;
    }

    private void BuildList(TeEditNode node, HashSet<object> ancestors, int depth)
    {
        var list = node.List!;
        if (depth >= 64 || !ancestors.Add(list)) { node.Diagnostic = "Circular or excessively deep list."; return; }
        try
        {
            var itemType = node.DeclaredType.IsArray ? node.DeclaredType.GetElementType()!
                : list.GetType().GetInterfaces().FirstOrDefault(type => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IList<>))?.GetGenericArguments()[0]
                    ?? typeof(object);
            for (var i = 0; i < list.Count; i++)
            {
                var index = i;
                var item = list[index];
                node.Children.Add(BuildNode(item, item?.GetType() ?? itemType, list, null, $"{node.Path}[{index}]", (index + 1).ToString(), newValue =>
                {
                    list[index] = newValue;
                    node.AssignSource(node.Option is ITeListEditorOption option ? option.Converter.FromField(list) : list);
                }, ancestors, depth + 1));
            }
        }
        finally { ancestors.Remove(list); }
    }

    public async Task SetFieldValueAsync(TeEditNode node, object? fieldValue)
    {
        try
        {
            var value = node.Option is ITeConvertable convertible
                ? convertible.Converter.FromField(fieldValue!)
                : Coerce(fieldValue, node.DeclaredType);
            await SetValueAsync(node, value);
        }
        catch (Exception exception)
        {
            node.ConversionError = Unwrap(exception).Message;
            node.Errors = [node.ConversionError];
            await NotifyValidationAsync();
        }
    }

    public async Task SetValueAsync(TeEditNode node, object? value)
    {
        if (IsReadOnly(node)) return;
        try
        {
            node.AssignSource(Coerce(value, node.DeclaredType));
            node.ConversionError = null;
            node.IsDirty = true;
            await ValidateNodeAsync(node, false);
            if (node.Option is ITeTextFieldOption { Event.ValueChanged: { } changed }) await changed(value);
            if (DataChanged is not null) await DataChanged(Data);
            foreach (var argument in _customArguments.ToArray())
                if (ReferenceEquals(argument.Parent, node.Parent))
                {
                    if (argument is CustomEditorArgument<object, object?> typed && _customNodes.TryGetValue(argument, out var editedNode)) typed.Value = editedNode.Value;
                    argument.OnParentChanged(argument.Parent);
                }
        }
        catch (Exception exception) { node.Errors = [Unwrap(exception).Message]; }
        await NotifyValidationAsync();
    }

    public async Task AddItemAsync(TeEditNode node)
    {
        var action = node.Option switch { ITeListEditorOption option => option.AddItemAction, ITeObjectListEditorOption option => option.AddItemAction, _ => null };
        if (action is null || node.List is null) return;
        try
        {
            await action.Action(node.List);
            node.AssignSource(node.Option is ITeListEditorOption option ? option.Converter.FromField(node.List) : node.List);
            if (DataChanged is not null) await DataChanged(Data);
            Rebuild();
            StructureChanged?.Invoke();
        }
        catch (Exception exception) { node.Errors = [Unwrap(exception).Message]; }
        await NotifyValidationAsync();
    }

    public async Task RemoveItemAsync(TeEditNode node, int index)
    {
        var action = node.Option switch { ITeListEditorOption option => option.RemoveItemAction, ITeObjectListEditorOption option => option.RemoveItemAction, _ => null };
        if (action is null || node.List is null || node.List.IsFixedSize || node.List.IsReadOnly) return;
        node.List.RemoveAt(index);
        node.AssignSource(node.Option is ITeListEditorOption listOption ? listOption.Converter.FromField(node.List) : node.List);
        if (DataChanged is not null) await DataChanged(Data);
        Rebuild();
        StructureChanged?.Invoke();
        await NotifyValidationAsync();
    }

    public async Task ValidateAsync()
    {
        foreach (var node in _nodes) await ValidateNodeAsync(node, true);
        await NotifyValidationAsync();
    }

    private async Task ValidateNodeAsync(TeEditNode node, bool force)
    {
        var property = Property(node.Option);
        if (!force && property is TeBaseInputProperty { OnlyValidateIfDirty: true } && !node.IsDirty) return;
        var errors = new List<string>();
        if (node.ConversionError is { } conversionError) errors.Add(conversionError);
        if (property?.Required == true && (node.Value is null || node.Value is string text && string.IsNullOrWhiteSpace(text)
            || property is TeBooleanInputProperty && node.Value is false)) errors.Add(property.RequiredError);
        if (property?.Error == true) errors.Add(property.ErrorText ?? "Invalid value.");
        if (property is TeBaseInputProperty input && node.Value is string stringValue)
        {
            if (stringValue.Length > input.MaxLength) errors.Add($"Maximum length is {input.MaxLength}.");
            if (input.Pattern is { Length: > 0 } pattern && input is not TeNumericFieldProperty)
            {
                try { if (!Regex.IsMatch(stringValue, $"^(?:{pattern})$")) errors.Add("Input does not match the required pattern."); }
                catch (ArgumentException exception) { errors.Add(exception.Message); }
            }
        }
        var validations = node.Option switch
        {
            ITeTextFieldOption option => option.Validations,
            ITeNumericFieldOption option => option.Validations,
            ITeRadioOption option => option.Validations,
            _ => []
        };
        foreach (var validation in validations)
        {
            try { if (!await validation.Func(node.Value!)) errors.Add(validation.Message); }
            catch (Exception exception) { errors.Add(Unwrap(exception).Message); }
        }
        node.Errors = errors.ToArray();
    }

    private async Task NotifyValidationAsync()
    {
        ValidationChanged?.Invoke();
        if (ErrorsChanged is not null) await ErrorsChanged(Errors);
        if (IsValidChanged is not null) await IsValidChanged(IsValid);
    }

    public ICustomEditorArgument CustomArgument(TeEditNode node)
    {
        var argument = new CustomEditorArgument<object, object?>
        {
            Parent = node.Parent ?? Data ?? this,
            Value = node.Value,
            DataChanged = value => SetValueAsync(node, value)
        };
        _customArguments.Add(argument);
        _customNodes[argument] = node;
        return argument;
    }

    public static TeFormComponentProperty? Property(ITeFieldOption? option) => option switch
    {
        ITeTextFieldOption field => field.Property,
        ITeNumericFieldOption field => field.Property,
        ITeSelectBoxOption field => field.Property,
        ITeRadioOption field => field.Property,
        ITeCheckBoxOption field => field.Property,
        ITeSwitchOption field => field.Property,
        ITeAutocompleteOption field => field.Property,
        _ => null
    };

    public static bool IsReadOnly(TeEditNode node) => Property(node.Option) switch
    {
        TeBaseInputProperty property => property.Disabled || property.ReadOnly,
        TeBooleanInputProperty property => property.Disabled || property.ReadOnly,
        TeRadioGroupProperty property => property.Disabled || property.ReadOnly,
        _ => false
    };

    public static IEnumerable<MemberInfo> EditableMembers(Type type) => type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
        .Where(property => property.GetMethod?.IsPublic == true && property.SetMethod?.IsPublic == true && property.GetIndexParameters().Length == 0 && property.PropertyType != typeof(Type))
        .Cast<MemberInfo>().Concat(type.GetFields(BindingFlags.Public | BindingFlags.Instance).Where(field => !field.IsInitOnly && field.FieldType != typeof(Type)))
        .Where(member => member.GetCustomAttribute<TeIgnoreAttribute>() is null);

    public static Type MemberType(MemberInfo member) => member switch { PropertyInfo property => property.PropertyType, FieldInfo field => field.FieldType, _ => typeof(object) };
    public static object? ReadMember(MemberInfo member, object value) => member switch { PropertyInfo property => property.GetValue(value), FieldInfo field => field.GetValue(value), _ => null };
    private static void WriteMember(MemberInfo member, object value, object? newValue)
    {
        if (member is PropertyInfo property) property.SetValue(value, newValue);
        else if (member is FieldInfo field) field.SetValue(value, newValue);
    }
    public static object? Coerce(object? value, Type type)
    {
        if (value is null) return Nullable.GetUnderlyingType(type) is not null || !type.IsValueType ? null : Activator.CreateInstance(type);
        var actual = Nullable.GetUnderlyingType(type) ?? type;
        if (actual.IsInstanceOfType(value)) return value;
        if (actual.IsEnum) return Enum.Parse(actual, Convert.ToString(value, CultureInfo.InvariantCulture)!, true);
        if (actual == typeof(nint)) return nint.Parse(value.ToString()!, CultureInfo.InvariantCulture);
        if (actual == typeof(nuint)) return nuint.Parse(value.ToString()!, CultureInfo.InvariantCulture);
        var converter = TypeDescriptor.GetConverter(actual);
        return converter.CanConvertFrom(value.GetType()) ? converter.ConvertFrom(null, CultureInfo.InvariantCulture, value)
            : Convert.ChangeType(value, actual, CultureInfo.InvariantCulture);
    }

    private static ITeFieldOption? DefaultOption(Type type)
    {
        if (type == typeof(string) || type == typeof(char)) return new TeTextFieldOption();
        if (type == typeof(bool)) return new TeCheckBoxOption();
        if (type == typeof(DateTime)) return new TeTextFieldOption<DateTime> { Converter = new() { StringValue = value => value.ToString("O"), FromString = value => DateTime.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind) } };
        if (type == typeof(TimeSpan)) return new TeTextFieldOption<TimeSpan> { Converter = new() { StringValue = value => value.ToString(), FromString = value => TimeSpan.Parse(value, CultureInfo.InvariantCulture) } };
        if (type == typeof(DateTimeOffset)) return new TeTextFieldOption<DateTimeOffset> { Converter = new() { StringValue = value => value.ToString("O"), FromString = value => DateTimeOffset.Parse(value, CultureInfo.InvariantCulture) } };
        if (type == typeof(Guid)) return new TeTextFieldOption<Guid> { Converter = new() { StringValue = value => value.ToString(), FromString = Guid.Parse } };
        if (type.IsEnum) return null;
        if (type.IsPrimitive && type != typeof(IntPtr) && type != typeof(UIntPtr) || type == typeof(decimal) || type == typeof(nint) || type == typeof(nuint))
            return (ITeFieldOption?)Activator.CreateInstance(typeof(TeNumericFieldOption<>).MakeGenericType(type));
        return null;
    }

    private static Exception Unwrap(Exception exception) => exception is TargetInvocationException { InnerException: { } inner } ? inner : exception;
}

public sealed class TeEditNode
{
    private readonly Action<object?> _setter;
    internal TeEditNode(TeEditSession session, object? value, Type type, object? parent, MemberInfo? member, string path, string label, Action<object?> setter)
    { Session = session; Value = value; DeclaredType = type; Parent = parent; Member = member; Path = path; Label = label; _setter = setter; }
    public TeEditSession Session { get; }
    public object? Value { get; private set; }
    public Type DeclaredType { get; }
    public object? Parent { get; }
    public MemberInfo? Member { get; }
    public string Path { get; }
    public string Label { get; }
    public ITeFieldOption? Option;
    public IList? List { get; internal set; }
    public List<TeEditNode> Children { get; } = [];
    public string? Diagnostic { get; internal set; }
    public string[] Errors { get; internal set; } = [];
    public bool IsDirty { get; internal set; }
    public string? ConversionError { get; internal set; }
    internal void AssignSource(object? value) { _setter(value); Value = value; }
    public object? FieldValue => Option is ITeConvertable option ? option.Converter.ToField(Value!) : Value;
}
