namespace TableViewerBlazor.Internal.TvComponent;

public partial class TvStringLinkView : TvViewBase
{
    [Parameter] public required object Data { get; set; }
    [Parameter] public required object Parent { get; set; }
    [Parameter] public required ITvStringLinkOption StringLinkOption { get; set; }
}
