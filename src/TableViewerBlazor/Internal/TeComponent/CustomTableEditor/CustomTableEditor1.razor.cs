namespace TableViewerBlazor.Internal.TeComponent.CustomTableEditor;

public partial class CustomTableEditor1<T, TParent1, TItem1> : CustomTableEditorBase where T : class
{
    [Parameter] public T Data { get; set; } = default!;
    [Parameter] public EventCallback<T> DataChanged { get; set; }
    [Parameter] public TeOptions Options { get; set; } = new TeOptions();
    [Parameter] public EventCallback<bool> IsValidChanged { get; set; }
    [Parameter] public EventCallback<string[]> ErrorsChanged { get; set; }

    [Parameter] public RenderFragment<CustomEditorTypedArgument<TParent1, TItem1>>? CustomEditor1 { get; set; }

    protected override void OnInitialized()
    {
        SetCustomEditor();
    }

    private void SetCustomEditor()
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
