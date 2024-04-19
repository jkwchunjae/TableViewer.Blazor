namespace TableViewerBlazor.Internal.TvComponent;

public class TvViewBase : ComponentBase
{
    [Parameter] public TvOptions Options { get; set; } = new TvOptions();
    [Parameter] public int Depth { get; set; }
    [Parameter] public int OpenDepth { get; set; }
}
