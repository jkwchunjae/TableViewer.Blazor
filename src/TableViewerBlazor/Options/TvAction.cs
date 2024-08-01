namespace TableViewerBlazor.Options;

public interface ITvAction : ITvButton
{
    public Func<object?, Task> Action { get; }
    public string LabelAfterClick { get; }
}

public class TvAction<T> : ITvAction
{
    public Func<T , int, bool> Condition { get; set; } = (_, _) => true;
    public required Func<T, Task> Action { get; set; }
    public string Label { get; set; } = "ACTION";
    public string LabelAfterClick { get; set; } = "DONE";
    public TvButtonStyle Style { get; set; } = new();

    Func<object?, int, bool> ITvButton.Condition => (o, i) => o is T t && Condition(t, i);
    Func<object?, Task> ITvAction.Action => (o) => o is T t ? Action(t) : Task.CompletedTask;
}
