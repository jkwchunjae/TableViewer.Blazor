using System.Numerics;
using TableViewerBlazor.Options.Property;

namespace TableViewerBlazor.Options;

public class TeNumericFieldAttribute : TeFieldAttribute
{
    public TeNumericFieldAttribute(string id)
        : base(id)
    {
    }
}

public interface ITeNumericFieldOption : ITeConvertableFieldOption<object, object>
{
    IEnumerable<ITeValidation> Validations { get; }
    TeNumericFieldProperty Property { get; }
    object DefaultValue { get; }
}

public class TeNumericFieldOption<TValue, TNumber> : ITeNumericFieldOption, ITeGenericTypeOption
    where TNumber: INumber<TNumber>, IMinMaxValue<TNumber>
{
    public string? Id { get; set; }
    public List<ITeValidation> Validations { get; set; } = [];
    public TeNumericFieldProperty<TNumber> Property { get; set; } = new TeNumericFieldProperty<TNumber>();
    public required TeNumericFieldConverter<TValue, TNumber> Converter { get; set; }
    public string TypeName => typeof(TValue).Name;

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

public class TeNumericFieldOption<TNumber> : ITeNumericFieldOption
    where TNumber: INumber<TNumber>, IMinMaxValue<TNumber>
{
    public string? Id { get; set; }
    public List<ITeValidation> Validations { get; set; } = [];
    public TeNumericFieldProperty<TNumber> Property { get; set; } = new TeNumericFieldProperty<TNumber>();

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

public class TeNumericFieldConverter<TValue, TNumber> : ITeConverter<TValue, TNumber>
    where TNumber: INumber<TNumber>, IMinMaxValue<TNumber>
{
    public required Func<TValue, TNumber?> ToNumber { get; set; }
    public required Func<TNumber, TValue?> FromNumber { get; set; }

    public Func<TValue, TNumber?> ToField => userValue => ToNumber(userValue);
    public Func<TNumber, TValue?> FromField => fieldValue => FromNumber(fieldValue);
    Func<object, object?> ITeConverter.ToField => userValue => userValue is TValue value ? ToNumber(value) : TNumber.MinValue;
    Func<object, object?> ITeConverter.FromField => fieldValue => fieldValue is TNumber value ? FromNumber(value) : default(TValue);
}

public class TeNumericFieldConverter<TNumber> : ITeConverter<TNumber, TNumber>
    where TNumber: INumber<TNumber>, IMinMaxValue<TNumber>
{
    public Func<TNumber, TNumber?> ToField => userValue => userValue;
    public Func<TNumber, TNumber?> FromField => fieldValue => fieldValue;
    Func<object, object?> ITeConverter.ToField => userValue => userValue is TNumber TNumberValue ? ToField(TNumberValue) : TNumber.MinValue;
    Func<object, object?> ITeConverter.FromField => fieldValue => fieldValue is TNumber TNumberValue ? FromField(TNumberValue) : TNumber.MinValue;
}

