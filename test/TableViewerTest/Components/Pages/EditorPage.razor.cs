using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TableViewerBlazor.Options;

namespace TableViewerTest.Components.Pages;

public class InnerClass
{
    public string InnerName { get; set; } = string.Empty;
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
        TextFieldOptions = new ITeTextFieldOption[]
        {
            new TeTextFieldOption<string>
            {
                Id = "name",
                Validations = new ITeValidation[]
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
                            await Task.Delay(1000);
                            return value.Length <= 3;
                        },
                        Message = "이름은 10자 이하로 입력해주세요.",
                    },
                },
            },
        },
        NumericFieldOptions = new ITeNumericFieldOption[]
        {
            new TeNumericFieldOption<int>
            {
                Id = "age",
                Validations = new ITeValidation[]
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
                    new TeAsyncValidation<int>
                    {
                        Func = async value =>
                        {
                            await Task.Delay(1000);
                            value = value / (value - 29);
                            return value != 20;
                        },
                        Message = "사실 20세는 선택할 수 없어요.",
                    },
                },
            },
        },
        SelectBoxOptions = new ITeSelectBoxOption[]
        {
            new TeSelectBoxOption<string>
            {
                Id = "gender",
                Items = new ITeSelectItem[]
                {
                    new TeSelectItem<string>("M", "남자"),
                    new TeSelectItem<string>("F", "여자"),
                },
            },
            new TeSelectBoxOption<int>
            {
                Id = "age",
                Items = new ITeSelectItem[]
                {
                    new TeSelectItem<int>(10, "10대"),
                    new TeSelectItem<int>(20, "20대"),
                    new TeSelectItem<int>(30, "30대"),
                    new TeSelectItem<int>(40, "40대"),
                    new TeSelectItem<int>(50, "50대"),
                },
            },
        },
        RadioOptions = new ITeRadioOption[]
        {
            new TeRadioOption<string>
            {
                Id = "ostype",
                Items = new ITeRadioItem[]
                {
                    new TeRadioItem<string>("linux", "리눅스"),
                    new TeRadioItem<string>("windows", "윈도우"),
                },
            },
        },
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
