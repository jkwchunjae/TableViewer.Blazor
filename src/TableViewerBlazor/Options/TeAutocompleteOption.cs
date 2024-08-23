using TableViewerBlazor.Options.Property;

namespace TableViewerBlazor.Options;

public class TeAutocompleteAttribute : TeFieldAttribute
{
    public TeAutocompleteAttribute(string id) : base(id) { }
}

public interface ITeAutocompleteOption : ITeGenericTypeOption
{
    public TeAutocompleteProperty Property { get; }
    public IEnumerable<ITeAutocompleteItem> Items { get; }
    public Func<ITeAutocompleteItem, string> StringConverter { get; }
    public Func<ITeAutocompleteItem, string, bool> CustomSearchFilter { get; }
}

public class TeAutocompleteOption<TValue> : ITeAutocompleteOption
{
    public string? Id { get; set; }
    public string TypeName => typeof(TValue).FullName!;
    public TeAutocompleteProperty Property { get; set; } = new();
    public List<TeAutocompleteItem<TValue>> Items { get; set; } = [];
    IEnumerable<ITeAutocompleteItem> ITeAutocompleteOption.Items => Items;
    public required Func<TeAutocompleteItem<TValue>, string>? StringConverter { get; set; }

    Func<ITeAutocompleteItem, string> ITeAutocompleteOption.StringConverter => obj =>
    {
        if (obj is TeAutocompleteItem<TValue> item)
        {
            if (StringConverter != null)
            {
                return StringConverter(item);
            }
            else
            {
                return item.Value?.ToString() ?? string.Empty;
            }
        }
        else
        {
            return obj?.ToString() ?? string.Empty;
        }
    };

    /// <summary>
    /// <para>
    /// @param1 - TeAutocompleteItem item: 아이템 데이터
    /// </para>
    /// <para>
    /// @param2 - string inputValue: 유저가 입력 값
    /// </para>
    /// </summary>
    public Func<TeAutocompleteItem<TValue>, string, bool>? CustomSearchFilter { get; set; }

    Func<ITeAutocompleteItem, string, bool> ITeAutocompleteOption.CustomSearchFilter => (item, input) =>
    {
        if (item is TeAutocompleteItem<TValue> tItem)
        {
            if (CustomSearchFilter != null)
            {
                return CustomSearchFilter(tItem, input);
            }
            else if (StringConverter != null)
            {
                return StringConverter(tItem).Contains(input, StringComparison.InvariantCultureIgnoreCase);
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    };
}

public interface ITeAutocompleteItem
{
    public object Value { get; }
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