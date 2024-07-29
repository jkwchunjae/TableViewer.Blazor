
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TableViewerBlazor.Options;

namespace TableViewerTest.Components.Pages;

class EditItem
{
    public PersonName? Name { get; set; }
    public int Age { get; set; }
    public DateTime Birth { get; set; }
    public string Address { get; set; } = string.Empty;
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
                Age = 30,
                Birth = new DateTime(1990, 1, 1),
                Address = "Seoul",
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
                        StringValue = (p) => p.ToString(),
                    },
                },
            },
        };
        return base.OnInitializedAsync();
    }

    private async Task OnDataChanged(List<EditItem>? item)
    {
        await Js.InvokeVoidAsync("console.log", item);
        await Js.InvokeVoidAsync("console.log", this.items);
    }
}