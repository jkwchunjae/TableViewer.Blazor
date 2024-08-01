using Microsoft.AspNetCore.Components;
using TableViewerBlazor.Options;

namespace TableViewerTest.Components.Pages;

public partial class EditorArray : ComponentBase
{
    private readonly string[] arrayData = ["1", "2", "배그", "bitter"];
    //private readonly List<object> arrayData = ["1", 2, "배그", "bitter", false];
    //private readonly List<object> arrayData = ["1", 2, "배그", "bitter", false, new TeOptions()];
    private readonly TeOptions teOptions = new()
    {
        Title = "array test",
    };
}
