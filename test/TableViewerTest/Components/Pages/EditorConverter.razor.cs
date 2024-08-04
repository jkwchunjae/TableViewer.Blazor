using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TableViewerBlazor.Options;

namespace TableViewerTest.Components.Pages;

public class StringTestData
{
    [TeNumericField(nameof(String))]
    public string String { get; set; } = string.Empty;
    [TeNumericField(nameof(Number))]
    public int Number { get; set; }
    [TeNumericField(nameof(DateTime))]
    public DateTime DateTime { get; set; }
    [TeNumericField(nameof(Bool))]
    public bool Bool { get; set; } = true;
    [TeNumericField(nameof(Enum))]
    public TestEnum Enum { get; set; }
    [TeNumericField(nameof(IntList))]
    public List<int> IntList { get; set; } = new List<int> { 1, 2, 3, 4, 5 };
}

[Route("/editor-converter")]
public partial class EditorConverter : ComponentBase
{
    [Inject] public IJSRuntime Js { get; set; } = default!;
    StringTestData data = new StringTestData();
    TeOptions options = new TeOptions
    {
        NumericFieldOptions =
        {
            new TeNumericFieldOption<string, int>
            {
                Id = nameof(StringTestData.String),
                // Items = new List<TeSelectItem<string>>
                // {
                //     new TeSelectItem<string> { Value = "A", Text = "A" },
                //     new TeSelectItem<string> { Value = "B", Text = "B" },
                //     new TeSelectItem<string> { Value = "C", Text = "C" },
                // },
                Converter = new TeNumericFieldConverter<string, int>
                {
                    FromNumber = (b) => b.ToString(),
                    ToNumber = (s) => int.TryParse(s, out var number) ? number : default,
                },
            },
            new TeNumericFieldOption<int>
            {
                Id = nameof(StringTestData.Number),
                // Items = new List<TeSelectItem<int>>
                // {
                //     new TeSelectItem<int> { Value = 1, Text = "1" },
                //     new TeSelectItem<int> { Value = 2, Text = "2" },
                //     new TeSelectItem<int> { Value = 3, Text = "3" },
                // },
                // Converter = new TeNumericFieldConverter<int>
                // {
                //     FromNumber = b => b ? 1 : 0,
                //     ToNumber = (i) => i == 0 ? false : true,
                // },
            },
            new TeNumericFieldOption<DateTime, double>
            {
                Id = nameof(StringTestData.DateTime),
                // Items = new List<TeSelectItem<DateTime>>
                // {
                //     new TeSelectItem<DateTime> { Value = DateTime.MinValue, Text = "Min" },
                //     new TeSelectItem<DateTime> { Value = DateTime.MaxValue, Text = "Max" },
                //     new TeSelectItem<DateTime> { Value = DateTime.Now, Text = "Now" },
                // },
                Converter = new TeNumericFieldConverter<DateTime, double>
                {
                    FromNumber = b => DateTime.FromOADate(b),
                    ToNumber = (d) => d.ToOADate(),
                },
            },
            new TeNumericFieldOption<bool, long>
            {
                Id = nameof(StringTestData.Bool),
                // Items = new List<TeSelectItem<bool>>
                // {
                //     new TeSelectItem<bool> { Value = true, Text = "True" },
                //     new TeSelectItem<bool> { Value = false, Text = "False" },
                // },
                Converter = new TeNumericFieldConverter<bool, long>
                {
                    FromNumber = (l) => l == 0 ? false : true,
                    ToNumber = b => b ? 1 : 0,
                },
            },
            new TeNumericFieldOption<TestEnum, byte>
            {
                Id = nameof(StringTestData.Enum),
                // Items = new List<TeSelectItem<TestEnum>>
                // {
                //     new TeSelectItem<TestEnum> { Value = TestEnum.A, Text = "A" },
                //     new TeSelectItem<TestEnum> { Value = TestEnum.B, Text = "B" },
                //     new TeSelectItem<TestEnum> { Value = TestEnum.C, Text = "C" },
                // },
                Converter = new TeNumericFieldConverter<TestEnum, byte>
                {
                    FromNumber = (b) => (TestEnum)b,
                    ToNumber = (e) => (byte)e,
                },
            },
            new TeNumericFieldOption<List<int>, ulong>
            {
                Id = nameof(StringTestData.IntList),
                // Items = new List<TeSelectItem<List<int>>>
                // {
                //     new TeSelectItem<List<int>> { Value = new List<int> { 1, 2, 3, 4, 5 }, Text = "1, 2, 3, 4, 5" },
                //     new TeSelectItem<List<int>> { Value = new List<int> { 6, 7, 8, 9, 10 }, Text = "6, 7, 8, 9, 10" },
                // },
                Converter = new TeNumericFieldConverter<List<int>, ulong>
                {
                    FromNumber = number => number.ToString().Select(x => int.Parse($"{x}")).ToList(),
                    ToNumber = list => ulong.Parse(string.Join("", list)),
                },
            },
        },
    };

    private async Task OnDataChanged<T>(T data)
    {
        await Js.InvokeVoidAsync("console.log", data);
    }
}
