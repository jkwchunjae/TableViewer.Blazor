using Microsoft.AspNetCore.Components;

namespace TableViewerBlazor.Internal;

public partial class TvElementView : TvViewBase
{
    [Parameter] public object? Data { get; set; }

    private bool IsNull => Data == null;
}
