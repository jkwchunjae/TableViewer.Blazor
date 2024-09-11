namespace TableViewerBlazor.Internal.Component;

public interface ICustomEditorArgument
{
    event EventHandler<object> ParentChanged;
    object Parent { get; }
    object? Value { get; }
    Func<object?, Task> DataChanged { get; }
    void OnParentChanged(object obj);
}

public class CustomEditorArgument<TParent, TItem> : ICustomEditorArgument
{
    public event EventHandler<TParent> ParentChanged = default!;
    public required TParent Parent { get; set; }
    public TItem? Value { get; set; }
    public Func<TItem?, Task> DataChanged { get; set; } = default!;

    public void OnParentChanged(object parent)
    {
        if (parent is TParent tParent)
        {
            ParentChanged?.Invoke(this, tParent);
        }
    }

    object ICustomEditorArgument.Parent => Parent!;

    object? ICustomEditorArgument.Value => Value;

    Func<object?, Task> ICustomEditorArgument.DataChanged => data => data is TItem item ? DataChanged(item) : default!;

    Dictionary<EventHandler<object>, EventHandler<TParent>> dic = new();

    event EventHandler<object> ICustomEditorArgument.ParentChanged
    {
        add
        {
            EventHandler<TParent> handler = (sender, parent) =>
            {
                if (parent is TParent typedParent)
                {
                    value(sender, typedParent);
                }
            };
            dic[value] = handler;
            ParentChanged += handler;
        }
        remove
        {
            if (dic.TryGetValue(value, out var handler))
            {
                ParentChanged -= handler;
                dic.Remove(value);
            }
        }
    }
}

