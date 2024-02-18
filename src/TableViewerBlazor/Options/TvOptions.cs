namespace TableViewerBlazor.Options;

public class TvOptions
{
    public int OpenDepth { get; set; } = 1;
    public int ArrayVisibleDepth { get; set; } = 1;
    public IEnumerable<ITvAction>? Actions { get; set; }
}

public interface ITvAction
{
    public Func<object?, int, bool> Condition { get; }
    public Func<object?, Task> Action { get; }
    public string Label { get; }
    public string LabelAfterClick { get; }
    public string Class { get; }
}

public class TvAction<T> : ITvAction
{
    public Func<T , int, bool> Condition { get; set; } = (_, _) => true;
    public Func<T, Task> Action { get; set; } = _ => Task.CompletedTask;
    public string Label { get; set; } = "ACTION";
    public string LabelAfterClick { get; set; } = "DONE";
    public string Class { get; set; } = string.Empty;

    Func<object?, int, bool> ITvAction.Condition => (o, i) => o is T t && Condition(t, i);
    Func<object?, Task> ITvAction.Action => (o) => o is T t ? Action(t) : Task.CompletedTask;
}