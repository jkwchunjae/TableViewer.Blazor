using System.Numerics;
using TableViewerBlazor.Internal.TeComponent;
using TableViewerBlazor.Options.Property;

namespace TableViewerBlazor.Options;

public static class TeNumericFieldOptionExtensions
{
    public static bool TryGetNumericFieldOption(this TeOptions options,
        MemberInfo? memberInfo, TeEditorBase teBase,
        out ITeNumericFieldOption? NumericFieldOption)
    {
        var NumericFieldAttribute = memberInfo?.GetCustomAttribute<TeNumericFieldAttribute>();
        if (NumericFieldAttribute != null)
        {
            NumericFieldOption = options.NumericFieldOptions?
                .FirstOrDefault(o => o.Id == NumericFieldAttribute.Id) ?? default;
            if (NumericFieldOption != null)
            {
                return true;
            }
        }

        NumericFieldOption = options.NumericFieldOptions?
            .Where(option => string.IsNullOrEmpty(option.Id))
            .FirstOrDefault(o => o.Condition?.Invoke(teBase.Data, teBase.Depth, teBase.Path) ?? true) ?? default;
        if (NumericFieldOption != null)
        {
            return true;
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
        return NumericFieldOption != null;
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

public interface ITeNumericFieldOption : ITeFieldOption
{
    IEnumerable<ITeValidation> Validations { get; }
    ITeNumericFieldProperty? Property { get; }
}

public class TeNumericFieldOption<T> : ITeFieldOption<T>, ITeNumericFieldOption
    where T: INumber<T>, IMinMaxValue<T>
{
    public string? Id { get; set; }
    public Func<T?, int, string, bool>? Condition { get; set; }
    public List<ITeValidation> Validations { get; set; } = [];
    public TeNumericFieldProperty<T>? Property { get; set; }

    IEnumerable<ITeValidation> ITeNumericFieldOption.Validations => Validations;
    ITeNumericFieldProperty? ITeNumericFieldOption.Property => Property;
}

