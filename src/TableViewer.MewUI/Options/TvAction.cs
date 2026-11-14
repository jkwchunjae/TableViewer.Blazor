namespace TableViewer.MewUI.Options;

public interface ITvAction : ITvButton, global::TableViewer.Share.Options.ITvAction<TvButtonStyle, TvTooltipOption>
{
}

public class TvAction<T> : global::TableViewer.Share.Options.TvAction<T, TvButtonStyle, TvTooltipOption>, ITvAction
{
}
