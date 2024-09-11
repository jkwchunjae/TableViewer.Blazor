namespace TableViewerBlazor;

public partial class TableEditor2<T,
    TParent1, TItem1,
    TParent2, TItem2
    >
    : TableEditor1<T,
        TParent1, TItem1
        > where T : class
{
    [Parameter] public RenderFragment<CustomEditorArgument<TParent2, TItem2>>? CustomEditor2 { get; set; }

    protected override void OnInitialized()
    {
        SetCustomEditor();
    }

    protected override void SetCustomEditor()
    {
        base.SetCustomEditor();
        if (CustomEditor2 != null)
        {
            if (Options.CustomEditorOptions.CustomEditor2 == null)
            {
                throw new Exception("Options.CustomEditorOptions.CustomEditor2 is null");
            }
            else
            {
                Options.CustomEditorOptions.CustomEditor2.RenderFragment = ConvertRenderFragment(CustomEditor2);
            }
        }
    }
}
