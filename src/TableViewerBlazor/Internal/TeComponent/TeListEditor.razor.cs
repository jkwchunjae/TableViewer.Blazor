namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeListEditor : TeEditorBase
{
    [Parameter] public ITeListEditorOption ListEditorOption { get; set; } = default!;

    IList<object> Items = default!;

    private IEnumerable<(int Index, object Item)> ItemsEnumerable => Items
                .Select((item, index) => (index, item));

    protected override void OnInitialized()
    {
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
        Label = ListEditorOption.AddItemAction?.Label ?? string.Empty,
        Style = ListEditorOption.AddItemAction?.Style ?? new TvButtonStyle(),
    };

    private ITvAction RemoveItemAction => new TvAction<int>
    {
        Action = RemoveItem,
        Label = ListEditorOption.RemoveItemAction?.Label ?? string.Empty,
        Style = ListEditorOption.RemoveItemAction?.Style ?? new TvButtonStyle(),
    };

    private async Task AddItem(object dummy)
    {
        if (Items is IList listData)
        {
            await ListEditorOption.AddItemAction!.Action.Invoke(listData);
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
