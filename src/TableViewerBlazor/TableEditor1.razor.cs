namespace TableViewerBlazor;

public partial class TableEditor1<T, TParent1, TItem1> : TableEditorBase<T> where T : class
{
    [Parameter] public RenderFragment<CustomEditorArgument<TParent1, TItem1>>? CustomEditor1 { get; set; }

    protected override void OnInitialized()
    {
        SetCustomEditor();
    }

    protected virtual void SetCustomEditor()
    {
        if (CustomEditor1 != null)
        {
            if (Options.CustomEditorOptions.CustomEditor1 == null)
            {
                throw new Exception("Options.CustomEditorOptions.CustomEditor1 is null");
            }
            else
            {
                Options.CustomEditorOptions.CustomEditor1.RenderFragment = ConvertRenderFragment(CustomEditor1);
            }
        }
    }
}
