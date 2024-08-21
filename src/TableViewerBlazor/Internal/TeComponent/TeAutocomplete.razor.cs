namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeAutocomplete : TeEditorBase
{
    [Parameter] public ITeAutocompleteOption AutocompleteOption { get; set; } = default!;

    private IEnumerable<object>? items;
    private object inputValue = string.Empty; // 이거 확인
    

    protected override void OnInitialized()
    {
        if (Data is IEnumerable<object> data)
        {
            items = data;
        }
    }

    private async Task OnValueChanged(object value)
    {
        inputValue = value;
        // 터짐
        //await DataChanged.InvokeAsync(Data);
    }

    private async Task<IEnumerable<object>> SearchFunction(string value, CancellationToken token)
    {
        if (string.IsNullOrEmpty(value))
        {
            return items!;
        }
        else
        {
            return items!
                .Where(x => StringConverter(x)
                    ?.Contains(value, StringComparison.InvariantCultureIgnoreCase) ?? false);
        }
    }

    private string StringConverter(object obj)
    {
        // 일단 항상 option으로 받게 구현
        return AutocompleteOption.StringConverter(obj);
    }
}
