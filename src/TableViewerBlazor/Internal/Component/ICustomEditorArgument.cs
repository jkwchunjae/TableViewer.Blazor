namespace TableViewerBlazor.Internal.Component;

public interface IUpdatable
{
    void Update(object parent);
}

public interface ICustomEditorArgument : IUpdatable
{
    event EventHandler<object> ParentChanged;
    object Parent { get; }
    object? Value { get; }
    Func<object?, Task> DataChanged { get; }

    CustomEditorTypedArgument<TParent, TItem> Convert<TParent, TItem>();
}

public class CustomEditorArguemnt : ICustomEditorArgument
{
    public event EventHandler<object> ParentChanged = default!;
    public required object Parent { get; set; }
    public object? Value { get; set; }
    public Func<object?, Task> DataChanged { get; set; } = default!;

    List<IUpdatable> updatables = new List<IUpdatable>();

    public void Update(object parent)
    {
        ParentChanged?.Invoke(this, parent);
        foreach (var updatable in updatables)
        {
            try
            {
                updatable.Update(parent);
            }
            finally
            {
            }
        }
    }

    public CustomEditorTypedArgument<TParent, TItem> Convert<TParent, TItem>()
    {
        if (Parent is TParent parent)
        {
            if (Value is TItem item)
            {
                var arg = new CustomEditorTypedArgument<TParent, TItem>()
                {
                    Parent = parent,
                    Value = item,
                    DataChanged = value => DataChanged(value),
                };
                updatables.Add(arg);
                return arg;
            }
            else if (Value == null)
            {
                var arg = new CustomEditorTypedArgument<TParent, TItem>()
                {
                    Parent = parent,
                    Value = default,
                    DataChanged = value => DataChanged(value),
                };
                updatables.Add(arg);
                return arg;
            }
        }
        throw new Exception();
    }
}

public class CustomEditorTypedArgument<TParent, TItem> : IUpdatable
{
    public event EventHandler<TParent> ParentChanged = default!;
    public required TParent Parent { get; set; }
    public TItem? Value { get; set; }
    public Func<TItem?, Task> DataChanged { get; set; } = default!;

    public void Update(object parent)
    {
        if (parent is TParent tParent)
        {
            ParentChanged?.Invoke(this, tParent);
        }
    }
}

