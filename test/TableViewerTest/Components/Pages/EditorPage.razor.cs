using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace TableViewerTest.Components.Pages;

public class EditData
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}

public partial class EditorPage : ComponentBase
{
    [Inject] private IJSRuntime Js { get; set; } = default!;

    EditData data = new EditData
    {
        Name = "John",
        Age = 30,
    };
    private async Task Changed(EditData data)
    {
        await Js.InvokeVoidAsync("console.log", data);
    }
}
