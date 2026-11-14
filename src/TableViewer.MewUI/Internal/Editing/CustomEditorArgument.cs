namespace TableViewer.MewUI.Internal.Editing;

public interface ICustomEditorArgument
{
    event EventHandler<object>? ParentChanged;
    object Parent { get; }
    object? Value { get; }
    Func<object?, Task> DataChanged { get; }
    void OnParentChanged(object parent);
}

public class CustomEditorArgument<TParent, TItem> : ICustomEditorArgument
{
    public event EventHandler<TParent>? ParentChanged;
    private event EventHandler<object>? UntypedParentChanged;
    public required TParent Parent { get; set; }
    public TItem? Value { get; set; }
    public Func<TItem?, Task> DataChanged { get; set; } = _ => Task.CompletedTask;
    public void OnParentChanged(object parent)
    {
        if (parent is TParent typed)
        {
            Parent = typed;
            ParentChanged?.Invoke(this, typed);
            UntypedParentChanged?.Invoke(this, typed!);
        }
    }
    object ICustomEditorArgument.Parent => Parent!;
    object? ICustomEditorArgument.Value => Value;
    Func<object?, Task> ICustomEditorArgument.DataChanged => value => value is TItem typed
        ? DataChanged(typed)
        : value is null ? DataChanged(default) : throw new ArgumentException($"Expected {typeof(TItem)}.");
    event EventHandler<object>? ICustomEditorArgument.ParentChanged { add => UntypedParentChanged += value; remove => UntypedParentChanged -= value; }
}
