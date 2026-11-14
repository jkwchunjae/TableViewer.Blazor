namespace TableViewer.MewUI;

public class TableEditor1<T, TParent1, TItem1> : TableEditorBase<T> where T : class
{
    public TableEditor1() { }
    public TableEditor1(T data, TeOptions? options = null) { Data = data; if (options is not null) Options = options; }

    public Func<CustomEditorArgument<TParent1, TItem1>, Element>? CustomEditor1 { get; set; }

    protected override void SetCustomEditor()
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

