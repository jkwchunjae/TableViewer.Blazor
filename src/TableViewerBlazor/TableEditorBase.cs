namespace TableViewerBlazor;

public abstract class TableEditorBase<T> : ComponentBase
{
    [Parameter] public T Data { get; set; } = default!;
    [Parameter] public EventCallback<T> DataChanged { get; set; }
    [Parameter] public TeOptions Options { get; set; } = new TeOptions();
    [Parameter] public EventCallback<bool> IsValidChanged { get; set; }
    [Parameter] public EventCallback<string[]> ErrorsChanged { get; set; }

    protected MudForm form = default!;
    protected bool success;
    protected string[] errors = { };

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

    protected static RenderFragment<ICustomEditorArgument> ConvertRenderFragment<TParent, TItem>(
        RenderFragment<CustomEditorArgument<TParent, TItem>> renderFragment)
    {
        return context =>
        {
            CustomEditorArgument<TParent, TItem> typedContext;
            if (context.Parent is TParent tParent && context.Value is TItem tValue)
            {
                typedContext = new CustomEditorArgument<TParent, TItem>
                {
                    Parent = tParent,
                    Value = tValue,
                    DataChanged = async (data) => await context.DataChanged(data)
                };
                context.ParentChanged += (sender, parent) =>
                {
                    if (parent is TParent typedParent)
                    {
                        typedContext.OnParentChanged(typedParent);
                    }
                };
            }
            else
            {
                typedContext = new CustomEditorArgument<TParent, TItem>() { Parent = default! };
            }
            return renderFragment(typedContext);
        };
    }
}
