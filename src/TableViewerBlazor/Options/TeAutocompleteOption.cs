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

    /// <summary>
    /// <para>
    /// @itemText - filter가 되는 string (StringConverter 존재 시, Converter에 의해 변환된 string)
    /// </para>
    /// <para>
    /// @inputValue - 유저가 입력한 input value (filtering 기준)
    /// </para>
    /// <remarks>
    /// Defaults to: (itemText, inputValue) => itemText?.Contains(inputValue, StringComparison.InvariantCultureIgnoreCase) ?? false
    /// </remarks>
    /// </summary>
    public Func<string, string, bool> CustomSearchFilter { get; }
}

public class TeAutocompleteOption<TValue> : ITeAutocompleteOption
{
    public string? Id { get; set; }
    public string TypeName => typeof(TValue).FullName!;
    public TeAutocompleteProperty Property { get; set; } = new();
    public List<TeAutocompleteItem<TValue>> Items { get; set; } = [];
    IEnumerable<ITeAutocompleteItem> ITeAutocompleteOption.Items => Items;
    public Func<TeAutocompleteItem<TValue>, string>? StringConverter { get; set; }

    Func<ITeAutocompleteItem, string> ITeAutocompleteOption.StringConverter => obj =>
    {
        
        if (obj is TeAutocompleteItem<TValue> item)
        {
            if (StringConverter == null)
            {
                return item.Value?.ToString() ?? string.Empty;
            }
            else
            {
                return StringConverter(item);
            }
        }
        else
        {
            return obj?.ToString() ?? string.Empty;
        }
    };

    public Func<string, string, bool> CustomSearchFilter { get; set; } = (itemText, inputValue) => itemText?.Contains(inputValue, StringComparison.InvariantCultureIgnoreCase) ?? false;
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