using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TableViewerBlazor.Options;

namespace TableViewerTest.Components.Pages;

public class StringTestData
{
    [TeSelectBox(nameof(String))]
    public string String { get; set; } = string.Empty;
    [TeSelectBox(nameof(Number))]
    public int Number { get; set; }
    [TeSelectBox(nameof(DateTime))]
    public DateTime DateTime { get; set; }
    [TeSelectBox(nameof(Bool))]
    public bool Bool { get; set; } = true;
    [TeSelectBox(nameof(Enum))]
    public TestEnum Enum { get; set; }
    [TeSelectBox(nameof(IntList))]
    public List<int> IntList { get; set; } = new List<int> { 1, 2, 3, 4, 5 };
}

[Route("/editor-converter")]
public partial class EditorConverter : ComponentBase
{
    [Inject] public IJSRuntime Js { get; set; } = default!;
    StringTestData data = new StringTestData();
    TeOptions options = new TeOptions
    {
        SelectBoxOptions =
        {
            new TeSelectBoxOption<string>
            {
                Id = nameof(StringTestData.String),
                Items = new List<TeSelectItem<string>>
                {
                    new TeSelectItem<string> { Value = "A", Text = "A" },
                    new TeSelectItem<string> { Value = "B", Text = "B" },
                    new TeSelectItem<string> { Value = "C", Text = "C" },
                },
                // Converter = new TeCheckBoxConverter<string>
                // {
                //     FromBoolean = (b) => b.ToString(),
                //     ToBoolean = (s) => bool.TryParse(s, out var boolean) ? boolean : default,
                // },
            },
            new TeSelectBoxOption<int>
            {
                Id = nameof(StringTestData.Number),
                Items = new List<TeSelectItem<int>>
                {
                    new TeSelectItem<int> { Value = 1, Text = "1" },
                    new TeSelectItem<int> { Value = 2, Text = "2" },
                    new TeSelectItem<int> { Value = 3, Text = "3" },
                },
                // Converter = new TeCheckBoxConverter<int>
                // {
                //     FromBoolean = b => b ? 1 : 0,
                //     ToBoolean = (i) => i == 0 ? false : true,
                // },
            },
            new TeSelectBoxOption<DateTime>
            {
                Id = nameof(StringTestData.DateTime),
                Items = new List<TeSelectItem<DateTime>>
                {
                    new TeSelectItem<DateTime> { Value = DateTime.MinValue, Text = "Min" },
                    new TeSelectItem<DateTime> { Value = DateTime.MaxValue, Text = "Max" },
                    new TeSelectItem<DateTime> { Value = DateTime.Now, Text = "Now" },
                },
                // Converter = new TeCheckBoxConverter<DateTime>
                // {
                //     FromBoolean = b => b ? DateTime.MaxValue : DateTime.MinValue,
                //     ToBoolean = (d) => d == DateTime.MinValue ? false : true,
                // },
            },
            new TeSelectBoxOption<bool>
            {
                Id = nameof(StringTestData.Bool),
                Items = new List<TeSelectItem<bool>>
                {
                    new TeSelectItem<bool> { Value = true, Text = "True" },
                    new TeSelectItem<bool> { Value = false, Text = "False" },
                },
            },
            new TeSelectBoxOption<TestEnum>
            {
                Id = nameof(StringTestData.Enum),
                Items = new List<TeSelectItem<TestEnum>>
                {
                    new TeSelectItem<TestEnum> { Value = TestEnum.A, Text = "A" },
                    new TeSelectItem<TestEnum> { Value = TestEnum.B, Text = "B" },
                    new TeSelectItem<TestEnum> { Value = TestEnum.C, Text = "C" },
                },
                // Converter = new TeCheckBoxConverter<TestEnum>
                // {
                //     FromBoolean = (b) => b ? TestEnum.B : TestEnum.C,
                //     ToBoolean = (e) => e == TestEnum.B ? true : false,
                // },
            },
            new TeSelectBoxOption<List<int>>
            {
                Id = nameof(StringTestData.IntList),
                Items = new List<TeSelectItem<List<int>>>
                {
                    new TeSelectItem<List<int>> { Value = new List<int> { 1, 2, 3, 4, 5 }, Text = "1, 2, 3, 4, 5" },
                    new TeSelectItem<List<int>> { Value = new List<int> { 6, 7, 8, 9, 10 }, Text = "6, 7, 8, 9, 10" },
                },
                // Converter = new TeCheckBoxConverter<List<int>>
                // {
                //     FromBoolean = b => b ? new List<int> { 1, 2, 3, 4, 5 } : new List<int>(),
                //     ToBoolean = list => list.Any(),
                // },
            },
        },
    };

    private async Task OnDataChanged<T>(T data)
    {
        await Js.InvokeVoidAsync("console.log", data);
    }
}
