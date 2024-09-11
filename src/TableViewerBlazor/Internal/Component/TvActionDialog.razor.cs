namespace TableViewerBlazor.Internal.Component;

public partial class TvActionDialog : ComponentBase
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = null!;

    [Parameter] public object Item { get; set; } = null!;
    [Parameter] public ITvPopupAction PopupAction { get; set; } = null!;

    private void OnClose() => MudDialog.Close();

    private async void OnConfirm()
    {
        await PopupAction.Action(Item);
        OnClose();
    }
}
