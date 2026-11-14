namespace TableViewer.MewUI.Options;

public interface ITvOneClickAction : ITvAction, global::TableViewer.Share.Options.ITvOneClickAction<ITvOneClickProgressStyle>
{
}

public interface ITvOneClickProgressStyle : global::TableViewer.Share.Options.ITvOneClickProgressStyle<Color, Size>
{
}

public class TvOneClickAction<T> : TvAction<T>, ITvOneClickAction
{
    private readonly global::TableViewer.Share.Options.TvOneClickOptions<ITvOneClickProgressStyle> _options = new(new TvOneClickProgressStyle());
    public bool Reusable { get => _options.Reusable; set => _options.Reusable = value; }
    public ITvOneClickProgressStyle ProgressStyle { get => _options.ProgressStyle; set => _options.ProgressStyle = value; }
}

public class TvOneClickProgressStyle() : global::TableViewer.Share.Options.TvOneClickProgressStyle<Color, Size>(Color.Default, Size.Small), ITvOneClickProgressStyle
{
}
