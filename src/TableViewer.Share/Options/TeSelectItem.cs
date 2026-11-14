namespace TableViewer.Share.Options;

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
