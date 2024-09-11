namespace TableViewerBlazor.Internal.Component;

// TODO: generic으로 받아야하는지 고민
public partial class TvActionDialog : ComponentBase
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = null!;

    [Parameter] public object Item { get; set; } = null!;
    [Parameter] public ITvPopupAction PopupAction { get; set; } = null!;

    private void OnClose() => MudDialog.Close();

    private void OnClickButton()
    {
        PopupAction.Action(Item);
    }
}
