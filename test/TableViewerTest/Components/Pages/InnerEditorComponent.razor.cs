using Microsoft.AspNetCore.Components;
using TableViewerBlazor.Internal.Component;

namespace TableViewerTest.Components.Pages;

public static class Rnd
{
    static Random rnd = new Random((int)DateTime.Now.Ticks);

    public static string Next() => rnd?.Next(1, 100).ToString() ?? "test";
}

public partial class InnerEditorComponent : ComponentBase
{
    [Parameter] public ICustomEditorArgument Argument { get; set; } = default!;
    string Value = string.Empty;

    protected override void OnInitialized()
    {
        if (Argument.Value is EditInner inner)
        {
            Value = inner.Birth;
        }
        else
        {
            Value = string.Empty;
        }
    }

    private async Task OnValueChanged(string v)
    {
        Value = v;
        if (Argument.DataChanged != null)
        {
            await Argument.DataChanged(new EditInner
            {
                Age = Argument.Value is EditInner inner ? inner.Age : 0,
                Birth = v,
            });
        }
    }
}
