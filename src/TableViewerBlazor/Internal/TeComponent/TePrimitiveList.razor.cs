namespace TableViewerBlazor.Internal.TeComponent;

public partial class TePrimitiveList : TeEditorBase
{
    [Parameter] public IList Items { get; set; } = default!;
    [Parameter] public TeArrayOption ArrayOption { get; set; } = default!;

    private IEnumerable<(int index, object item)> ItemsEnumerable = null!;

    protected override void OnParametersSet()
    {
        if (Items != null)
        {
            ItemsEnumerable = Items.Cast<object>()
                .Select((item, index )=> (index, item));
        }
    }

    private async Task OnDataChanged(object context)
    {
        await Js.InvokeVoidAsyncWithErrorHandling("console.log", context);
        await DataChanged.InvokeAsync(Data);
    }
}
