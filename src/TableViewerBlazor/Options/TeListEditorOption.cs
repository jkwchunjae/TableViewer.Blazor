using TableViewerBlazor.Internal.TeComponent;

namespace TableViewerBlazor.Options;

public static class TeArrayOptionExtensions
{
    public static bool TryGetListEditorOption(this TeOptions options,
        MemberInfo? memberInfo, TeEditorBase teBase,
        out ITeListEditorOption? listEditorOption)
    {
        var listEditorAttribute = memberInfo?.GetCustomAttribute<TeListEditorAttribute>();
        if (listEditorAttribute != null)
        {
            listEditorOption = options.ListEditorOptions?
                .FirstOrDefault(o => o.Id == listEditorAttribute.Id) ?? default;
            if (listEditorOption != null)
            {
                return true;
            }
        }

        listEditorOption = options.ListEditorOptions?
            .Where(option => string.IsNullOrEmpty(option.Id))
            .FirstOrDefault(o => o.Condition?.Invoke(teBase.Data, teBase.Depth, teBase.Path) ?? true) ?? default;
        if (listEditorOption != null)
        {
            return true;
        }

        listEditorOption = teBase.Data switch
        {
            IList<string> => TeListEditorOption<string>.Create(string.Empty),
            IList<bool> => TeListEditorOption<bool>.Create(default),

            // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/integral-numeric-types
            IList<sbyte> => TeListEditorOption<sbyte>.Create(default),
            IList<byte> => TeListEditorOption<byte>.Create(default),
            IList<short> => TeListEditorOption<short>.Create(default),
            IList<ushort> => TeListEditorOption<ushort>.Create(default),
            IList<int> => TeListEditorOption<int>.Create(default),
            IList<uint> => TeListEditorOption<uint>.Create(default),
            IList<long> => TeListEditorOption<long>.Create(default),
            IList<ulong> => TeListEditorOption<ulong>.Create(default),
            IList<nint> => TeListEditorOption<nint>.Create(default),
            IList<nuint> => TeListEditorOption<nuint>.Create(default),
            // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/floating-point-numeric-types
            IList<float> => TeListEditorOption<float>.Create(default),
            IList<double> => TeListEditorOption<double>.Create(default),
            IList<decimal> => TeListEditorOption<decimal>.Create(default),

            _ => default,
        };
        return listEditorOption != null;
    }
}

public class TeListEditorAttribute : Attribute
{
    public string Id { get; init; }
    public TeListEditorAttribute(string id)
    {
        Id = id;
    }
}

public interface ITeListEditorOption : ITeFieldOption<object, IList>
{
    bool ShowNumber { get; }
    Func<object> CreateNew { get; }
    ITvAction AddItemAction { get; }
    ITvAction RemoveItemAction { get; }
}

public class TeListEditorOption<TValue, TListField, TListItem> : ITeListEditorOption
    where TListField : IList<TListItem>
{
    public string? Id { get; set; }
    public Func<TValue?, int, string, bool>? Condition { get; set; }
    public bool ShowNumber { get; set; } = false;
    public required Func<TListItem> CreateNew { get; init; }
    public ITvAction AddItemAction { get; set; } = CreateDefaultAddAction();
    public ITvAction RemoveItemAction { get; set; } = CreateDefaultRemoveAction();
    public required TeListEditorConverter<TValue, TListField, TListItem> Converter { get; set; }

    Func<object> ITeListEditorOption.CreateNew => () => CreateNew()!;
    ITeConverter ITeFieldOption.Converter => Converter;
    ITeConverter<object, IList> ITeFieldOption<object, IList>.Converter => new TeConverter<object, IList>
    {
        ToField = userValue => userValue is TValue value ? Converter.ToField(value).Cast<object>().ToList() : default,
        //FromField = fieldValue => fieldValue is TListField value ? Converter.FromField(value) : default,
        FromField = fieldValue =>
        {
            if (fieldValue.Cast<TListItem>().ToList() is TListField value)
            {
                return Converter.FromField(value);
            }
            else
            {
                return default;
            }
        }
    };

    Func<object?, int, string, bool>? ITeFieldOption.Condition =>
        (obj, depth, path) =>
        {
            if (obj is TValue value)
            {
                return Condition?.Invoke(value, depth, path) ?? true;
            }
            else
            {
                return false;
            }
        };

    private static TvAction<object> CreateDefaultAddAction()
    {
        return new TvAction<object>
        {
            Action = _ => Task.CompletedTask,
            Label = "Add Item",
            Style = new TvButtonStyle
            {
                Size = Size.Medium,
                Dense = true,
                SuperDense = false,
            },
        };
    }

    private static TvAction<object> CreateDefaultRemoveAction()
    {
        return new TvAction<object>
        {
            Action = _ => Task.CompletedTask,
            Label = string.Empty,
            Style = new TvButtonStyle
            {
                StartIcon = Icons.Material.Outlined.Cancel,
                IconColor = Color.Error,
                Dense = false,
                SuperDense = false,
            }
        };
    }
}

public class TeListEditorOption<TListItem> : ITeListEditorOption
{
    public string? Id { get; set; }
    public Func<IList<TListItem>?, int, string, bool>? Condition { get; set; }
    public bool ShowNumber { get; set; } = false;
    public required Func<TListItem> CreateNew { get; init; }
    public ITvAction AddItemAction { get; set; } = CreateDefaultAddAction();
    public ITvAction RemoveItemAction { get; set; } = CreateDefaultRemoveAction();

    Func<object> ITeListEditorOption.CreateNew => () => CreateNew()!;
    ITeConverter ITeFieldOption.Converter => new TeListEditorConverter<List<TListItem>>();
    ITeConverter<object, IList> ITeFieldOption<object, IList>.Converter => new TeConverter<object, IList>
    {
        ToField = userValue => userValue as IList,
        //FromField = fieldValue => fieldValue,
        FromField = fieldValue =>
        {
            if (fieldValue.Cast<TListItem>().ToList() is IList<TListItem> value)
            {
                return value;
            }
            else
            {
                return default;
            }
        }
    };

    Func<object?, int, string, bool>? ITeFieldOption.Condition =>
        (obj, depth, path) =>
        {
            if (obj is IList<TListItem> value)
            {
                return Condition?.Invoke(value, depth, path) ?? true;
            }
            else
            {
                return false;
            }
        };

    public static TeListEditorOption<TListItem> Create(TListItem defaultValue)
    {
        return new TeListEditorOption<TListItem>()
        {
            CreateNew = () => defaultValue,
        };
    }

    private static TvAction<object> CreateDefaultAddAction()
    {
        return new TvAction<object>
        {
            Action = _ => Task.CompletedTask,
            Label = "Add Item",
            Style = new TvButtonStyle
            {
                Size = Size.Medium,
                Dense = true,
                SuperDense = false,
            },
        };
    }

    private static TvAction<object> CreateDefaultRemoveAction()
    {
        return new TvAction<object>
        {
            Action = _ => Task.CompletedTask,
            Label = string.Empty,
            Style = new TvButtonStyle
            {
                StartIcon = Icons.Material.Outlined.Cancel,
                IconColor = Color.Error,
                Dense = false,
                SuperDense = false,
            }
        };
    }
}

public class TeListEditorConverter<TValue, TListField> : ITeConverter<TValue, TListField>
    where TListField : IList
{
    public required Func<TValue, TListField> ToList { get; set; }
    public required Func<TListField, TValue?> FromList { get; set; }

    public Func<TValue, TListField> ToField => userValue => ToList(userValue);
    public Func<TListField, TValue?> FromField => fieldValue => FromList(fieldValue);
    Func<object, object?> ITeConverter.ToField => userValue => userValue is TValue value ? ToList(value) : default;
    Func<object, object?> ITeConverter.FromField => fieldValue => fieldValue is TListField value ? FromList(value) : default;
}

public class TeListEditorConverter<TValue, TListField, TListItem> : ITeConverter<TValue, TListField>
    where TListField : IList<TListItem>
{
    public required Func<TValue, TListField> ToList { get; set; }
    public required Func<TListField, TValue?> FromList { get; set; }

    public Func<TValue, TListField> ToField => userValue => ToList(userValue);
    public Func<TListField, TValue?> FromField => fieldValue => FromList(fieldValue);
    Func<object, object?> ITeConverter.ToField => userValue => userValue is TValue value ? ToList(value) : default;
    Func<object, object?> ITeConverter.FromField => fieldValue => fieldValue is TListField value ? FromList(value) : default;
}

public class TeListEditorConverter<TList> : ITeConverter<TList, TList>
    where TList : IList
{
    public Func<TList, TList> ToField => userValue => userValue;
    public Func<TList, TList> FromField => fieldValue => fieldValue;
    Func<object, object?> ITeConverter.ToField => userValue => userValue;
    Func<object, object?> ITeConverter.FromField => fieldValue => fieldValue;
}

