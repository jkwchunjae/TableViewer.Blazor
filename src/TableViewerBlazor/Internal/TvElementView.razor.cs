using Microsoft.AspNetCore.Components;

namespace TableViewerBlazor.Internal;

public partial class TvElementView : TvViewBase
{
    [Parameter] public object? Data { get; set; }

    private bool IsNumber => Data switch
    {
        byte => true,
        int => true,
        long => true,
        float => true,
        double => true,
        decimal => true,
        _ => false,
    };
}
