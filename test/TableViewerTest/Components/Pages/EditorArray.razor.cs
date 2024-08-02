using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TableViewerBlazor.Options;

namespace TableViewerTest.Components.Pages;

public partial class EditorArray : ComponentBase
{
    [Inject] private IJSRuntime Js { get; set; } = default!;
    private readonly List<string> arrayData = ["1", "2", "배그", "bitter"];
    //private readonly List<bool> arrayData = [false];
    //private readonly List<object> arrayData = ["1", 2, "배그", "bitter", false];
    //private readonly List<object> arrayData = ["1", 2, "배그", "bitter", false, new TeOptions()];
    private readonly TeOptions teOptions = new()
    {
        Title = "array test",
        ArrayOptions = 
        [
            new TeArrayOption
            {
                ShowNumber = true,
                EnableAddItem = true,
            }
        ],
        TextFieldOptions =
        {
            new TeTextFieldOption<object>
            {
                Condition = (obj, _, _) => obj is bool,
                Converter = new TeTextFieldConverter<object>
                {
                    FromString = s => bool.TryParse(s, out var boolValue) ? boolValue : false,
                    StringValue = p =>  p?.ToString() ?? string.Empty,
                }
            },
        }
        //CheckBoxOptions =
        //[
        //    new TeCheckBoxOption
        //    {
        //        Property = new TeCheckBoxProperty
        //        {
        //            Color = MudBlazor.Color.Primary,
        //            //LabelOptions = new TeLabelOptions<>
        //            //{
        //            //    //SelectedLabel
        //            //}
        //        }
        //    }
        //]
    };

    private async Task OnDataChanged(List<string> data)
    {
        await Js.InvokeVoidAsync("console.log", data);
    }
}
