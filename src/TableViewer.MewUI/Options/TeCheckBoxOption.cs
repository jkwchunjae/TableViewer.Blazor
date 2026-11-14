using TableViewer.MewUI.Options.Property;

namespace TableViewer.MewUI.Options;



public interface ITeCheckBoxOption : ITeConvertableFieldOption<object, bool>
{
    public TeCheckBoxProperty Property { get; }
}

public class TeCheckBoxOption<TValue> : global::TableViewer.Share.Options.TeFieldOption<TValue, TeCheckBoxProperty>, ITeCheckBoxOption, ITeGenericTypeOption
{

    public required TeBoolConverter<TValue> Converter { get; set; }

    ITeConverter ITeConvertable.Converter => Converter;
    ITeConverter<object, bool> ITeConvertableFieldOption<object, bool>.Converter => new TeConverter<object, bool>
    {
        ToField = userValue => userValue is TValue value ? Converter.ToBoolean(value) : false,
        FromField = fieldValue => fieldValue is bool value ? Converter.FromBoolean(value) : default,
    };
}

public class TeCheckBoxOption : global::TableViewer.Share.Options.TeFieldOption<bool, TeCheckBoxProperty>, ITeCheckBoxOption
{

    ITeConverter ITeConvertable.Converter => new TeBoolConverter();
    ITeConverter<object, bool> ITeConvertableFieldOption<object, bool>.Converter => new TeConverter<object, bool>
    {
        ToField = userValue => userValue is bool value ? value : false,
        FromField = fieldValue => fieldValue,
    };
}
