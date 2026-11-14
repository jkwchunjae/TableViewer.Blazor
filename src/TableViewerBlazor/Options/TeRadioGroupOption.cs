using TableViewerBlazor.Options.Property;

namespace TableViewerBlazor.Options;



public interface ITeRadioOption : ITeFieldOption
{
    IEnumerable<ITeValidation> Validations { get; }
    IEnumerable<ITeRadioItem> Items { get; }
    TeRadioGroupProperty Property { get; }
}

public class TeRadioOption<TValue> : global::TableViewer.Share.Options.TeValidatedFieldOption<TValue, TeRadioGroupProperty>, ITeRadioOption, ITeGenericTypeOption
{

    public required List<TeRadioItem<TValue>> Items { get; set; }

    IEnumerable<ITeValidation> ITeRadioOption.Validations => Validations;
    IEnumerable<ITeRadioItem> ITeRadioOption.Items => Items;
    TeRadioGroupProperty ITeRadioOption.Property => Property;
}

public interface ITeRadioItem
{
    object? Value { get; }
    string Text { get; }
    bool Default { get; }
    TeRadioItemProperty Property { get; }
}

public record TeRadioItem<TValue> : ITeRadioItem
{
    public TValue? Value { get; init; } = default!;
    public string Text { get; init; } = string.Empty;
    public bool Default { get; init; } = false;
    public TeRadioItemProperty Property { get; set; } = new TeRadioItemProperty();

    object? ITeRadioItem.Value => Value;
    TeRadioItemProperty ITeRadioItem.Property => Property;

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
