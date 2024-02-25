namespace TableViewerBlazor.Options;

public class TvDateTimeOptions
{
    public string Calendar { get; set; } = "gregorian";
    public string Year { get; set; } = "numeric";
    public string Month { get; set; } = "numeric";
    public string Day { get; set; } = "numeric";
    public string Locale { get; set; } = "en";
    public string NumberingSystem { get; set; } = "latn";
    public string Timezone { get; set; } = "auto";
    public int Offset { get; set; } = 0;
    public string Format { get; set; } = "yyyy-MM-dd HH:mm:ss";
    public string FormatUtc { get; set; } = "yyyy-MM-dd HH:mm:ss";
    public string FormatLocal { get; set; } = "yyyy-MM-dd HH:mm:ss";
}
