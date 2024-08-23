namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeAutocomplete : TeEditorBase
{
    [Parameter] public ITeAutocompleteOption AutocompleteOption { get; set; } = default!;

    private IEnumerable<ITeAutocompleteItem> items => AutocompleteOption.Items;
    private ITeAutocompleteItem selectedItem = default!;

    private async Task OnValueChanged(ITeAutocompleteItem value)
    {
        selectedItem = value;
        await DataChanged.InvokeAsync(value.Value);
    }

    private Task<IEnumerable<ITeAutocompleteItem>> SearchFunction(string value, CancellationToken token)
    {
        if (string.IsNullOrEmpty(value))
        {
            return Task.FromResult(items!);
        }
        else
        {
            return Task.FromResult(items!.Where(item => AutocompleteOption.CustomSearchFilter(item, value)));
        }
    }

    private string StringConverter(ITeAutocompleteItem obj)
    {
        return AutocompleteOption.StringConverter(obj);
    }
}
