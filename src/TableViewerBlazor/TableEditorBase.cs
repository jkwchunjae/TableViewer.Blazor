namespace TableViewerBlazor;

public abstract class TableEditorBase<T> : ComponentBase, IDisposable
{
    [Parameter] public T Data { get; set; } = default!;
    [Parameter] public EventCallback<T> DataChanged { get; set; }
    [Parameter] public TeOptions Options { get; set; } = new TeOptions();
    [Parameter] public EventCallback<bool> IsValidChanged { get; set; }
    [Parameter] public EventCallback<string[]> ErrorsChanged { get; set; }

    protected MudForm form = default!;
    protected bool success;
    protected string[] errors = { };

    private readonly List<(ICustomEditorArgument argument, EventHandler<object> handler)> argumentsAndHandlers = [];

    protected Task OnValidChanged(bool success)
    {
        this.success = success;
        return IsValidChanged.InvokeAsync(success);
    }

    protected Task OnDataChanged(object? data)
    {
        if (data is T newData)
        {
            Data = newData;
            return DataChanged.InvokeAsync(newData);
        }
        return Task.CompletedTask;
    }

    protected async Task OnErrorsChanged(string[] errors)
    {
        this.errors = errors;
        await ErrorsChanged.InvokeAsync(errors);
    }

    protected RenderFragment<ICustomEditorArgument> ConvertRenderFragment<TParent, TItem>(
        RenderFragment<CustomEditorArgument<TParent, TItem>> renderFragment)
    {
        return context =>
        {
            if (context.Parent is TParent tParent && context.Value is TItem tValue)
            {
                var typedContext = new CustomEditorArgument<TParent, TItem>
                {
                    Parent = tParent,
                    Value = tValue,
                    DataChanged = async (data) => await context.DataChanged(data)
                };
                EventHandler<object> handler = (sender, parent) =>
                {
                    if (parent is TParent typedParent)
                    {
                        typedContext.OnParentChanged(typedParent);
                    }
                };
                context.ParentChanged += handler;
                argumentsAndHandlers.Add((context, handler));

                return renderFragment(typedContext);
            }
            else
            {
                throw new Exception("Converting RenderFragment is failed. Check out the types of data");
            }
        };
    }

    public Task Validate()
    {
        return form.Validate();
    }

    public void Dispose()
    {
        foreach (var (argument, handler) in argumentsAndHandlers)
        {
            argument.ParentChanged -= handler;
        }
    }
}
