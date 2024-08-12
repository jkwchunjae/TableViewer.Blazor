using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using TableViewerBlazor.Options;
using TableViewerBlazor.Options.Property;

namespace TableViewerTest.Components.Pages;

public class StringTestData
{
    [TeCheckBox(nameof(String))]
    public string String { get; set; } = "QWERTY";
    //[TeListEditor(nameof(Number))]
    //public int Number { get; set; }
    //[TeListEditor(nameof(DateTime))]
    //public DateTime DateTime { get; set; }
    //[TeListEditor(nameof(Bool))]
    //public bool Bool { get; set; } = true;
    //[TeListEditor(nameof(Enum))]
    //public TestEnum Enum { get; set; }
    [TeListEditor(nameof(IntList))]
    public List<int> IntList { get; set; } = new List<int> { 1, 2, 3, 4, 5 };

    [TeSwitch(nameof(Switch))]
    public string Switch { get; set; } = "hello";
    [TeSwitch(nameof(Bool))]
    public bool Bool { get; set; } = false;
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
                //Items = new List<TeRadioItem<string>>
                //{
                //    new TeRadioItem<string> { Value = "A", Text = "A" },
                //    new TeRadioItem<string> { Value = "B", Text = "B" },
                //    new TeRadioItem<string> { Value = "C", Text = "C" },
                //},
                Converter = new TeBoolConverter<string>
                {
                    FromBoolean = b => b ? "트루" : "폴스", // = FromField
                    ToBoolean = str => str == "폴스" ? false : true, // = ToField
                },

            },
            //new TeRadioOption<int>
            //{
            //    Id = nameof(StringTestData.Number),
            //    Items = new List<TeRadioItem<int>>
            //    {
            //        new TeRadioItem<int> { Value = 1, Text = "1" },
            //        new TeRadioItem<int> { Value = 2, Text = "2" },
            //        new TeRadioItem<int> { Value = 3, Text = "3" },
            //    },
            //    // Converter = new TeNumericFieldConverter<int>
            //    // {
            //    //     FromNumber = b => b ? 1 : 0,
            //    //     ToNumber = (i) => i == 0 ? false : true,
            //    // },
            //},
            //new TeRadioOption<DateTime>
            //{
            //    Id = nameof(StringTestData.DateTime),
            //    Items = new List<TeRadioItem<DateTime>>
            //    {
            //        new TeRadioItem<DateTime> { Value = DateTime.MinValue, Text = "Min" },
            //        new TeRadioItem<DateTime> { Value = DateTime.MaxValue, Text = "Max" },
            //        new TeRadioItem<DateTime> { Value = DateTime.Now, Text = "Now" },
            //    },
            //    // Converter = new TeNumericFieldConverter<DateTime, double>
            //    // {
            //    //     FromNumber = b => DateTime.FromOADate(b),
            //    //     ToNumber = (d) => d.ToOADate(),
            //    // },
            //},
            //new TeRadioOption<bool>
            //{
            //    Id = nameof(StringTestData.Bool),
            //    Items = new List<TeRadioItem<bool>>
            //    {
            //        new TeRadioItem<bool> { Value = true, Text = "True" },
            //        new TeRadioItem<bool> { Value = false, Text = "False" },
            //    },
            //    // Converter = new TeNumericFieldConverter<bool, long>
            //    // {
            //    //     FromNumber = (l) => l == 0 ? false : true,
            //    //     ToNumber = b => b ? 1 : 0,
            //    // },
            //},
            //new TeRadioOption<TestEnum>
            //{
            //    Id = nameof(StringTestData.Enum),
            //    Items = new List<TeRadioItem<TestEnum>>
            //    {
            //        new TeRadioItem<TestEnum> { Value = TestEnum.A, Text = "A" },
            //        new TeRadioItem<TestEnum> { Value = TestEnum.B, Text = "B" },
            //        new TeRadioItem<TestEnum> { Value = TestEnum.C, Text = "C" },
            //    },
            //    // Converter = new TeNumericFieldConverter<TestEnum, byte>
            //    // {
            //    //     FromNumber = (b) => (TestEnum)b,
            //    //     ToNumber = (e) => (byte)e,
            //    // },
            //},
            //new TeListEditorOption<int>
            //{
            //    Id = nameof(StringTestData.IntList),
            //    //Items = new List<TeRadioItem<List<int>>>
            //    //{
            //    //    new TeRadioItem<List<int>> { Value = new List<int> { 1, 2, 3, 4, 5 }, Text = "1, 2, 3, 4, 5" },
            //    //    new TeRadioItem<List<int>> { Value = new List<int> { 6, 7, 8, 9, 10 }, Text = "6, 7, 8, 9, 10" },
            //    //},
            //    // Converter = new TeNumericFieldConverter<List<int>, ulong>
            //    // {
            //    //     FromNumber = number => number.ToString().Select(x => int.Parse($"{x}")).ToList(),
            //    //     ToNumber = list => ulong.Parse(string.Join("", list)),
            //    // },
            //},
        },
        SwitchOptions =
        {
            new TeSwitchOption<string>
            {
                Id = nameof(StringTestData.Switch),
                Converter = new TeBoolConverter<string>
                {
                    FromBoolean = b => b ? "트루" : "폴폴스",
                    ToBoolean = str => str != "폴폴스",
                }
            },
            new TeSwitchOption
            {
                Id = nameof(StringTestData.Bool),
                Property = new TeSwitchProperty
                {
                    ShowLabel = false,
                    ThumbIcon = Icons.Material.Outlined.Check,
                    ThumbIconUnChecked = Icons.Custom.Uncategorized.ChessKing,
                    ThumbIconColor = MudBlazor.Color.Warning,
                    UncheckedColor = MudBlazor.Color.Error,
                }
            }
        }
    };

    private async Task OnDataChanged<T>(T data)
    {
        var a = new TeListEditorConverter<string, List<int>>
        {
            ToList = (s) => s.Select(x => int.Parse($"{x}")).ToList(),
            FromList = (list) => string.Join("", list),
        };

        await Js.InvokeVoidAsync("console.log", data);
    }
}
