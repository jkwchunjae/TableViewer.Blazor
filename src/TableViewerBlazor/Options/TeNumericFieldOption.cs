using System.Numerics;
using TableViewerBlazor.Internal.TeComponent;
using TableViewerBlazor.Options.Property;

namespace TableViewerBlazor.Options;

public static class TeNumericFieldOptionExtensions
{
    public static bool TryGetNumericFieldOption(this TeOptions options,
        MemberInfo? memberInfo, TeEditorBase teBase,
        out ITeNumericFieldOption? NumericFieldOption,
        out object numericValue)
    {
        var NumericFieldAttribute = memberInfo?.GetCustomAttribute<TeNumericFieldAttribute>();
        if (NumericFieldAttribute != null)
        {
            NumericFieldOption = options.NumericFieldOptions?
                .FirstOrDefault(o => o.Id == NumericFieldAttribute.Id) ?? default;
            if (NumericFieldOption != null)
            {
                numericValue = NumericFieldOption.DefaultValue;
                return true;
            }
        }

        NumericFieldOption = teBase.Data switch
        {
            // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/integral-numeric-types
            sbyte => new TeNumericFieldOption<sbyte>(),
            byte => new TeNumericFieldOption<byte>(),
            short => new TeNumericFieldOption<short>(),
            ushort => new TeNumericFieldOption<ushort>(),
            int => new TeNumericFieldOption<int>(),
            uint => new TeNumericFieldOption<uint>(),
            long => new TeNumericFieldOption<long>(),
            ulong => new TeNumericFieldOption<ulong>(),
            nint => new TeNumericFieldOption<nint>(),
            nuint => new TeNumericFieldOption<nuint>(),
            // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/floating-point-numeric-types
            float => new TeNumericFieldOption<float>(),
            double => new TeNumericFieldOption<double>(),
            decimal => new TeNumericFieldOption<decimal>(),
            _ => null,
        };

        if (NumericFieldOption != null)
        {
            numericValue = NumericFieldOption.DefaultValue;
            return true;
        }
        else
        {
            numericValue = new();
            return false;
        }
    }
}

public class TeNumericFieldAttribute : Attribute
{
    public string Id { get; init; }
    public TeNumericFieldAttribute(string id)
    {
        Id = id;
    }
}

public interface ITeNumericFieldOption : ITeFieldOption<object, object>
{
    IEnumerable<ITeValidation> Validations { get; }
    ITeNumericFieldProperty? Property { get; }
    object DefaultValue { get; }
}

public class TeNumericFieldOption<TValue, TNumber> : ITeNumericFieldOption
    where TNumber: INumber<TNumber>, IMinMaxValue<TNumber>
{
    public string? Id { get; set; }
    public List<ITeValidation> Validations { get; set; } = [];
    public TeNumericFieldProperty<TNumber>? Property { get; set; }
    public required TeNumericFieldConverter<TValue, TNumber> Converter { get; set; }

    public object DefaultValue => TNumber.Zero;
    ITeConverter ITeFieldOption.Converter => Converter;
    IEnumerable<ITeValidation> ITeNumericFieldOption.Validations => Validations;
    ITeConverter<object, object> ITeFieldOption<object, object>.Converter => new TeConverter<object, object>
    {
        ToField = value => value is TValue tValue ? Converter.ToField(tValue) : DefaultValue,
        FromField = value => value is TNumber number ? Converter.FromField(number) : DefaultValue,
    };
    ITeNumericFieldProperty? ITeNumericFieldOption.Property => Property;
}

public class TeNumericFieldOption<TNumber> : ITeNumericFieldOption
    where TNumber: INumber<TNumber>, IMinMaxValue<TNumber>
{
    public string? Id { get; set; }
    public List<ITeValidation> Validations { get; set; } = [];
    public TeNumericFieldProperty<TNumber>? Property { get; set; }

    public object DefaultValue => TNumber.Zero;
    ITeConverter ITeFieldOption.Converter => new TeNumericFieldConverter<TNumber>();
    IEnumerable<ITeValidation> ITeNumericFieldOption.Validations => Validations;
    ITeConverter<object, object> ITeFieldOption<object, object>.Converter => new TeConverter<object, object>
    {
        ToField = value => value is TNumber TNumberValue ? TNumberValue : TNumber.MinValue,
        FromField = value => value is TNumber TNumberValue ? TNumberValue : TNumber.MinValue,
    };
    ITeNumericFieldProperty? ITeNumericFieldOption.Property => Property;
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

