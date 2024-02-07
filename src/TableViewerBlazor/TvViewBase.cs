using Microsoft.AspNetCore.Components;

namespace TableViewerBlazor;

public class TvViewBase : ComponentBase
{
    [Parameter] public TvOptions? Options { get; set; }
    [Parameter] public int Depth { get; set; }
}
