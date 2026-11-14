namespace TableViewer.MewUI;

public class TableEditor4<T,
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
    public TableEditor4() { }
    public TableEditor4(T data, TeOptions? options = null) { Data = data; if (options is not null) Options = options; }

    public Func<CustomEditorArgument<TParent4, TItem4>, Element>? CustomEditor4 { get; set; }

protected override void SetCustomEditor()
    {
        base.SetCustomEditor();
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

