using Microsoft.AspNetCore.Components;

namespace TableViewerBlazor.Internal;

public partial class TvDateTimeView : TvViewBase
{
    [Parameter] public DateTime Data { get; set; }
}
