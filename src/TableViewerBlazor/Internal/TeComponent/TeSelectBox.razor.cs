using Microsoft.JSInterop;

namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeSelectBox : TeEditorBase
{
    [Parameter] public ITeSelectBoxOption SelectBoxOption { get; set; } = default!;

    private async Task OnValueChanged(object? value)
    {
        Data = value;
        await DataChanged.InvokeAsync(Data);
    }
}