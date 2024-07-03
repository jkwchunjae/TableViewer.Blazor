using moment.net;

namespace TableViewerBlazor.Service;

internal class DateTimeService
{
    public TvDateTimeGlobalOption? GlobalOption { get; set; }

    public static string ConvertDateTimeFormat(DateTime dateTime, TvDateTimeOption option)
    {
        dateTime = dateTime.ToLocalTime();

        if (option.RelativeTime)
        {
            return ConvertToRelativeTime(dateTime);
        }
        else
        {
            return dateTime.ToString(option.Format);
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
