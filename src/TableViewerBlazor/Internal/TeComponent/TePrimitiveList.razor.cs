namespace TableViewerBlazor.Internal.TeComponent;

public partial class TePrimitiveList : TeEditorBase
{
    [Parameter] public IList Items { get; set; } = default!;
    [Parameter] public TeArrayOption ArrayOption { get; set; } = default!;

    private IEnumerable<(int index, object item)> ItemsEnumerable => Items?.Cast<object>()
                .Select((item, index) => (index, item)) ?? default!;

    private async Task OnDataChanged(object item, int index)
    {

        await Js.InvokeVoidAsyncWithErrorHandling("console.log", item);
        Items[index] = item;
        await DataChanged.InvokeAsync(Items);
    }
}
