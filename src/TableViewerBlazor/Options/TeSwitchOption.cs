using TableViewerBlazor.Options.Property;

namespace TableViewerBlazor.Options;

public class TeSwitchAttribute(string id) : TeFieldAttribute(id) { }

public interface ITeSwitchOption : ITeConvertableFieldOption<object, bool>
{
    ITeSwitchProperty? Property { get; }
}

public class TeSwitchOption : ITeSwitchOption
{
    public string? Id { get; init; }
    public ITeSwitchProperty Property { get; init; } = new TeSwitchProperty();
    ITeConverter ITeConvertable.Converter => new TeCheckBoxConverter();
    ITeConverter<object, bool> ITeConvertableFieldOption<object, bool>.Converter => new TeConverter<object, bool>
    {
        ToField = userValue => userValue is bool value ? value : false,
        FromField = fieldValue => fieldValue,
    };
}

public class TeSwitchOption<TValue> : ITeSwitchOption, ITeGenericTypeOption
{
    public string? Id { get; set; }
    public ITeSwitchProperty? Property { get; set; } = new TeSwitchProperty();
    public string TypeName => typeof(TValue).FullName!;
    public required TeCheckBoxConverter<TValue> Converter { get; set; }
    ITeConverter ITeConvertable.Converter => Converter;

    ITeConverter<object, bool> ITeConvertableFieldOption<object, bool>.Converter => new TeConverter<object, bool>
    {
        ToField = userValue => userValue is TValue value ? Converter.ToBoolean(value) : false,
        FromField = fieldValue => fieldValue is bool value ? Converter.FromBoolean(value) : default,
    };
}