using Microsoft.JSInterop;

namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeSelectBox : ComponentBase
{
    [Inject] IJSRuntime Js { get; set; } = default!;
    [Parameter] public ITeSelectBoxOption SelectBoxOption { get; set; } = default!;
    [Parameter] public object? Data { get; set; }
    [Parameter] public EventCallback<object?> DataChanged { get; set; }

    private async Task OnValueChanged(object? value)
    {
        await Js.InvokeVoidAsync("console.log", value);
        Data = value;
        await DataChanged.InvokeAsync(Data);
    }
    string Converter1(object? value)
    {
        if (value == null)
        {
            return string.Empty;
        }
        var text = SelectBoxOption.Items
            .FirstOrDefault(item => item.Value == value)?.Text ?? value.ToString()!;
        return text + "a";
    }
}