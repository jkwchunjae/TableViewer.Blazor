using TableViewerBlazor.Internal.TvComponent;

namespace TableViewerBlazor.Internal.Component;

public partial class TvButton : TvViewBase
{
    [Parameter] public object? Item { get; set; }
    [Parameter] public ITvButton Button { get; set; } = null!;

}
