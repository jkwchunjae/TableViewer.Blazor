namespace TableViewerBlazor.Options;

public static class TeNumericFieldOptionExtensions
{
    public static bool TryGetNumericFieldOption(this TeOptions options,
        MemberInfo? memberInfo, object? data,
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
            .FirstOrDefault(o => o.Condition?.Invoke(data, 0, "path") ?? true) ?? default;
        if (NumericFieldOption != null)
        {
            return true;
        }

        NumericFieldOption = data switch
        {
            int => new TeNumericFieldOption<int>(),
            long => new TeNumericFieldOption<long>(),
            float => new TeNumericFieldOption<float>(),
            double => new TeNumericFieldOption<double>(),
            decimal => new TeNumericFieldOption<decimal>(),
            byte => new TeNumericFieldOption<byte>(),
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
    IEnumerable<ITeValidation>? Validations { get; }
    new ITeNumericFieldProperty? Property { get; }
}

public class TeNumericFieldOption<T> : ITeFieldOption<T>, ITeNumericFieldOption
{
    public string? Id { get; set; }
    public Func<T?, int, string, bool>? Condition { get; set; }
    public IEnumerable<ITeValidation>? Validations { get; set; }
    public ITeNumericFieldProperty? Property { get; set; }

    ITeFieldProperty? ITeFieldOption.Property => Property;
}

public interface ITeNumericFieldProperty : ITeFieldProperty
{
    public bool FullWidth { get; set; }
}
