namespace TableViewerBlazor.Options;

public interface ITvImageViewerOption : global::TableViewer.Share.Options.ITvImageViewerOption<ObjectFit, ObjectPosition>
{
}

public class TvImageViewerOption<TParent, TValue> : global::TableViewer.Share.Options.TvImageViewerOption<TParent, TValue, ObjectFit, ObjectPosition>, ITvImageViewerOption
{
    public TvImageViewerOption() : base(ObjectFit.Fill, ObjectPosition.Center) { }
}
