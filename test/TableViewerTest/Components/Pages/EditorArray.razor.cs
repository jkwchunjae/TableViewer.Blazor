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
    private string arrayDataString = "67328";
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

    /// <summary>
    /// 숫자로 이루어진 문자열을 리스트로 변환하는 옵션
    /// </summary>
    private TeOptions stringNumberListOption = new()
    {
        Title = "string number list",
        ListEditorOptions = new List<ITeListEditorOption>
        {
            new TeListEditorOption<string, List<int>, int>
            {
                // 1. string을 List<int>로 변환
                // 2. List<int>를 string으로 변환
                Converter = new TeListEditorConverter<string, List<int>, int>()
                {
                    ToList = userValue => userValue.Select(x => int.Parse(x.ToString())).ToList(),
                    FromList = fieldValue => new string(fieldValue.SelectMany(i => i.ToString()).ToArray()),
                },
                // 3. AddItemAction을 통해 List<int>에 값을 추가
                AddItemAction = new TvAction<List<int>>
                {
                    Action = list =>
                    {
                        list.Add(0);
                        return Task.CompletedTask;
                    },
                    Label = "Add Number",
                    Style = new TvButtonStyle
                    {
                        Size = MudBlazor.Size.Medium,
                        Dense = true,
                        SuperDense = false,
                    },
                },
            }
        }
    };

    private async Task OnDataChanged<T>(T data)
    {
        await Js.InvokeVoidAsync("console.log", data);
    }

    private async Task OnStringDataChanged(string data)
    {
        arrayDataString = data;
        await Js.InvokeVoidAsync("console.log", data);
    }

}
