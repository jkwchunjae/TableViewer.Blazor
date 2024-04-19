namespace TableViewerBlazor;

public partial class TableEditor<T> : ComponentBase
{
    [Parameter] public T Data { get; set; } = default!;
    [Parameter] public EventCallback<T> DataChanged { get; set; }

    private async Task TestClick()
    {
        await DataChanged.InvokeAsync(Data);
    }
}
