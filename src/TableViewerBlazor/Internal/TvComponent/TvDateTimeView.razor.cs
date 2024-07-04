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
        var targetDateTime = Data.ToUniversalTime();
        var options = DateTimeService.Options;
        // NOTE: 타이밍 이슈로 항상 options == null인 상황
        if (options != null)
        {
            targetDateTime = targetDateTime.AddHours(-(options.Offset / 60));
        }
        dateTime = ConvertDateTime(targetDateTime);
    }

    private string ConvertDateTime(DateTime targetDateTime)
    {
        var dateTimeOption = Options?.DateTime;
        if (dateTimeOption != null)
        {
            if (dateTimeOption.RelativeTime)
            {
                if (targetDateTime > DateTime.Now)
                {
                    return targetDateTime.ToNow();
                }
                else
                {
                    return targetDateTime.FromNow();
                }
            }
            else
            {
                return targetDateTime.ToString(dateTimeOption.Format);
            }
        }
        else
        {
            return "";
        }
    }
}
