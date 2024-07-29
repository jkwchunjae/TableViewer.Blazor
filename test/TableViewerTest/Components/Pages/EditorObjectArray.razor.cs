
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TableViewerBlazor.Options;

namespace TableViewerTest.Components.Pages;

class EditItem
{
    [TeSelectBox("IsSelected")]
    public bool IsSelected { get; set; }
    public PersonName? Name { get; set; }
    public string Address { get; set; } = string.Empty;
    public EditInner Inner { get; set; } = new EditInner();
}

class EditInner
{
    public string Birth { get; set; } = string.Empty;
    public int Age { get; set; }
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
            SelectBoxOptions =
            {
                new TeSelectBoxOption<bool>
                {
                    Id = "IsSelected",
                    Items =
                    {
                        new TeSelectItem<bool>(true, "true"),
                        new TeSelectItem<bool>(false, "false"),
                    },
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