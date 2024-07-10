using MudBlazor;

namespace TableViewerBlazor.Options;

public interface ITvAction
{
    public Func<object?, int, bool> Condition { get; }
    public Func<object?, Task> Action { get; }
    public string Label { get; }
    public string LabelAfterClick { get; }
    public TvButtonStyle Style { get; }
    public Link? Link { get; }
}

public class TvAction<T> : ITvAction
{
    public Func<T , int, bool> Condition { get; set; } = (_, _) => true;
    public Func<T, Task> Action { get; set; } = _ => Task.CompletedTask;
    public string Label { get; set; } = "ACTION";
    public string LabelAfterClick { get; set; } = "DONE";
    public TvButtonStyle Style { get; set; } = new();
    public Link? Link { get; set; }

    Func<object?, int, bool> ITvAction.Condition => (o, i) => o is T t && Condition(t, i);
    Func<object?, Task> ITvAction.Action => (o) => o is T t ? Action(t) : Task.CompletedTask;
}

public class TvButtonStyle
{
    public Variant Variant { get; set; } = Variant.Outlined;
    public Size Size { get; set; } = Size.Small;
    public Color Color { get; set; } = Color.Default;
    public string Style { get; set; } = "text-transform: none;";
    public string? StartIcon { get; set; } = null;
    public string? EndIcon { get; set; } = null;
    public Size IconSize { get; set; } = Size.Small;
    public Color IconColor { get; set; } = Color.Default;
    public bool Dense { get; set; } = true;
    public bool SuperDense { get; set; } = false;
}

public class Link
{
    public string Target { get; set; } = "_self";
    public string Href { get; set; } = string.Empty;
}