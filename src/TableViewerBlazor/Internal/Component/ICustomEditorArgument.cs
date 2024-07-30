namespace TableViewerBlazor.Internal.Component;

public interface ICustomEditorArgument
{
    event EventHandler<object> ParentChanged;
    object? Parent { get; }
    object? Value { get; }
    Func<object?, Task> DataChanged { get; }
}

public class CustomEditorArguemnt : ICustomEditorArgument
{
    public event EventHandler<object> ParentChanged;
    public object? Parent { get; set; }
    public object? Value { get; set; }
    public Func<object?, Task> DataChanged { get; set; } = default!;
}
