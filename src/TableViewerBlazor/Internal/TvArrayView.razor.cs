using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace TableViewerBlazor.Internal;

public partial class TvArrayView : TvViewBase
{
    [Parameter] public IEnumerable Data { get; set; } = null!;

    private IEnumerable<object?>? all;
    private IEnumerable<(int Index, object? Item)>? DataArray;
    int VisibleItems = 2;
    bool Open = false;

    bool HasMoreItems => DataArray?.Count() < all?.Count();

    protected override void OnInitialized()
    {
        if (Options != null)
        {
            Open = Depth <= Options.OpenDepth;
            if (VisibleItems < Options.ArrayVisibleDepth)
            {
                VisibleItems = Options.ArrayVisibleDepth;
            }
            all = Data.Cast<object>();
            DataArray = all
                .Select((item, index) => (index, item))
                .Take(VisibleItems);
        }
    }

    private void MoreItems()
    {
        VisibleItems *= 2;
        DataArray = all?
            .Select((item, index) => (index, item))
            .Take(VisibleItems);
        StateHasChanged();
    }

    private void ToggleOpen()
    {
        Open = !Open;
    }
}

// https://stackoverflow.com/questions/4015930/when-to-use-cast-and-oftype-in-linq