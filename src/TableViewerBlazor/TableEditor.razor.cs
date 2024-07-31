namespace TableViewerBlazor;

public partial class TableEditor<T> : ComponentBase
    where T : class
{
    [Parameter] public T Data { get; set; } = default!;
    [Parameter] public EventCallback<T> DataChanged { get; set; }
    [Parameter] public TeOptions Options { get; set; } = new TeOptions();
    [Parameter] public EventCallback<bool> IsValidChanged { get; set; }
    [Parameter] public EventCallback<string[]> ErrorsChanged { get; set; }
    [Parameter] public RenderFragment<ICustomEditorArgument>? CustomEditor1 { get; set; }
    [Parameter] public RenderFragment<ICustomEditorArgument>? CustomEditor2 { get; set; }
    [Parameter] public RenderFragment<ICustomEditorArgument>? CustomEditor3 { get; set; }
    [Parameter] public RenderFragment<ICustomEditorArgument>? CustomEditor4 { get; set; }
    [Parameter] public RenderFragment<ICustomEditorArgument>? CustomEditor5 { get; set; }

    MudForm form = default!;
    bool success;
    string[] errors = { };

    protected override void OnInitialized()
    {
        SetCustomEditor();
    }

    private Task OnValidChanged(bool success)
    {
        this.success = success;
        return IsValidChanged.InvokeAsync(success);
    }

    private Task OnDataChanged(object? data)
    {
        if (data is T newData)
        {
            Data = newData;
            return DataChanged.InvokeAsync(newData);
        }
        return Task.CompletedTask;
    }

    private async Task OnErrorsChanged(string[] errors)
    {
        this.errors = errors;
        await ErrorsChanged.InvokeAsync(errors);
    }

    public Task Validate()
    {
        return form.Validate();
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
                Options.CustomEditorOptions.CustomEditor1.RenderFragment = CustomEditor1;
            }
        }
        if (CustomEditor2 != null)
        {
            if (Options.CustomEditorOptions.CustomEditor2 == null)
            {
                throw new Exception("Options.CustomEditorOptions.CustomEditor2 is null");
            }
            else
            {
                Options.CustomEditorOptions.CustomEditor2.RenderFragment = CustomEditor2;
            }
        }
        if (CustomEditor3 != null)
        {
            if (Options.CustomEditorOptions.CustomEditor3 == null)
            {
                throw new Exception("Options.CustomEditorOptions.CustomEditor3 is null");
            }
            else
            {
                Options.CustomEditorOptions.CustomEditor3.RenderFragment = CustomEditor3;
            }
        }
        if (CustomEditor4 != null)
        {
            if (Options.CustomEditorOptions.CustomEditor4 == null)
            {
                throw new Exception("Options.CustomEditorOptions.CustomEditor4 is null");
            }
            else
            {
                Options.CustomEditorOptions.CustomEditor4.RenderFragment = CustomEditor4;
            }
        }
        if (CustomEditor5 != null)
        {
            if (Options.CustomEditorOptions.CustomEditor5 == null)
            {
                throw new Exception("Options.CustomEditorOptions.CustomEditor5 is null");
            }
            else
            {
                Options.CustomEditorOptions.CustomEditor5.RenderFragment = CustomEditor5;
            }
        }
    }
}
