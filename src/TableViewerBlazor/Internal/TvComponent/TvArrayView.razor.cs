using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace TableViewerBlazor.Internal.TvComponent;

public partial class TvArrayView : TvViewBase
{
    [Parameter] public IEnumerable Data { get; set; } = null!;

    private IEnumerable<object?>? all;
    private bool HasAnyAction;
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
            HasAnyAction = all.Any(HasAction);
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

    private bool HasAction(object? item)
    {
        return Options?.Actions?.Any(action => action.Condition(item, Depth)) ?? false;
    }

    private void ToggleOpen()
    {
        Open = !Open;
    }
}

// https://stackoverflow.com/questions/4015930/when-to-use-cast-and-oftype-in-linq