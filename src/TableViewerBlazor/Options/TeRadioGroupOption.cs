using TableViewerBlazor.Options.Property;

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
            .Where(option => option.Condition?.Invoke(data, 0, "path") ?? true)
            .FirstOrDefault() ?? default;
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
    IEnumerable<ITeValidation> Validations { get; }
    IEnumerable<ITeRadioItem> Items { get; }
    ITeRadioGroupProperty? Property { get; }
}

public class TeRadioOption<T> : ITeFieldOption<T>, ITeRadioOption
{
    public string? Id { get; set; }
    public Func<T?, int, string, bool>? Condition { get; set; }
    public List<ITeValidation> Validations { get; set; } = [];
    public List<TeRadioItem<T>> Items { get; set; } = [];
    public TeRadioGroupProperty? Property { get; set; }

    IEnumerable<ITeValidation> ITeRadioOption.Validations => Validations;
    IEnumerable<ITeRadioItem> ITeRadioOption.Items => Items;
    ITeRadioGroupProperty? ITeRadioOption.Property => Property;
}

public interface ITeRadioItem
{
    object? Value { get; }
    string Text { get; }
    bool Default { get; }
    ITeRadioItemProperty? Property { get; }
}

public record TeRadioItem<T> : ITeRadioItem
{
    public T? Value { get; init; } = default!;
    public string Text { get; init; } = string.Empty;
    public bool Default { get; init; } = false;
    public TeRadioItemProperty? Property { get; set; }

    object? ITeRadioItem.Value => Value;
    ITeRadioItemProperty? ITeRadioItem.Property => Property;

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
