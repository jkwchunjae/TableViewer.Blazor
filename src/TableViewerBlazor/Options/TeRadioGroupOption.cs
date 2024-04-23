namespace TableViewerBlazor.Options;

public static class TeRadioOptionExtensions
{
    public static bool TryGetRadioOption(this TeOptions options,
        MemberInfo? memberInfo, object? data,
        out ITeRadioOption? radioOption)
    {
        var selectAttribute = memberInfo?.GetCustomAttribute<TeRadioAttribute>();
        if (selectAttribute != null)
        {
            radioOption = options.RadioOptions?
                .FirstOrDefault(o => o.Id == selectAttribute.Id) ?? default;
            if (radioOption != null)
            {
                return true;
            }
        }
        radioOption = options.RadioOptions?
            .Where(option => string.IsNullOrEmpty(option.Id))
            .FirstOrDefault(o => o.Condition(data, 0, "path")) ?? default;
        return radioOption != null;
    }
}

public class TeRadioAttribute : Attribute
{
    public string Id { get; init; }
    public TeRadioAttribute(string id)
    {
        Id = id;
    }
}

public interface ITeRadioOption : ITeFieldOption
{
    IEnumerable<ITeRadioItem> Items { get; }
    new ITeRadioGroupProperty? Property { get; set; }
}

public class TeRadioOption<T> : ITeFieldOption<T>, ITeRadioOption
{
    public string? Id { get; set; }
    public Func<T?, int, string, bool>? Condition { get; set; }
    public IEnumerable<ITeRadioItem> Items { get; set; } = new List<TeRadioItem<T>>();
    public ITeRadioGroupProperty? Property { get; set; }

    ITeFieldProperty? ITeFieldOption.Property => Property;
}

public interface ITeRadioItem
{
    object? Value { get; }
    string Text { get; }
    bool Default { get; }
}

public record TeRadioItem<T> : ITeRadioItem
{
    public T? Value { get; init; } = default!;
    public string Text { get; init; } = string.Empty;
    public bool Default { get; init; } = false;

    object? ITeRadioItem.Value => Value;

    public TeRadioItem()
    {
    }
    public TeRadioItem(T value, string text, bool @default = false)
    {
        Value = value;
        Text = text;
        Default = @default;
    }
}

public interface ITeRadioGroupProperty : ITeFieldProperty
{
}
