namespace TableViewer.MewUI;

public class TableEditor3<T,
    TParent1, TItem1,
    TParent2, TItem2,
    TParent3, TItem3
    >
    : TableEditor2<T,
        TParent1, TItem1,
        TParent2, TItem2
        > where T : class
{
    public TableEditor3() { }
    public TableEditor3(T data, TeOptions? options = null) { Data = data; if (options is not null) Options = options; }

    public Func<CustomEditorArgument<TParent3, TItem3>, Element>? CustomEditor3 { get; set; }

protected override void SetCustomEditor()
    {
        base.SetCustomEditor();
        if (CustomEditor3 != null)
        {
            if (Options.CustomEditorOptions.CustomEditor3 == null)
            {
                throw new Exception("Options.CustomEditorOptions.CustomEditor3 is null");
            }
            else
            {
                Options.CustomEditorOptions.CustomEditor3.RenderFragment = ConvertRenderFragment(CustomEditor3);
            }
        }
    }
}

