using System.Numerics;
using TableViewer.MewUI.Options.Property;

namespace TableViewer.MewUI.Options;



public interface ITeNumericFieldOption : ITeConvertableFieldOption<object, object>
{
    IEnumerable<ITeValidation> Validations { get; }
    TeNumericFieldProperty Property { get; }
    object DefaultValue { get; }
}

public class TeNumericFieldOption<TValue, TNumber> : global::TableViewer.Share.Options.TeValidatedFieldOption<TValue, TeNumericFieldProperty<TNumber>>, ITeNumericFieldOption, ITeGenericTypeOption
    where TNumber: INumber<TNumber>, IMinMaxValue<TNumber>
{

    public required TeNumericFieldConverter<TValue, TNumber> Converter { get; set; }

    public object DefaultValue => TNumber.Zero;
    ITeConverter ITeConvertable.Converter => Converter;
    IEnumerable<ITeValidation> ITeNumericFieldOption.Validations => Validations;
    ITeConverter<object, object> ITeConvertableFieldOption<object, object>.Converter => new TeConverter<object, object>
    {
        ToField = value => value is TValue tValue ? Converter.ToField(tValue) : DefaultValue,
        FromField = value => value is TNumber number ? Converter.FromField(number) : DefaultValue,
    };
    TeNumericFieldProperty ITeNumericFieldOption.Property => Property;
}

public class TeNumericFieldOption<TNumber> : global::TableViewer.Share.Options.TeValidatedFieldOption<TNumber, TeNumericFieldProperty<TNumber>>, ITeNumericFieldOption
    where TNumber: INumber<TNumber>, IMinMaxValue<TNumber>
{

    public object DefaultValue => TNumber.Zero;
    ITeConverter ITeConvertable.Converter => new TeNumericFieldConverter<TNumber>();
    IEnumerable<ITeValidation> ITeNumericFieldOption.Validations => Validations;
    ITeConverter<object, object> ITeConvertableFieldOption<object, object>.Converter => new TeConverter<object, object>
    {
        ToField = value => value is TNumber TNumberValue ? TNumberValue : TNumber.MinValue,
        FromField = value => value is TNumber TNumberValue ? TNumberValue : TNumber.MinValue,
    };
    TeNumericFieldProperty ITeNumericFieldOption.Property => Property;
}
