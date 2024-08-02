using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TableViewerBlazor.Options;

namespace TableViewerTest.Components.Pages;

public partial class EditorArray : ComponentBase
{
    [Inject] private IJSRuntime Js { get; set; } = default!;
    private readonly List<string> arrayData = ["1", "2", "배그", "bitter"];
    private List<int> intList = new() { 123, 4567, };
    private List<bool> boolList = new() { true, false, };
    //private readonly List<object> arrayData = ["1", 2, "배그", "bitter", false];
    //private readonly List<object> arrayData = ["1", 2, "배그", "bitter", false, new TeOptions()];
    private readonly TeOptions teOptions = new()
    {
        Title = "string list",
    };
    private TeOptions intOptions = new()
    {
        Title = "int list",
    };
    private TeOptions boolOptions = new()
    {
        Title = "bool list",
    };

    private async Task OnDataChanged<T>(T data)
    {
        await Js.InvokeVoidAsync("console.log", data);
    }
}
