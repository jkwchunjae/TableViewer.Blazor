using System.Globalization;
using TableViewer.MewUI.Options;

namespace TableViewer.MewUI.Service;

/// <summary>Replaces browser Intl timezone discovery with the host's .NET culture and timezone.</summary>
public sealed class DateTimeService
{
    public static DateTimeService Default { get; } = new();
    public TvDateTimeGlobalOption? Options { get; set; }
    public Func<DateTimeOffset> Clock { get; set; } = () => DateTimeOffset.Now;

    public string Format(DateTime value, TvDateTimeOption? option = null) => Format(new DateTimeOffset(value), option);
    public string Format(DateTimeOffset value, TvDateTimeOption? option = null)
    {
        if (option?.RelativeTime == true) return Relative(value, Clock());
        var culture = Options is { Locale.Length: > 0 } global && global.Locale != "auto"
            ? CultureInfo.GetCultureInfo(global.Locale) : CultureInfo.CurrentCulture;
        DateTimeOffset local;
        if (Options is { Timezone: not ("auto" or "") } configured)
            local = TimeZoneInfo.ConvertTime(value, TimeZoneInfo.FindSystemTimeZoneById(configured.Timezone));
        else if (Options is { } fixedOffset)
            local = value.ToOffset(TimeSpan.FromMinutes(-fixedOffset.Offset));
        else local = TimeZoneInfo.ConvertTime(value, TimeZoneInfo.Local);
        return option is null ? local.DateTime.ToString(culture) : local.ToString(option.Format, culture);
    }

    public static string Relative(DateTimeOffset value, DateTimeOffset now)
    {
        var delta = value - now;
        var seconds = Math.Abs(delta.TotalSeconds);
        string text = seconds switch
        {
            < 45 => "a few seconds",
            < 90 => "a minute",
            < 45 * 60 => $"{Math.Round(seconds / 60)} minutes",
            < 90 * 60 => "an hour",
            < 22 * 3600 => $"{Math.Round(seconds / 3600)} hours",
            < 36 * 3600 => "a day",
            < 26 * 86400 => $"{Math.Round(seconds / 86400)} days",
            < 45 * 86400 => "a month",
            < 320 * 86400 => $"{Math.Round(seconds / (30 * 86400))} months",
            < 548 * 86400 => "a year",
            _ => $"{Math.Round(seconds / (365 * 86400))} years"
        };
        return delta > TimeSpan.Zero ? $"in {text}" : $"{text} ago";
    }
}
