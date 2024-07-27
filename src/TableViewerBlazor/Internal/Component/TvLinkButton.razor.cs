using TableViewerBlazor.Internal.TvComponent;

namespace TableViewerBlazor.Internal.Component;

public partial class TvLinkButton : TvViewBase
{
    [Parameter] public object? Item { get; set; }
    [Parameter] public ITvLink Link { get; set; } = null!;
}
