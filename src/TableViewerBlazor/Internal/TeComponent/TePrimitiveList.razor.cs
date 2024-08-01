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
        Data = context;
        await DataChanged.InvokeAsync(context);
    }
}
