using TableViewerBlazor.Options.Property;

namespace TableViewerBlazor.Options;

public class TeRadioAttribute : TeFieldAttribute
{
    public TeRadioAttribute(string id)
        : base(id)
    {
    }
}

public interface ITeRadioOption : ITeFieldOption
{
    IEnumerable<ITeValidation> Validations { get; }
    IEnumerable<ITeRadioItem> Items { get; }
    TeRadioGroupProperty Property { get; }
}

public class TeRadioOption<TValue> : ITeRadioOption, ITeGenericTypeOption
{
    public string? Id { get; set; }
    public List<ITeValidation> Validations { get; set; } = [];
    public required List<TeRadioItem<TValue>> Items { get; set; }
    public TeRadioGroupProperty Property { get; set; } = new TeRadioGroupProperty();
    public string TypeName => typeof(TValue).Name;

    IEnumerable<ITeValidation> ITeRadioOption.Validations => Validations;
    IEnumerable<ITeRadioItem> ITeRadioOption.Items => Items;
    TeRadioGroupProperty ITeRadioOption.Property => Property;
}

public interface ITeRadioItem
{
    object? Value { get; }
    string Text { get; }
    bool Default { get; }
    TeRadioProperty Property { get; }
}

public record TeRadioItem<TValue> : ITeRadioItem
{
    public TValue? Value { get; init; } = default!;
    public string Text { get; init; } = string.Empty;
    public bool Default { get; init; } = false;
    public TeRadioProperty Property { get; set; } = new TeRadioProperty();

    object? ITeRadioItem.Value => Value;
    TeRadioProperty ITeRadioItem.Property => Property;

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
