using TableViewer.MewUI.Options.Property;

namespace TableViewer.MewUI.Options;



public interface ITeSwitchOption : ITeConvertableFieldOption<object, bool>
{
    TeSwitchProperty Property { get; }
}

public class TeSwitchOption : ITeSwitchOption
{
    public string? Id { get; init; }
    public TeSwitchProperty Property { get; init; } = new TeSwitchProperty();
    ITeConverter ITeConvertable.Converter => new TeBoolConverter();
    ITeConverter<object, bool> ITeConvertableFieldOption<object, bool>.Converter => new TeConverter<object, bool>
    {
        ToField = userValue => userValue is bool value ? value : false,
        FromField = fieldValue => fieldValue,
    };
}

public class TeSwitchOption<TValue> : global::TableViewer.Share.Options.TeFieldOption<TValue, TeSwitchProperty>, ITeSwitchOption, ITeGenericTypeOption
{

    public required TeBoolConverter<TValue> Converter { get; set; }
    ITeConverter ITeConvertable.Converter => Converter;

    ITeConverter<object, bool> ITeConvertableFieldOption<object, bool>.Converter => new TeConverter<object, bool>
    {
        ToField = userValue => userValue is TValue value ? Converter.ToBoolean(value) : false,
        FromField = fieldValue => fieldValue is bool value ? Converter.FromBoolean(value) : default,
    };
}
