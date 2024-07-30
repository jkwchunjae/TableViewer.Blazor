using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using TableViewerBlazor.Options;
using TableViewerBlazor.Options.Property;

namespace TableViewerTest.Components.Pages;

public class InnerClass
{
    public PersonName InnerName { get; set; }
    public int Inner2 { get; set; }
}
public class EditData
{
    [TeTextField("name")]
    public string Name { get; set; } = string.Empty;
    [TeSelectBox("gender")]
    public string Gender { get; set; } = string.Empty;
    [TeNumericField("age")]
    public int Age { get; set; }
    [TeRadio("ostype")]
    public string OsType { get; set; } = "windows";
    public InnerClass Inner { get; set; } = new InnerClass();
    [TeRadio("enum")]
    public TestEnum EnumValue { get; set; }
    [TeCheckBox("region")]
    public string Region { get; set; } = string.Empty;
}

public partial class EditorPage : ComponentBase
{
    [Inject] private IJSRuntime Js { get; set; } = default!;

    EditData data = new EditData
    {
        Name = "John",
        Gender = "M",
        Age = 30,
        Region = "ko"
    };
    TeOptions options = new TeOptions
    {
        TextFieldOptions =
        {
            new TeTextFieldOption
            {
                Id = "name",
                Validations =
                {
                    new TeValidation<string>
                    {
                        Func = value => !string.IsNullOrEmpty(value),
                        Message = "이름을 입력해주세요.",
                    },
                    new TeAsyncValidation<string>
                    {
                        Func = async value =>
                        {
                            await Task.Delay(100);
                            return value.Length <= 5;
                        },
                        Message = "이름은 5자 이하로 입력해주세요.",
                    },
                },
                Property = new TeTextFieldProperty
                {
                    FullWidth = true,
                    Immediate = false,
                    DebounceInterval = 2000,
                    Underline = true,
                    HelperText = "이름을 입력해주세요.",
                    HelperTextOnFocus = true,
                    AdornmentIcon = Icons.Material.Filled.Person,
                    AdornmentText = "Name",
                    AutoFocus = true,
                },
            },
        },
        NumericFieldOptions =
        {
            new TeNumericFieldOption<int>
            {
                Id = "age",
                Validations =
                {
                    new TeValidation<int>
                    {
                        Func = value => value >= 0,
                        Message = "나이는 0세 이상으로 입력해주세요.",
                    },
                    new TeValidation<int>
                    {
                        Func = value => value <= 100,
                        Message = "나이는 100세 이하로 입력해주세요.",
                    },
                    new TeValidation<int>
                    {
                        Func = value =>
                        {
                            value = value / (value - 29);
                            return value != 20;
                        },
                        Message = "런타임 에러 테스트",
                    },
                    new TeAsyncValidation<int>
                    {
                        Func = async value =>
                        {
                            await Task.Delay(2000);
                            return value != 20;
                        },
                        Message = "사실 20세는 선택할 수 없어요.",
                    },
                },
                Property = new TeNumericFieldProperty<int>
                {
                    Step = 3,
                },
            },
        },
        SelectBoxOptions =
        {
            new TeSelectBoxOption<string>
            {
                Id = "gender",
                Items =
                {
                    new TeSelectItem<string>("M", "남자"),
                    new TeSelectItem<string>("F", "여자"),
                },
                Property = new TeSelectBoxProperty<string>
                {
                    Dense = true,
                    // OpenIcon = Icons.Material.Filled.Abc,
                    // AnchorOrigin = Origin.TopCenter,
                    HelperText = "성별을 선택해주세요.",
                    HelperTextOnFocus = true,
                },
            },
            new TeSelectBoxOption<int>
            {
                Id = "age",
                Items =
                {
                    new TeSelectItem<int>(10, "10대"),
                    new TeSelectItem<int>(20, "20대"),
                    new TeSelectItem<int>(30, "30대"),
                    new TeSelectItem<int>(40, "40대"),
                    new TeSelectItem<int>(50, "50대"),
                },
            },
        },
        RadioOptions =
        {
            new TeRadioOption<string>
            {
                Id = "ostype",
                Validations =
                {
                    new TeValidation<string>
                    {
                        Func = value => value == "linux",
                        Message = "리눅스를 선택해주세요.",
                    },
                },
                Items =
                {
                    new TeRadioItem<string>("linux", "리눅스")
                    {
                        Property = new TeRadioItemProperty
                        {
                            Color = Color.Secondary,
                        },
                    },
                    new TeRadioItem<string>("windows", "윈도우")
                    {
                        Property = new TeRadioItemProperty
                        {
                            Color = Color.Primary,
                        },
                    },
                },
            },
            new TeRadioOption<TestEnum>
            {
                Id = "enum",
                Items =
                {
                    new TeRadioItem<TestEnum>(TestEnum.A, "A값입니다.")
                    {
                        Property = new TeRadioItemProperty
                        {
                            Color = Color.Secondary,
                        },
                    },
                    new TeRadioItem<TestEnum>(TestEnum.B, "B급입니다")
                    {
                        Property = new TeRadioItemProperty
                        {
                            Color = Color.Primary,
                        },
                    },
                    new TeRadioItem<TestEnum>(TestEnum.C, "이건 좀 아닙니다")
                    {
                        Property = new TeRadioItemProperty
                        {
                            Color = Color.Error,
                        },
                    },
                },
            },
        },
        CheckBoxOptions =
        {
            new TeCheckBoxOption<string>
            {
                Id = "region",
                Property = new TeCheckBoxProperty
                {
                    Color = Color.Info,
                    TriState = false,
                    LabelOptions = new TeLabelOptions<string>
                    {
                        Condition = (_) => true,
                        Label = (value) => $"{value} label option added",
                        //LabelPosition = LabelPosition.Start
                    }
                }
            }
        },
        IgnoreOptions =
        {
            //new TeIgnoreOption
            //{
            //    Condition = (data, index, path) => data is string str && str == "John",
            //},
            //new TeIgnoreOption
            //{
            //    Condition = (data, index, path) => data is int,
            //},
        }
    };
    private async Task Changed(EditData data)
    {
        await Js.InvokeVoidAsync("console.log", data);
    }
    private async Task OnValidChanged(bool isValid)
    {
        await Js.InvokeVoidAsync("console.log", "OnValidChanged", isValid);
    }
}
