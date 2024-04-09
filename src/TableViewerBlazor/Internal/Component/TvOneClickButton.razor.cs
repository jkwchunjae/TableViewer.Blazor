using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using TableViewerBlazor.Options;

namespace TableViewerBlazor.Internal.Component;

public partial class TvOneClickButton : MudButton
{
    public enum ButtonState
    {
        Default,
        Progressing,
        Done,
    }
    [Parameter] public object? Item { get; set; }
    [Parameter] public ITvOneClickAction OneClickAction { get; set; } = null!;

    private ButtonState State = ButtonState.Default;
    private async Task Click(MouseEventArgs ev)
    {
        State = ButtonState.Progressing;
        StateHasChanged();

        await OneClickAction.Action(Item);

        if (OneClickAction.Reusable)
        {
            State = ButtonState.Default;
        }
        else
        {
            State = ButtonState.Done;
        }
        StateHasChanged();
    }
}
