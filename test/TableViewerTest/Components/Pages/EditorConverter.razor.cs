using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TableViewerBlazor.Options;

namespace TableViewerTest.Components.Pages;

public class StringTestData
{
    [TeRadio(nameof(String))]
    public string String { get; set; } = string.Empty;
    [TeRadio(nameof(Number))]
    public int Number { get; set; }
    [TeRadio(nameof(DateTime))]
    public DateTime DateTime { get; set; }
    [TeRadio(nameof(Bool))]
    public bool Bool { get; set; } = true;
    [TeRadio(nameof(Enum))]
    public TestEnum Enum { get; set; }
    [TeRadio(nameof(IntList))]
    public List<int> IntList { get; set; } = new List<int> { 1, 2, 3, 4, 5 };
}

[Route("/editor-converter")]
public partial class EditorConverter : ComponentBase
{
    [Inject] public IJSRuntime Js { get; set; } = default!;
    StringTestData data = new StringTestData();
    TeOptions options = new TeOptions
    {
        RadioOptions =
        {
            new TeRadioOption<string>
            {
                Id = nameof(StringTestData.String),
                Items = new List<TeRadioItem<string>>
                {
                    new TeRadioItem<string> { Value = "A", Text = "A" },
                    new TeRadioItem<string> { Value = "B", Text = "B" },
                    new TeRadioItem<string> { Value = "C", Text = "C" },
                },
                // Converter = new TeNumericFieldConverter<string, int>
                // {
                //     FromNumber = (b) => b.ToString(),
                //     ToNumber = (s) => int.TryParse(s, out var number) ? number : default,
                // },
            },
            new TeRadioOption<int>
            {
                Id = nameof(StringTestData.Number),
                Items = new List<TeRadioItem<int>>
                {
                    new TeRadioItem<int> { Value = 1, Text = "1" },
                    new TeRadioItem<int> { Value = 2, Text = "2" },
                    new TeRadioItem<int> { Value = 3, Text = "3" },
                },
                // Converter = new TeNumericFieldConverter<int>
                // {
                //     FromNumber = b => b ? 1 : 0,
                //     ToNumber = (i) => i == 0 ? false : true,
                // },
            },
            new TeRadioOption<DateTime>
            {
                Id = nameof(StringTestData.DateTime),
                Items = new List<TeRadioItem<DateTime>>
                {
                    new TeRadioItem<DateTime> { Value = DateTime.MinValue, Text = "Min" },
                    new TeRadioItem<DateTime> { Value = DateTime.MaxValue, Text = "Max" },
                    new TeRadioItem<DateTime> { Value = DateTime.Now, Text = "Now" },
                },
                // Converter = new TeNumericFieldConverter<DateTime, double>
                // {
                //     FromNumber = b => DateTime.FromOADate(b),
                //     ToNumber = (d) => d.ToOADate(),
                // },
            },
            new TeRadioOption<bool>
            {
                Id = nameof(StringTestData.Bool),
                Items = new List<TeRadioItem<bool>>
                {
                    new TeRadioItem<bool> { Value = true, Text = "True" },
                    new TeRadioItem<bool> { Value = false, Text = "False" },
                },
                // Converter = new TeNumericFieldConverter<bool, long>
                // {
                //     FromNumber = (l) => l == 0 ? false : true,
                //     ToNumber = b => b ? 1 : 0,
                // },
            },
            new TeRadioOption<TestEnum>
            {
                Id = nameof(StringTestData.Enum),
                Items = new List<TeRadioItem<TestEnum>>
                {
                    new TeRadioItem<TestEnum> { Value = TestEnum.A, Text = "A" },
                    new TeRadioItem<TestEnum> { Value = TestEnum.B, Text = "B" },
                    new TeRadioItem<TestEnum> { Value = TestEnum.C, Text = "C" },
                },
                // Converter = new TeNumericFieldConverter<TestEnum, byte>
                // {
                //     FromNumber = (b) => (TestEnum)b,
                //     ToNumber = (e) => (byte)e,
                // },
            },
            new TeRadioOption<List<int>>
            {
                Id = nameof(StringTestData.IntList),
                Items = new List<TeRadioItem<List<int>>>
                {
                    new TeRadioItem<List<int>> { Value = new List<int> { 1, 2, 3, 4, 5 }, Text = "1, 2, 3, 4, 5" },
                    new TeRadioItem<List<int>> { Value = new List<int> { 6, 7, 8, 9, 10 }, Text = "6, 7, 8, 9, 10" },
                },
                // Converter = new TeNumericFieldConverter<List<int>, ulong>
                // {
                //     FromNumber = number => number.ToString().Select(x => int.Parse($"{x}")).ToList(),
                //     ToNumber = list => ulong.Parse(string.Join("", list)),
                // },
            },
        },
    };

    private async Task OnDataChanged<T>(T data)
    {
        await Js.InvokeVoidAsync("console.log", data);
    }
}
