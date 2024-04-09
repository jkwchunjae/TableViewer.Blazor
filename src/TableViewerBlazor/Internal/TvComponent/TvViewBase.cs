using Microsoft.AspNetCore.Components;
using TableViewerBlazor.Options;

namespace TableViewerBlazor.Internal.TvComponent;

public class TvViewBase : ComponentBase
{
    [Parameter] public TvOptions Options { get; set; } = new TvOptions();
    [Parameter] public int Depth { get; set; }
}
