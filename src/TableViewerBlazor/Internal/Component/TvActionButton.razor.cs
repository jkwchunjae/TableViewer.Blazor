using TableViewerBlazor.Internal.TvComponent;

namespace TableViewerBlazor.Internal.Component;

public partial class TvActionButton : TvViewBase
{
    [Parameter] public object? Item { get; set; }
    [Parameter] public ITvAction Action { get; set; } = null!;
}
