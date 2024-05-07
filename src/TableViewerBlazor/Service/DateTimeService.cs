using System.Globalization;
using TableViewerBlazor.Options;

namespace TableViewerBlazor.Service;

internal class DateTimeService
{
    public TvDateTimeOptions? Options { get; set; }
    public string? Language { get; set; } = "en-US";
    public CultureInfo CultureInfo => CultureInfo.GetCultureInfo(Language ?? "en-US");
}
