using TableViewerBlazor.Options.Property;

namespace TableViewerBlazor.Options;

public class TeTextFieldAttribute : TeFieldAttribute
{
    public TeTextFieldAttribute(string id)
        : base(id)
    {
    }
}

public interface ITeTextFieldOption : ITeConvertableFieldOption<object, string>
{
    IEnumerable<ITeValidation> Validations { get; }
    TeTextFieldProperty Property { get; }
    ITeTextFieldEvent? Event { get; }
}

public class TeTextFieldOption<TValue> : ITeTextFieldOption, ITeGenericTypeOption
{
    public string? Id { get; set; }
    public List<ITeValidation> Validations { get; set; } = [];
    public TeTextFieldProperty Property { get; set; } = new TeTextFieldProperty();
    public TeTextFieldEvent<TValue>? Event { get; set; }
    public required TeTextFieldConverter<TValue> Converter { get; set; }

    IEnumerable<ITeValidation> ITeTextFieldOption.Validations => Validations;
    public string TypeName => typeof(TValue).FullName!;
    ITeConverter ITeConvertable.Converter => Converter;
    ITeConverter<object, string> ITeConvertableFieldOption<object, string>.Converter => new TeConverter<object, string>()
    {
        ToField = value => value is TValue tValue ? Converter.ToField(tValue) : string.Empty,
        FromField = value => Converter.FromField(value) ?? default,
    };
    ITeTextFieldEvent? ITeTextFieldOption.Event => Event;
}

public class TeTextFieldOption : ITeTextFieldOption
{
    public string? Id { get; set; }
    public List<ITeValidation> Validations { get; set; } = [];
    public TeTextFieldProperty Property { get; set; } = new TeTextFieldProperty();
    public TeTextFieldEvent<string>? Event { get; set; }

    IEnumerable<ITeValidation> ITeTextFieldOption.Validations => Validations;
    ITeConverter ITeConvertable.Converter => new TeTextFieldConverter();
    ITeConverter<object, string> ITeConvertableFieldOption<object, string>.Converter => new TeConverter<object, string>()
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
