namespace TableViewerBlazor.Options;

internal class TvDateTimeGlobalOption
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
}