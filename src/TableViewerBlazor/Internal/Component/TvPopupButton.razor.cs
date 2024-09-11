using Microsoft.AspNetCore.Components.Web;

namespace TableViewerBlazor.Internal.Component;

public partial class TvPopupButton : MudButton
{
    [Inject] IDialogService Dialog { get; set; } = null!;
    [Parameter] public object? Item { get; set; }
    [Parameter] public ITvPopupAction PopupAction { get; set; } = null!;

    private async Task OnClickPopupButton(MouseEventArgs e)
    {
        var parameters = new DialogParameters<TvActionDialog>
        {
            { x => x.Item, Item },
            { x => x.PopupAction, PopupAction },
        };
        await Dialog.ShowAsync<TvActionDialog>("Dialog Title", parameters, PopupAction.PopupStyle);
    }
}
