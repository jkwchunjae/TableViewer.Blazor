namespace TableViewerBlazor.Internal.TvComponent;

public partial class TvDictionaryView : TvViewBase
{
    [Parameter] public IDictionary Data { get; set; } = null!;
    [Parameter] public bool Loading { get; set; }

    bool? Open = null;

    private IEnumerable<(object? Key, object? Value)> Items = Enumerable.Empty<(object?, object?)>();

    protected override void OnParametersSet()
    {
        if (Options != null)
        {
            Open ??= Depth <= OpenDepth;
        }
        if (Data != null)
        {
            Items = Convert(Data);
        }
    }

    private IEnumerable<(object? Key, object? Value)> Convert(IDictionary data)
    {
        foreach (var key in data.Keys)
        {
            yield return (key, data[key]);
        }
    }

    private void ToggleOpen()
    {
        Open = !Open;
    }
}
