using Microsoft.AspNetCore.Components;
using TableViewerBlazor.Service;

namespace TableViewerBlazor.Internal.TvComponent;

public partial class TvDateTimeView : TvViewBase
{
    [Inject] DateTimeService DateTimeService { get; set; } = null!;
    [Parameter] public DateTime Data { get; set; }

    private string dateTime = "";

    protected override void OnParametersSet()
    {
        var dateTime = Data.ToUniversalTime();
        var options = DateTimeService.Options;
        // NOTE: 타이밍 이슈로 항상 options == null인 상황
        if (options != null)
        {
            dateTime = dateTime.AddHours(-(options.Offset / 60));
        }
        SetOption(dateTime);
    }

    private void SetOption(DateTime dateTime)
    {
        var dateTimeOption = Options?.DateTime;
        if (dateTimeOption != null)
        {
            if (dateTimeOption.RelativeTime)
            {
                var relativeTimeInMin = (dateTime - System.DateTime.Now).TotalMinutes;
                this.dateTime = GetRelativeTimeInStr(relativeTimeInMin);
            }
            else
            {
                this.dateTime = dateTime.ToString(dateTimeOption.Format);
            }
        }
    }

    private string GetRelativeTimeInStr(double min)
    {
        var suffix = GetSuffix(min);
        if (min < 0)
        {
            min = -min;
        }
        (int time, string unit) result = GetTimeAndUnit(min);
        return $"{result.time} {result.unit} {suffix}";
    }

    private string GetSuffix(double min)
    {
        if (min > 0)
        {
            return "후";
        }
        else
        {
            return "전";
        }
    }

    private (int time, string unit) GetTimeAndUnit(double min)
    {
        int time;
        string unit;
        if (min < 60)
        {
            time = (int)min;
            unit = "분";
        }
        else if (min < 60 * 24)
        {
            var hourInMinute = 60;
            time = (int)(min / hourInMinute);
            unit = "시간";
        }
        else if (min < 60 * 24 * 365)
        {
            var dayInMinute = 60 * 24;
            time = (int)(min / dayInMinute);
            unit = "일";
        }
        else
        {
            var yearInMinute = 60 * 24 * 365;
            time = (int)(min / yearInMinute);
            unit = "년";
        }
        return (time, unit);
    }
}
