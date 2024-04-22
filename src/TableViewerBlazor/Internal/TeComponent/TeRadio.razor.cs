namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeRadio : TeEditorBase
{
    [Parameter] public ITeRadioOption RadioOption { get; set; } = default!;
    private async Task OnValueChanged(object? value)
    {
        Data = value;
        await DataChanged.InvokeAsync(value);
    }
}
