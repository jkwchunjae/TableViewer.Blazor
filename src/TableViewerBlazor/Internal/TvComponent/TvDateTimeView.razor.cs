using Microsoft.AspNetCore.Components;
using moment.net;

namespace TableViewerBlazor.Internal.TvComponent;

public partial class TvDateTimeView : TvViewBase
{
    [Inject] DateTimeService DateTimeService { get; set; } = null!;
    [Parameter] public DateTime Data { get; set; }

    private string dateTime = "";

    protected override void OnParametersSet()
    {
        if (Options?.DateTime != null)
        {
            dateTime = DateTimeService.ConvertDateTimeFormat(Data, Options.DateTime);
        }
    }
}
