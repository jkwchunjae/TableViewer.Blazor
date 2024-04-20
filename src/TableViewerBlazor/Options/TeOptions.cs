namespace TableViewerBlazor.Options;

public class TeOptions
{
    public bool ReadProperty { get; set; } = true;
    public bool ReadField { get; set; } = true;
    public IEnumerable<ITeSelectBoxOption> SelectBoxOptions { get; set; } = new List<ITeSelectBoxOption>();
}

public class TeSelectBoxAttribute : Attribute
{
    public string Id { get; init; }
    public TeSelectBoxAttribute(string id)
    {
        Id = id;
    }
}

public interface ITeSelectItem
{
    string Text { get; }
    object? Value { get; }
    bool Default { get; }
}

public record TeSelectItem<T> : ITeSelectItem
{
    public string Text { get; init; } = string.Empty;
    public T? Value { get; init; } = default!;
    public bool Default { get; init; } = false;

    object? ITeSelectItem.Value => Value;

    public TeSelectItem()
    {
    }
    public TeSelectItem(string text, T value, bool @default = false)
    {
        Text = text;
        Value = value;
        Default = @default;
    }
}

public interface ITeSelectBoxOption
{
    public string Id { get; init; }
    public IEnumerable<ITeSelectItem> Items { get; }
    public Func<object?, int, string, bool> Condition { get; }
}

public class TeSelectBoxOption<T> : ITeSelectBoxOption
{
    public required string Id { get; init; }
    public IEnumerable<TeSelectItem<T>> Items { get; set; } = new List<TeSelectItem<T>>();
    public Func<T?, int, string, bool> Condition { get; set; } = (value, index, columnName) => true;

    IEnumerable<ITeSelectItem> ITeSelectBoxOption.Items => Items;
    Func<object?, int, string, bool> ITeSelectBoxOption.Condition =>
        (value, index, columnName) =>
        {
            if (value is T tValue)
            {
                return Condition?.Invoke(tValue, index, columnName) ?? true;
            }
            else
            {
                return false;
            }
        };
}