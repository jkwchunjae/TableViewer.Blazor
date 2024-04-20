namespace TableViewerBlazor;

public partial class TableEditor<T> : ComponentBase
{
    [Parameter] public T Data { get; set; } = default!;
    [Parameter] public EventCallback<T> DataChanged { get; set; }
    [Parameter] public TeOptions Options { get; set; } = new TeOptions();

    private async Task TestClick()
    {
        await DataChanged.InvokeAsync(Data);
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
}
