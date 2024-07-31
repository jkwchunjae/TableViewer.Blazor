
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TableViewerBlazor.Options;

namespace TableViewerTest.Components.Pages;

class EditItem
{
    [TeCheckBox("IsSelected")]
    public bool IsSelected { get; set; }
    public PersonName? Name { get; set; }
    public string Address { get; set; } = string.Empty;
    [TeCustomEditor("Inner")]
    public EditInner Inner { get; set; } = new EditInner();
}

class EditInner
{
    public string Birth { get; set; } = string.Empty;
    public int Age { get; set; }
}

class GsmValue2
{
    public int IntValue { get; set; }
    public string StringValue { get; set; } = string.Empty;
}

public partial class EditorObjectArray : ComponentBase
{
    [Inject] public IJSRuntime Js { get; set; } = default!;

    TeOptions? options = default;
    List<EditItem> items = new List<EditItem>();
    protected override Task OnInitializedAsync()
    {
        items = new List<EditItem>
        {
            new EditItem
            {
                Name = new PersonName("jkw"),
                Address = "Seoul",
                Inner = new EditInner
                {
                    Birth = DateTime.Now.ToString(),
                    Age = 20,
                },
            },
        };
        options = new TeOptions
        {
            Title = "User table",
            TextFieldOptions =
            {
                new TeTextFieldOption<PersonName>
                {
                    Converter = new TeTextFieldConverter<PersonName>
                    {
                        FromString = (s) => new PersonName(s),
                        StringValue = (p) => p?.ToString() ?? string.Empty,
                    },
                },
            },
            CheckBoxOptions =
            {
                new TeCheckBoxOption<EditItem>
                {
                    Id = "IsSelected",
                },
            },
            CustomEditorOptions = new TeCustomEditorOptionGroup
            {
                CustomEditor1 = new TeCustomEditorOption
                {
                    Id = "Inner",
                    Condition = (memberInfo, type) => type == typeof(EditInner),
                },
            },
        };
        return base.OnInitializedAsync();
    }

    private async Task OnDataChanged(List<EditItem>? item)
    {
        if (item != null)
        {
            await Js.InvokeVoidAsync("console.log", item);
        }
    }
}