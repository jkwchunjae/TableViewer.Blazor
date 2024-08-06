using TableViewerBlazor.Internal.TeComponent;
using TableViewerBlazor.Options.Property;

namespace TableViewerBlazor.Options;

public static class TeRadioOptionExtensions
{
    public static bool TryGetRadioOption(this TeOptions options,
        MemberInfo? memberInfo, TeEditorBase teBase,
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
        radioOption = default;
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

public class TeRadioOption<TValue> : ITeRadioOption
{
    public string? Id { get; set; }
    public List<ITeValidation> Validations { get; set; } = [];
    public required List<TeRadioItem<TValue>> Items { get; set; }
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

public record TeRadioItem<TValue> : ITeRadioItem
{
    public TValue? Value { get; init; } = default!;
    public string Text { get; init; } = string.Empty;
    public bool Default { get; init; } = false;
    public TeRadioItemProperty? Property { get; set; }

    object? ITeRadioItem.Value => Value;
    ITeRadioItemProperty? ITeRadioItem.Property => Property;

    public TeRadioItem()
    {
    }
    public TeRadioItem(TValue value, string text, bool @default = false)
    {
        Value = value;
        Text = text;
        Default = @default;
    }
}
