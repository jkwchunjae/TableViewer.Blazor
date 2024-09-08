using Microsoft.AspNetCore.Components;
using TableViewerBlazor.Internal.Component;

namespace TableViewerTest.Components.Pages;

public partial class InnerEditorComponent2 : ComponentBase
{
    [Parameter] public ICustomEditorArgument BaseArgument { get; set; } = default!;
    private CustomEditorTypedArgument<EditItem, EditInner2>? Argument;
    string Value = string.Empty;

    protected override void OnInitialized()
    {
        Argument = BaseArgument.Convert<EditItem, EditInner2>();
        Value = GetValue(Argument.Parent!);
        Argument.ParentChanged += Argument_ParentChanged;
    }

    private void Argument_ParentChanged(object? sender, EditItem parent)
    {
        Argument!.Parent = parent;
        Argument!.Value = parent.Inner2;

        Value = GetValue(parent!);
        StateHasChanged();
    }

    private string GetValue(EditItem parent)
    {
        if (parent.IsSelected)
        {
            return parent.Inner2.Level.ToString();
        }
        else
        {
            return parent.Inner2.Name;
        }
    }

    private async Task OnValueChanged(string v)
    {
        Value = v;
        if (Argument!.DataChanged != null)
        {
            if (Argument!.Parent?.IsSelected ?? false)
            {
                await Argument.DataChanged(new EditInner2
                {
                    Name = Argument.Value?.Name ?? string.Empty,
                    Level = int.TryParse(v, out var level) ? level : 0
                });
            }
            else
            {
                await Argument.DataChanged(new EditInner2
                {
                    Name = v,
                    Level = Argument.Value?.Level ?? 0
                });
            }
        }
    }
}
