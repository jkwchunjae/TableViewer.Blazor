namespace TableViewerBlazor.Options;

public interface ITvOpenDepthOption
{
    delegate bool OpenDepthOptionCondition(object? data, int depth, string path);
    //delegate bool OpenDepthOptionConditionByType(Type type, int depth, string path);
    int OpenDepth { get; }
    OpenDepthOptionCondition? Condition { get; }
    //OpenDepthOptionConditionByType ConditionByType { get; }
}

public class TvOpenDepthOption<T> : ITvOpenDepthOption
{
    //public delegate bool OpenDepthOptionConditionT(T? data, int depth, string path);
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

    //public ITvOpenDepthOption.OpenDepthOptionConditionByType ConditionByType =>
    //    (type, depth, path) => typeof(T) == type && Condition != null ? true : false;
}
