namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeAutocomplete : TeEditorBase
{
    [Parameter] public ITeAutocompleteOption AutocompleteOption { get; set; } = default!;

    private IEnumerable<ITeAutocompleteItem>? items;
    private ITeAutocompleteItem selectedItem = default!;
    

    protected override void OnInitialized()
    {
        items = AutocompleteOption.Items;
    }

    private async Task OnValueChanged(ITeAutocompleteItem value)
    {
        selectedItem = value;
        await DataChanged.InvokeAsync(value.Value);
    }

    private async Task<IEnumerable<ITeAutocompleteItem>> SearchFunction(string value, CancellationToken token)
    {
        if (string.IsNullOrEmpty(value))
        {
            return items!;
        }
        else
        {
            return items!
                .Where(str => AutocompleteOption.CustomSearchFilter(StringConverter(str), value));
        }
    }

    private string StringConverter(ITeAutocompleteItem obj)
    {
        return AutocompleteOption.StringConverter(obj);
    }
}
