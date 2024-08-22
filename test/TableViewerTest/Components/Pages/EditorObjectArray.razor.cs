
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TableViewerBlazor.Options;
using TableViewerBlazor.Options.Property;
using TableViewerBlazor.Public;

namespace TableViewerTest.Components.Pages;

class EditItem
{
    [TeSelectBox(nameof(IsSelected))]
    public bool IsSelected { get; set; }
    [TeTextField(nameof(Name))]
    public PersonName? Name { get; set; }
    public string Address { get; set; } = string.Empty;
    [TeCustomEditor(nameof(Inner))]
    public EditInner Inner { get; set; } = new EditInner();
}

class EditInner
{
    public string Birth { get; set; } = string.Empty;
    [TeTextField(nameof(Age))]
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
            ObjectListEditorOptions =
            {
                new TeObjectListEditorOption<EditItem>
                {
                },
            },
            TextFieldOptions =
            {
                new TeTextFieldOption<PersonName>
                {
                    Converter = new TeTextFieldConverter<PersonName>
                    {
                        FromString = (s) => new PersonName(s),
                        StringValue = (p) => p?.ToString() ?? string.Empty,
                    },
                    Property = new TeTextFieldProperty
                    {
                        Style = "width: 50px; font-weight: bold;",
                    },
                },
                new TeTextFieldOption<int>
                {
                    Id = nameof(EditInner.Age),
                    Converter = new TeTextFieldConverter<int>
                    {
                        FromString = (s) => int.TryParse(s, out var number) ? number : default,
                        StringValue = (i) => i.ToString(),
                    },
                },
            },
            SelectBoxOptions =
            {
                new TeSelectBoxOption<bool>
                {
                    Id = nameof(EditItem.IsSelected),
                    Items =
                    {
                        new TeSelectItem<bool>(true, "True", true),
                        new TeSelectItem<bool>(false, "False", false),
                    },
                }
            },
            CustomEditorOptions = new TeCustomEditorOptionGroup
            {
                CustomEditor1 = new TeCustomEditorOption
                {
                    Id = "Inner",
                    Condition = (memberInfo, type) => type == typeof(EditInner),
                },
            },
            ToolbarButtons =
            {
                new TvLink<object>
                {
                    Href = x => "https://www.google.com",
                    Label = "Google",
                    Condition = (x, i) => true,
                },
                new TvLink<object>
                {
                    Href = x => "https://www.google.com",
                    Label = "Google",
                    Condition = (x, i) => true,
                },
                new TvLink<object>
                {
                    Href = x => "https://www.google.com",
                    Label = "Google",
                    Condition = (x, i) => true,
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