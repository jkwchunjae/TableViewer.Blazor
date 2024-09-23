namespace TableViewerBlazor.Internal.TvComponent;

public partial class TvTextLinkView : TvViewBase
{
    [Parameter] public object Parent { get; set; } = null!;
    [Parameter] public object Data { get; set; } = null!;
    [Parameter] public ITvTextLinkOption TextLinkOption { get; set; } = null!;
}
