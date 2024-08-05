namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeListEditor : TeEditorBase
{
    [Parameter] public ITeListEditorOption ListEditorOption { get; set; } = default!;

    IList<object> Items = default!;

    private IEnumerable<(int Index, object Item)> ItemsEnumerable => Items
                .Select((item, index) => (index, item));

    private object DefaultValue = default!;

    protected override void OnInitialized()
    {
        DefaultValue = ListEditorOption.CreateNew();
        if (Data != null)
        {
            var items = ListEditorOption.Converter.ToField(Data);
            Items = items?.Cast<object>().ToList() ?? new List<object>();
        }
        else
        {
            Items = new List<object>();
        }
    }

    private async Task OnDataChanged(object item, int index)
    {
        Items[index] = item;
        if (Items is IList listData)
        {
            Data = ListEditorOption.Converter.FromField(listData);
            await DataChanged.InvokeAsync(Data);
        }
    }

    private ITvAction AddItemAction => new TvAction<object>
    {
        Action = AddItem,
        Label = ListEditorOption.AddItemAction.Label,
        Style = ListEditorOption.AddItemAction.Style,
    };

    private ITvAction RemoveItemAction => new TvAction<int>
    {
        Action = RemoveItem,
        Label = ListEditorOption.RemoveItemAction.Label,
        Style = ListEditorOption.RemoveItemAction.Style,
    };

    private async Task AddItem(object defaultValue)
    {
        Items.Add(defaultValue);
        if (Items is IList listData)
        {
            Data = ListEditorOption.Converter.FromField(listData);
            await DataChanged.InvokeAsync(Data);
        }
    }

    private async Task RemoveItem(int index)
    {
        Items.RemoveAt(index);
        if (Items is IList listData)
        {
            Data = ListEditorOption.Converter.FromField(listData);
            await DataChanged.InvokeAsync(Data);
        }
    }
}
