namespace TableViewerBlazor;

public partial class TableEditor4<T,
    TParent1, TItem1,
    TParent2, TItem2,
    TParent3, TItem3,
    TParent4, TItem4
    >
    : TableEditor3<T,
        TParent1, TItem1,
        TParent2, TItem2,
        TParent3, TItem3
        > where T : class
{
    [Parameter] public RenderFragment<CustomEditorTypedArgument<TParent4, TItem4>>? CustomEditor4 { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        SetCustomEditor();
    }

    private void SetCustomEditor()
    {
        if (CustomEditor4 != null)
        {
            if (Options.CustomEditorOptions.CustomEditor4 == null)
            {
                throw new Exception("Options.CustomEditorOptions.CustomEditor4 is null");
            }
            else
            {
                Options.CustomEditorOptions.CustomEditor4.RenderFragment = ConvertRenderFragment(CustomEditor4);
            }
        }
    }
}
