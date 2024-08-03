using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TableViewerBlazor.Options;

namespace TableViewerTest.Components.Pages;

public class StringTestData
{
    [TeCheckBox(nameof(String))]
    public string String { get; set; } = string.Empty;
    [TeCheckBox(nameof(Number))]
    public int Number { get; set; }
    [TeCheckBox(nameof(DateTime))]
    public DateTime DateTime { get; set; }
    [TeCheckBox(nameof(Bool))]
    public bool Bool { get; set; } = true;
    [TeCheckBox(nameof(Enum))]
    public TestEnum Enum { get; set; }
    [TeCheckBox(nameof(IntList))]
    public List<int> IntList { get; set; } = new List<int> { 1, 2, 3, 4, 5 };
}

[Route("/editor-converter")]
public partial class EditorConverter : ComponentBase
{
    [Inject] public IJSRuntime Js { get; set; } = default!;
    StringTestData data = new StringTestData();
    TeOptions options = new TeOptions
    {
        CheckBoxOptions =
        {
            new TeCheckBoxOption<string>
            {
                Id = nameof(StringTestData.String),
                Converter = new TeCheckBoxConverter<string>
                {
                    FromBoolean = (b) => b.ToString(),
                    ToBoolean = (s) => bool.TryParse(s, out var boolean) ? boolean : default,
                },
            },
            new TeCheckBoxOption<int>
            {
                Id = nameof(StringTestData.Number),
                Converter = new TeCheckBoxConverter<int>
                {
                    FromBoolean = b => b ? 1 : 0,
                    ToBoolean = (i) => i == 0 ? false : true,
                },
            },
            new TeCheckBoxOption<DateTime>
            {
                Id = nameof(StringTestData.DateTime),
                Converter = new TeCheckBoxConverter<DateTime>
                {
                    FromBoolean = b => b ? DateTime.MaxValue : DateTime.MinValue,
                    ToBoolean = (d) => d == DateTime.MinValue ? false : true,
                },
            },
            new TeCheckBoxOption
            {
                Id = nameof(StringTestData.Bool),
            },
            new TeCheckBoxOption<TestEnum>
            {
                Id = nameof(StringTestData.Enum),
                Converter = new TeCheckBoxConverter<TestEnum>
                {
                    FromBoolean = (b) => b ? TestEnum.B : TestEnum.C,
                    ToBoolean = (e) => e == TestEnum.B ? true : false,
                },
            },
            new TeCheckBoxOption<List<int>>
            {
                Id = nameof(StringTestData.IntList),
                Converter = new TeCheckBoxConverter<List<int>>
                {
                    FromBoolean = b => b ? new List<int> { 1, 2, 3, 4, 5 } : new List<int>(),
                    ToBoolean = list => list.Any(),
                },
            },
        },
    };

    private async Task OnDataChanged<T>(T data)
    {
        await Js.InvokeVoidAsync("console.log", data);
    }
}
