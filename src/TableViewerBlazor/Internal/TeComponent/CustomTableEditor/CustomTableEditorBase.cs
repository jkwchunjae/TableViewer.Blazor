namespace TableViewerBlazor.Internal.TeComponent.CustomTableEditor;

public class CustomTableEditorBase : ComponentBase
{
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
