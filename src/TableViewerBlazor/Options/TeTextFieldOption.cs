using TableViewerBlazor.Options.Property;

namespace TableViewerBlazor.Options;



public interface ITeTextFieldOption : ITeConvertableFieldOption<object, string>
{
    IEnumerable<ITeValidation> Validations { get; }
    TeTextFieldProperty Property { get; }
    ITeTextFieldEvent? Event { get; }
}

public class TeTextFieldOption<TValue> : global::TableViewer.Share.Options.TeValidatedFieldOption<TValue, TeTextFieldProperty>, ITeTextFieldOption, ITeGenericTypeOption
{

    public TeTextFieldEvent<TValue>? Event { get; set; }
    public required TeTextFieldConverter<TValue> Converter { get; set; }

    IEnumerable<ITeValidation> ITeTextFieldOption.Validations => Validations;

    ITeConverter ITeConvertable.Converter => Converter;
    ITeConverter<object, string> ITeConvertableFieldOption<object, string>.Converter => new TeConverter<object, string>()
    {
        ToField = value => value is TValue tValue ? Converter.ToField(tValue) : string.Empty,
        FromField = value => Converter.FromField(value) ?? default,
    };
    ITeTextFieldEvent? ITeTextFieldOption.Event => Event;
}

public class TeTextFieldOption : global::TableViewer.Share.Options.TeValidatedFieldOption<string, TeTextFieldProperty>, ITeTextFieldOption
{

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
