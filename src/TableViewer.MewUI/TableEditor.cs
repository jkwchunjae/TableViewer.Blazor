namespace TableViewer.MewUI;

public class TableEditor<T> : TableEditorBase<T> where T : class
{
    public TableEditor() { }
    public TableEditor(T data, TeOptions? options = null) : base(data, options) { }
}
