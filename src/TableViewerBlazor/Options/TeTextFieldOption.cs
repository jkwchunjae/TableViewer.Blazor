using TableViewerBlazor.Internal.TeComponent;
using TableViewerBlazor.Options.Property;

namespace TableViewerBlazor.Options;

public static class TeTextFieldOptionExtensions
{
    public static bool TryGetTextFieldOption(this TeOptions options,
        MemberInfo? memberInfo, TeEditorBase teBase,
        out ITeTextFieldOption? textFieldOption)
    {
        var textFieldAttribute = memberInfo?.GetCustomAttribute<TeTextFieldAttribute>();
        if (textFieldAttribute != null)
        {
            textFieldOption = options.TextFieldOptions?
                .FirstOrDefault(o => o.Id == textFieldAttribute.Id) ?? default;
            if (textFieldOption != null)
            {
                return true;
            }
        }

        textFieldOption = teBase.Data switch
        {
            string => new TeTextFieldOption(),
            _ => null,
        };
        if (teBase.Data == null && memberInfo?.MemberType() == typeof(string))
        {
            textFieldOption = new TeTextFieldOption();
        }
        if (textFieldOption != null)
        {
            return true;
        }
        if (memberInfo != null)
        {
            var memberType = MemberType(memberInfo);
            textFieldOption = options.TextFieldOptions?
                .Where(option => option.GetType().GenericTypeArguments.Any())
                .FirstOrDefault(option => option.TypeName == memberType.Name);
            if (textFieldOption != null)
            {
                return true;
            }
        }
        return false;

        Type MemberType(MemberInfo memberInfo)
        {
            if (memberInfo is PropertyInfo property)
            {
                return property.PropertyType;
            }
            if (memberInfo is FieldInfo field)
            {
                return field.FieldType;
            }
            throw new Exception();
        }
    }
}

public class TeTextFieldAttribute : Attribute
{
    public string Id { get; init; }
    public TeTextFieldAttribute(string id)
    {
        Id = id;
    }
}

public interface ITeTextFieldOption : ITeFieldOption<object, string>
{
    IEnumerable<ITeValidation> Validations { get; }
    ITeTextFieldProperty? Property { get; }
    ITeTextFieldEvent? Event { get; }
    string TypeName { get; }
}

public class TeTextFieldOption<TValue> : ITeTextFieldOption
{
    public string? Id { get; set; }
    public IEnumerable<ITeValidation> Validations { get; set; } = [];
    public ITeTextFieldProperty? Property { get; set; }
    public TeTextFieldEvent<TValue>? Event { get; set; }
    public required TeTextFieldConverter<TValue> Converter { get; set; }

    public string TypeName => typeof(TValue).Name;
    ITeConverter ITeFieldOption.Converter => Converter;
    ITeConverter<object, string> ITeFieldOption<object, string>.Converter => new TeConverter<object, string>()
    {
        ToField = value => value is TValue tValue ? Converter.ToField(tValue) : string.Empty,
        FromField = value => Converter.FromField(value) ?? default,
    };
    ITeTextFieldEvent? ITeTextFieldOption.Event => Event;
}

public class TeTextFieldOption : ITeTextFieldOption
{
    public string? Id { get; set; }
    public IEnumerable<ITeValidation> Validations { get; set; } = [];
    public ITeTextFieldProperty? Property { get; set; }
    public TeTextFieldEvent<string>? Event { get; set; }

    public string TypeName => typeof(string).Name;
    ITeConverter ITeFieldOption.Converter => new TeTextFieldConverter();
    ITeConverter<object, string> ITeFieldOption<object, string>.Converter => new TeConverter<object, string>()
    {
        ToField = value => value is string stringValue ? stringValue : string.Empty,
        FromField = value => value is string ? value : string.Empty,
    };
    ITeTextFieldEvent? ITeTextFieldOption.Event => Event;
}

public class TeTextFieldConverter<TValue> : ITeConverter<TValue, string>
{
    public required Func<TValue, string?> StringValue { get; set; }
    public required Func<string, TValue?> FromString { get; set; }

    public Func<TValue, string?> ToField => userValue => StringValue(userValue);
    public Func<string, TValue?> FromField => fieldValue => FromString(fieldValue);
    Func<object, object?> ITeConverter.ToField => userValue => userValue is TValue value ? StringValue(value) : string.Empty;
    Func<object, object?> ITeConverter.FromField => fieldValue => fieldValue is string value ? FromString(value) : default;
}

public class TeTextFieldConverter : ITeConverter<string, string>
{
    public Func<string, string?> ToField => userValue => userValue;
    public Func<string, string?> FromField => fieldValue => fieldValue;
    Func<object, object?> ITeConverter.ToField => userValue => userValue is string stringValue ? ToField(stringValue) : string.Empty;
    Func<object, object?> ITeConverter.FromField => fieldValue => fieldValue is string stringValue ? FromField(stringValue) : string.Empty;
}
