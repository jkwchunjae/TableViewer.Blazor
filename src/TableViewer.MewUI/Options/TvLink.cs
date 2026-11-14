namespace TableViewer.MewUI.Options;

public interface ITvLink : ITvButton, global::TableViewer.Share.Options.ITvLink<TvButtonStyle, TvTooltipOption>
{
}

public class TvLink<T> : global::TableViewer.Share.Options.TvLink<T, TvButtonStyle, TvTooltipOption>, ITvLink
{
}
