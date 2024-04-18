namespace TableViewerBlazor.Options;

public class TvOptions
{
    public int GlobalOpenDepth { get; set; } = 1;
    public int ArrayVisibleDepth { get; set; } = 1;
    public bool ReadProperty { get; set; } = true;
    public bool ReadField { get; set; } = true;
    public IEnumerable<ITvAction>? Actions { get; set; }
    public IEnumerable<TvColumnVisibleOption>? ColumnVisible { get; set; }
    public IEnumerable<string>? DisableKeys { get; set; }
    public TvStyleOption Style { get; set; } = new();
    public IEnumerable<ITvEditorOption>? Editor { get; set; }
}

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
        (data, depth, path) => data is T t && Condition != null ? Condition(t, depth, path) : true;

    //public ITvOpenDepthOption.OpenDepthOptionConditionByType ConditionByType =>
    //    (type, depth, path) => typeof(T) == type && Condition != null ? true : false;
}
