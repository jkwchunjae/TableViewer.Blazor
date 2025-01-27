namespace TableViewerBlazor.Options.Property;

public interface ITeLabelOptions
{
    public Func<object, bool> Condition { get; }

    /// <summary>
    /// Label text if selected
    /// </summary>
    public Func<bool, object, string> SelectedLabel { get; }
    /// <summary>
    /// Label text if not selected
    /// </summary>
    public Func<bool, object, string> NotSelectedLabel { get; }
    /// <summary>
    /// The position of the Label text. Defaults to End.
    /// </summary>
    public Placement LabelPlacement { get; }
}

public class TeLabelOptions<T> : ITeLabelOptions
{
    public Func<T, bool> Condition { get; init; } = _ => true;
    public Func<bool, T, string>? SelectedLabel { get; init; }
    public Func<bool, T, string>? NotSelectedLabel { get; init; }
    public Placement LabelPlacement { get; init; } = Placement.End;

    Func<object, bool> ITeLabelOptions.Condition => o => o is T value ? Condition(value) : false;

    Func<bool, object, string> ITeLabelOptions.SelectedLabel => (b, o) =>
    {
        if (o is T value && SelectedLabel != null)
        {
            return SelectedLabel(b, value);
        }
        else
        {
            return string.Empty;
        }
    };

    Func<bool, object, string> ITeLabelOptions.NotSelectedLabel => (b, o) =>
    {
        if (o is T value && NotSelectedLabel != null)
        {
            return NotSelectedLabel(b, value);
        }
        else
        {
            return string.Empty;
        }
    };
}