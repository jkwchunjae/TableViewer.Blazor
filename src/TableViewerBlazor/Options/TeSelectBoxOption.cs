using TableViewerBlazor.Options.Property;

namespace TableViewerBlazor.Options;

public class TeSelectBoxAttribute : TeFieldAttribute
{
    public TeSelectBoxAttribute(string id)
        : base(id)
    {
    }
}

public interface ITeSelectBoxOption : ITeFieldOption
{
    IEnumerable<ITeSelectItem> Items { get; }
    TeSelectBoxProperty Property { get; }
}

public class TeSelectBoxOption<TValue> : ITeSelectBoxOption, ITeGenericTypeOption
{
    public string? Id { get; set; }
    public List<TeSelectItem<TValue>> Items { get; set; } = [];
    public TeSelectBoxProperty<TValue> Property { get; set; } = new TeSelectBoxProperty<TValue>();
    public string TypeName => typeof(TValue).Name;

    IEnumerable<ITeSelectItem> ITeSelectBoxOption.Items => Items;
    TeSelectBoxProperty ITeSelectBoxOption.Property => Property;
}

public interface ITeSelectItem
{
    object? Value { get; }
    string Text { get; }
    bool Default { get; }
}

public record TeSelectItem<T> : ITeSelectItem
{
    public T? Value { get; init; } = default!;
    public string Text { get; init; } = string.Empty;
    public bool Default { get; init; } = false;

    object? ITeSelectItem.Value => Value;

    public TeSelectItem()
    {
    }
    public TeSelectItem(T value, string text, bool @default = false)
    {
        Value = value;
        Text = text;
        Default = @default;
    }
}
