namespace TableViewerBlazor.Internal.TeComponent.CustomTableEditor;

public partial class CustomTableEditor2<T,
    TParent1, TItem1,
    TParent2, TItem2
    >
    : CustomTableEditor1<T,
        TParent1, TItem1
        > where T : class
{
    [Parameter] public RenderFragment<CustomEditorTypedArgument<TParent2, TItem2>>? CustomEditor2 { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        SetCustomEditor();
    }

    private void SetCustomEditor()
    {
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
