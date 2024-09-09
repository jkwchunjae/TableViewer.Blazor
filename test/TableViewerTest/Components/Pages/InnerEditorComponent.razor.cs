using Microsoft.AspNetCore.Components;
using TableViewerBlazor.Internal.Component;

namespace TableViewerTest.Components.Pages;

public partial class InnerEditorComponent : ComponentBase, IDisposable
{
    [Parameter] public CustomEditorArgument<EditItem, EditInner> Argument { get; set; } = default!;
    string Value = string.Empty;

    protected override void OnInitialized()
    {
        Value = GetValue(Argument.Parent!);
        Argument.ParentChanged += Argument_ParentChanged;
    }

    private void Argument_ParentChanged(object? sender, EditItem parent)
    {
        Argument!.Parent = parent;
        Argument!.Value = parent.Inner;

        Value = GetValue(parent!);
        StateHasChanged();
    }

    private string GetValue(EditItem parent)
    {
        if (parent.IsSelected)
        {
            return parent.Inner.Birth;
        }
        else
        {
            return parent.Inner.Age.ToString();
        }
    }

    private async Task OnValueChanged(string v)
    {
        Value = v;
        if (Argument!.DataChanged != null)
        {
            if (Argument!.Parent?.IsSelected ?? false)
            {
                await Argument.DataChanged(new EditInner
                {
                    Age = Argument.Value?.Age ?? 0,
                    Birth = v,
                });
            }
            else
            {
                await Argument.DataChanged(new EditInner
                {
                    Age = int.TryParse(v, out var age) ? age : 0,
                    Birth = Argument.Value?.Birth ?? string.Empty,
                });
            }
        }
    }

    public void Dispose()
    {
        Argument.ParentChanged -= Argument_ParentChanged;
    }
}

