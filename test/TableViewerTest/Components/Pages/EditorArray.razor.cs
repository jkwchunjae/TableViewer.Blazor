using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TableViewerBlazor.Options;
using TableViewerBlazor.Options.Property;

namespace TableViewerTest.Components.Pages;

public partial class EditorArray : ComponentBase
{
    [Inject] private IJSRuntime Js { get; set; } = default!;
    //private readonly List<string> arrayData = ["1", "2", "배그", "bitter"];
    private readonly List<bool> arrayData = [false];
    //private readonly List<object> arrayData = ["1", 2, "배그", "bitter", false];
    //private readonly List<object> arrayData = ["1", 2, "배그", "bitter", false, new TeOptions()];
    private readonly TeOptions teOptions = new()
    {
        Title = "array test",
        CheckBoxOptions =
        [
            new TeCheckBoxOption
            {
                Property = new TeCheckBoxProperty
                {
                    Color = MudBlazor.Color.Primary,
                    //LabelOptions = new TeLabelOptions<>
                    //{
                    //    //SelectedLabel
                    //}
                }
            }
        ]
    };

    private async Task OnDataChanged(List<bool> data)
    {
        await Js.InvokeVoidAsync("console.log", data);
    }
}
