namespace TableViewerBlazor;

public partial class TableEditor<T> : ComponentBase
    where T : class
{
    [Parameter] public T Data { get; set; } = default!;
    [Parameter] public EventCallback<T> DataChanged { get; set; }
    [Parameter] public TeOptions Options { get; set; } = new TeOptions();
    [Parameter] public EventCallback<bool> IsValidChanged { get; set; }
    [Parameter] public EventCallback<string[]> ErrorsChanged { get; set; }

    [Parameter] public RenderFragment<ICustomEditorArgument>? CustomEditor { get; set; }

    MudForm form = default!;
    bool success;
    string[] errors = { };

    protected override void OnInitialized()
    {
        Options.CustomEditor = CustomEditor;
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
}
