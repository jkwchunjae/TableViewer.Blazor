namespace TableViewer.Share.Options;

public interface ITeAutocompleteItem
{
    public object Value { get; }
}

public record TeAutocompleteItem : ITeAutocompleteItem
{
    public string Value { get; init; }
    object ITeAutocompleteItem.Value => Value!;

    public TeAutocompleteItem(string value)
    {
        Value = value;
    }
}

public record TeAutocompleteItem<T> : ITeAutocompleteItem
{
    public T Value { get; init; }
    object ITeAutocompleteItem.Value => Value!;

    public TeAutocompleteItem(T value)
    {
        Value = value;
    }
}
