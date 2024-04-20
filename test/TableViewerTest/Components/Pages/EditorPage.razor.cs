using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TableViewerBlazor.Options;

namespace TableViewerTest.Components.Pages;

public class EditData
{
    public string Name { get; set; } = string.Empty;
    [TeSelectBox("gender")]
    public string Gender { get; set; } = string.Empty;
    public int Age { get; set; }
}

public partial class EditorPage : ComponentBase
{
    [Inject] private IJSRuntime Js { get; set; } = default!;

    EditData data = new EditData
    {
        Name = "John",
        Gender = "M",
        Age = 30,
    };
    TeOptions options = new TeOptions
    {
        SelectBoxOptions = new ITeSelectBoxOption[]
        {
            new TeSelectBoxOption<string>
            {
                Id = "gender",
                Items = new TeSelectItem<string>[]
                {
                    new TeSelectItem<string>("남자", "M"),
                    new TeSelectItem<string>("여자", "F"),
                },
            },
        },
    };
    private async Task Changed(EditData data)
    {
        await Js.InvokeVoidAsync("console.log", data);
    }
}
