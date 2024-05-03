using Microsoft.AspNetCore.Components;
using System.Collections;
using System.Reflection;

namespace TableViewerBlazor.Internal.TvComponent;

public partial class TvDictionaryView : TvViewBase
{
    [Parameter] public IDictionary Data { get; set; } = null!;

    bool Open = false;

    private IEnumerable<(object? Key, object? Value)> Items = Enumerable.Empty<(object?, object?)>();

    protected override void OnParametersSet()
    {
        if (Options != null)
        {
            Open = Depth <= OpenDepth;
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
            if (EnabledKey(key))
            {
                yield return (key, data[key]);
            }
        }

        bool EnabledKey(object key)
        {
            if (Options?.DisableKeys?.Any() ?? false)
            {
                if (key is string keystr)
                {
                    if (Options.DisableKeys.Any(disabled => disabled == keystr))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }

    private void ToggleOpen()
    {
        Open = !Open;
    }
}
