namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeListEditor : TeEditorBase
{
    [Parameter] public IList Items { get; set; } = default!;
    [Parameter] public ITeListEditorOption ArrayOption { get; set; } = default!;

    private IEnumerable<(int Index, object Item)> ItemsEnumerable => Items.Cast<object>()
                .Select((item, index) => (index, item));

    private object? DefaultValue;

    protected override void OnInitialized()
    {
        DefaultValue = ArrayOption.CreateNew();
    }

    private async Task OnDataChanged(object item, int index)
    {
        Items[index] = item;
        await DataChanged.InvokeAsync(Items);
    }

    private ITvAction AddItemAction => new TvAction<object>
    {
        Action = AddItem,
        Label = ArrayOption.AddItemAction.Label,
        Style = ArrayOption.AddItemAction.Style,
    };

    private ITvAction RemoveItemAction => new TvAction<int>
    {
        Action = RemoveItem,
        Label = ArrayOption.RemoveItemAction.Label,
        Style = ArrayOption.RemoveItemAction.Style,
    };

    private async Task AddItem(object? defaultValue)
    {
        Items.Add(defaultValue);
        await DataChanged.InvokeAsync(Items);
    }

    private async Task RemoveItem(int index)
    {
        Items.RemoveAt(index);
        await DataChanged.InvokeAsync(Items);
    }
}
