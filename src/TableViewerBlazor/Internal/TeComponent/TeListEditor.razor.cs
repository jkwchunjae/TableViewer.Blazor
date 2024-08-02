namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeListEditor : TeEditorBase
{
    [Parameter] public IList Items { get; set; } = default!;
    [Parameter] public ITeListEditorOption ArrayOption { get; set; } = default!;

    private IEnumerable<(int Index, object Item)> ItemsEnumerable => Items?.Cast<object>()
                .Select((item, index) => (index, item)) ?? default!;

    private async Task OnDataChanged(object item, int index)
    {
        await Js.InvokeVoidAsyncWithErrorHandling("console.log", item);
        Items[index] = item;
        await DataChanged.InvokeAsync(Items);
    }

    private ITvAction AddItemAction => new TvAction<object>
    {
        Action = _ => AddItem(),
        Label = ArrayOption.AddItemAction.Label,
        Style = ArrayOption.AddItemAction.Style,
    };

    private ITvAction RemoveItemAction => new TvAction<object>
    {
        Action = RemoveItem,
        Label = ArrayOption.RemoveItemAction.Label,
        Style = ArrayOption.RemoveItemAction.Style,
    };

    private async Task AddItem()
    {
        var itemType = Items.GetType().GenericTypeArguments[0];
        var item = CreateInstance(itemType);
        Items.Add(item);
        await DataChanged.InvokeAsync(Items);
    }

    private async Task RemoveItem(object item)
    {
        Items.Remove(item);
        await DataChanged.InvokeAsync(Items);
    }

    private object? CreateInstance(Type itemType)
    {
        if (itemType == typeof(string))
        {
            return string.Empty;
        }
        else
        {
            return Activator.CreateInstance(itemType);
        }
    }
}
