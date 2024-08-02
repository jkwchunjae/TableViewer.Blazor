namespace TableViewerBlazor.Internal.TeComponent;

public partial class TePrimitiveList : TeEditorBase
{
    [Parameter] public IList Items { get; set; } = default!;
    [Parameter] public TeArrayOption ArrayOption { get; set; } = default!;

    private IEnumerable<(int index, object item)> ItemsEnumerable => Items?.Cast<object>()
                .Select((item, index) => (index, item)) ?? default!;

    private string addType = "string";

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

    private async Task AddItem()
    {
        var instance = CreateInstance();
        Items.Add(instance);
        await DataChanged.InvokeAsync(Items);
    }

    private object CreateInstance()
    {
        return addType switch
        {
            "string" => string.Empty,
            "int" => default(int),
            "float" => default(float),
            "bool" => default(bool),
            _ => string.Empty,
        };
    }
}
