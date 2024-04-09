using Microsoft.AspNetCore.Components;
using MudBlazor;
using TableViewerBlazor.Options;

namespace TableViewerBlazor.Internal.Component;

public partial class TvDoubleClickButton : MudButton
{
    public enum ButtonState
    {
        First,
        Second,
    };

    [Parameter] public object? Item { get; set; }
    [Parameter] public ITvDoubleClickAction DoubleClickAction { get; set; } = null!;
    //[Parameter] public string Class { get; set; } = string.Empty;

    private ButtonState State = ButtonState.First;
    private CancellationTokenSource? cts;

    private Task ClickFirst()
    {
        State = ButtonState.Second;
        cts = new CancellationTokenSource();
        _ = Task.Run(async () =>
        {
            if (cts != null)
            {
                await Task.Delay(DoubleClickAction.ResetDelay, cts.Token);
            }
            if (cts?.IsCancellationRequested ?? false)
                return;
            await InvokeAsync(() =>
            {
                State = ButtonState.First;
                StateHasChanged();
            });
        });
        StateHasChanged();
        return Task.CompletedTask;
    }

    private async Task ClickSecond()
    {
        await DoubleClickAction.Action(Item);
        State = ButtonState.First;
        if (cts != null && !cts.IsCancellationRequested)
        {
            await cts.CancelAsync();
        }
        StateHasChanged();
    }
}
