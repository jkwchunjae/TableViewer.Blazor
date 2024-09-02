namespace TableViewerBlazor.Options;

public interface ITvOpenDepthOption
{
    delegate bool OpenDepthOptionCondition(object? data, int depth, string path);
    int OpenDepth { get; }
    OpenDepthOptionCondition? Condition { get; }
}

public class TvOpenDepthOption<T> : ITvOpenDepthOption
{
    public int OpenDepth { get; set; }
    public Func<T?, int, string, bool>? Condition { get; set; }

    ITvOpenDepthOption.OpenDepthOptionCondition? ITvOpenDepthOption.Condition =>
        (data, depth, path) =>
        {
            if (data is T t)
            {
                return Condition?.Invoke(t, depth, path) ?? true;
            }
            else
            {
                return false;
            }
        };
}
