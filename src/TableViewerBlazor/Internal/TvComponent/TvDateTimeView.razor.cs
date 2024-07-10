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
        var globalOptions = DateTimeService.Options;
        if (globalOptions != null)
        {
            // NOTE: 타이밍 이슈로 Offset update 안됨
            targetDateTime = targetDateTime.AddHours(-(globalOptions.Offset / 60));
        }
        dateTime = ConvertDateTimeFormat(targetDateTime);
    }

    public string ConvertDateTimeFormat(DateTime dateTime)
    {
        var options = Options.DateTime;
        if (options != null)
        {
            if (options.RelativeTime)
            {
                return ConvertToRelativeTime(dateTime);
            }
            else
            {
                return dateTime.ToString(options.Format);
            }
        }
        else
        {
            return dateTime.ToString();
        }
    }

    private static string ConvertToRelativeTime(DateTime dateTime)
    {
        if (dateTime > DateTime.Now)
        {
            return dateTime.ToNow();
        }
        else
        {
            return dateTime.FromNow();
        }
    }
}
