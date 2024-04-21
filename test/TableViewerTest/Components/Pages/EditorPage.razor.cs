using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TableViewerBlazor.Options;

namespace TableViewerTest.Components.Pages;

public class EditData
{
    public string Name { get; set; } = string.Empty;
    [TeSelectBox("gender")]
    public string Gender { get; set; } = string.Empty;
    [TeSelectBox("age")]
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
    EditData? viewData;
    TeOptions options = new TeOptions
    {
        SelectBoxOptions = new ITeSelectBoxOption[]
        {
            new TeSelectBoxOption<string>
            {
                Id = "gender",
                Items = new TeSelectItem<string>[]
                {
                    new TeSelectItem<string>("M", "남자"),
                    new TeSelectItem<string>("F", "여자"),
                },
            },
            new TeSelectBoxOption<int>
            {
                Id = "age",
                Items = new TeSelectItem<int>[]
                {
                    new TeSelectItem<int>(10, "10대"),
                    new TeSelectItem<int>(20, "20대"),
                    new TeSelectItem<int>(30, "30대"),
                    new TeSelectItem<int>(40, "40대"),
                    new TeSelectItem<int>(50, "50대"),
                },
            },
        },
    };
    private async Task Changed(EditData data)
    {
        viewData = data;
        await Js.InvokeVoidAsync("console.log", data);
    }
}
