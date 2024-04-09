using MudBlazor;

namespace TableViewerBlazor.Options;

public interface ITvOneClickAction : ITvAction
{
    public bool Reusable { get; }
    public ITvOneClickProgressStyle ProgressStyle { get; }
}

public interface ITvOneClickProgressStyle
{
    public Color Color { get; }
    public Size Size { get; }
}

public class TvOneClickAction<T> : TvAction<T>, ITvOneClickAction
{
    public bool Reusable { get; set; } = false;
    public ITvOneClickProgressStyle ProgressStyle { get; set; } = new TvOneClickProgressStyle();
}

public class TvOneClickProgressStyle : ITvOneClickProgressStyle
{
    public Color Color { get; set; } = Color.Default;
    public Size Size { get; set; } = Size.Small;
}