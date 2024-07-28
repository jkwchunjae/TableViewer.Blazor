
using Microsoft.AspNetCore.Components;
using TableViewerBlazor.Options;

namespace TableViewerTest.Components.Pages;

class EditItem
{
    public PersonName? Name { get; set; }
    public int Age { get; set; }
    public DateTime Birth { get; set; }
}

public partial class EditorObjectArray : ComponentBase
{
    TeOptions? options = default;
    List<EditItem> items = new List<EditItem>();
    protected override Task OnInitializedAsync()
    {
        options = new TeOptions
        {
            
        };
        return base.OnInitializedAsync();
    }
}