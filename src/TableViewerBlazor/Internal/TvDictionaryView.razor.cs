using Microsoft.AspNetCore.Components;
using System.Collections;
using System.Reflection;

namespace TableViewerBlazor.Internal;

public partial class TvDictionaryView : TvViewBase
{
    [Parameter] public IDictionary Data { get; set; } = null!;

    bool Open = false;

    private IEnumerable<(object? Key, object? Value)> Items = Enumerable.Empty<(object?, object?)>();

    protected override void OnInitialized()
    {
        if (Options != null)
        {
            Open = Depth <= Options.OpenDepth;
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
