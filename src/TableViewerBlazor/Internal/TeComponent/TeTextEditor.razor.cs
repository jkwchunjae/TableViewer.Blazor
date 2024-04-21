namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeTextEditor : TeEditorBase
{
    private async Task OnValueChanged(object? value)
    {
        Data = value;
        await DataChanged.InvokeAsync(value);
    }
}
