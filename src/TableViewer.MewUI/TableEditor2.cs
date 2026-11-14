namespace TableViewer.MewUI;

public class TableEditor2<T,
    TParent1, TItem1,
    TParent2, TItem2
    >
    : TableEditor1<T,
        TParent1, TItem1
        > where T : class
{
    public TableEditor2() { }
    public TableEditor2(T data, TeOptions? options = null) { Data = data; if (options is not null) Options = options; }

    public Func<CustomEditorArgument<TParent2, TItem2>, Element>? CustomEditor2 { get; set; }

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

