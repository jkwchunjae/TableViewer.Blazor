namespace TableViewerBlazor.Internal.TeComponent;

public partial class TePrimitiveList : TeEditorBase
{
    [Parameter] public IList Items { get; set; } = default!;

    private IEnumerable<object> ItemsEnumerable = null!;

    protected override void OnParametersSet()
    {
        if (Items != null)
        {
            ItemsEnumerable = Items.Cast<object>();
        }
    }

    private async Task OnDataChanged(object context)
    {
        await Js.InvokeVoidAsyncWithErrorHandling("console.log", context);
        await DataChanged.InvokeAsync(Data);
    }
}
