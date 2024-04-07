using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TableViewerBlazor.Internal;
using TableViewerBlazor.Options;

namespace TableViewerBlazor;

public partial class TableViewer : TvViewBase
{
    [Inject] public IJSRuntime Js { get; set; } = null!;
    [Parameter] public object? Data { get; set; }
}
