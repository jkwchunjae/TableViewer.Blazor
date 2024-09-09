namespace TableViewerBlazor.Internal.TeComponent.CustomTableEditor;

public abstract class CustomTableEditorBase<T> : ComponentBase
{
    [Parameter] public T Data { get; set; } = default!;
    [Parameter] public EventCallback<T> DataChanged { get; set; }
    [Parameter] public TeOptions Options { get; set; } = new TeOptions();
    [Parameter] public EventCallback<bool> IsValidChanged { get; set; }
    [Parameter] public EventCallback<string[]> ErrorsChanged { get; set; }

    protected static RenderFragment<ICustomEditorArgument> ConvertRenderFragment<TParent, TItem>(
    RenderFragment<CustomEditorTypedArgument<TParent, TItem>> renderFragment)
    {
        return context =>
        {
            var typedContext = new CustomEditorTypedArgument<TParent, TItem>
            {
                Parent = (TParent)context.Parent,
                Value = (TItem?)context.Value,
                DataChanged = async (data) => await context.DataChanged(data)
            };

            context.ParentChanged += (sender, parent) =>
            {
                if (parent is TParent typedParent)
                {
                    typedContext.OnParentChanged(typedParent);
                }
            };
            return renderFragment(typedContext);
        };
    }
}
