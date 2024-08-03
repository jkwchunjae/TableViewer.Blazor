using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TableViewerBlazor.Options;

namespace TableViewerTest.Components.Pages;

public class StringTestData
{
    [TeTextField(nameof(String))]
    public string String { get; set; } = string.Empty;
    [TeTextField(nameof(Number))]
    public int Number { get; set; }
    [TeTextField(nameof(DateTime))]
    public DateTime DateTime { get; set; }
    [TeTextField(nameof(Bool))]
    public bool Bool { get; set; }
    [TeTextField(nameof(Enum))]
    public TestEnum Enum { get; set; }
    [TeTextField(nameof(IntList))]
    public List<int> IntList { get; set; } = new List<int> { 1, 2, 3, 4, 5 };
}

[Route("/editor-converter")]
public partial class EditorStringConverter : ComponentBase
{
    [Inject] public IJSRuntime Js { get; set; } = default!;
    StringTestData data = new StringTestData();
    TeOptions options = new TeOptions
    {
        TextFieldOptions =
        {
            new TeTextFieldOption
            {
                Id = nameof(StringTestData.String),
            },
            new TeTextFieldOption<int>
            {
                Id = nameof(StringTestData.Number),
                Converter = new TeTextFieldConverter<int>
                {
                    FromString = (s) => int.TryParse(s, out var number) ? number : default,
                    StringValue = (i) => i.ToString(),
                },
            },
            new TeTextFieldOption<DateTime>
            {
                Id = nameof(StringTestData.DateTime),
                Converter = new TeTextFieldConverter<DateTime>
                {
                    FromString = (s) => DateTime.TryParse(s, out var dateTime) ? dateTime : default,
                    StringValue = (d) => d.ToString(),
                },
            },
            new TeTextFieldOption<bool>
            {
                Id = nameof(StringTestData.Bool),
                Converter = new TeTextFieldConverter<bool>
                {
                    FromString = (s) => bool.TryParse(s, out var boolean) ? boolean : default,
                    StringValue = (b) => b.ToString(),
                },
            },
            new TeTextFieldOption<TestEnum>
            {
                Id = nameof(StringTestData.Enum),
                Converter = new TeTextFieldConverter<TestEnum>
                {
                    FromString = (s) => Enum.TryParse<TestEnum>(s, out var testEnum) ? testEnum : default,
                    StringValue = (e) => e.ToString(),
                },
            },
            new TeTextFieldOption<List<int>>
            {
                Id = nameof(StringTestData.IntList),
                Converter = new TeTextFieldConverter<List<int>>
                {
                    FromString = (s) => s.Split(',').Select(int.Parse).ToList(),
                    StringValue = (l) => string.Join(',', l),
                },
            },
        },
    };

    private async Task OnDataChanged<T>(T data)
    {
        await Js.InvokeVoidAsync("console.log", data);
    }
}
