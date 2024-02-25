using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TableViewerBlazor.Internal;
using TableViewerBlazor.Options;

namespace TableViewerBlazor;

public partial class TableViewer : TvViewBase, IAsyncDisposable
{
    [Inject] public IJSRuntime Js { get; set; } = null!;
    [Parameter] public object? Data { get; set; }

    IJSObjectReference? module;

    protected override Task OnInitializedAsync()
    {
        if (Options == null)
        {
            Options = new TvOptions();
        }
        return base.OnInitializedAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                module = await Js.InvokeAsync<IJSObjectReference>("import", "./_content/TableViewerBlazor/js/table-viewer.js");
                var timezoneOptions = await module.InvokeAsync<TvDateTimeOptions>("getTimezoneOptions");
                if (Options != null)
                {
                    Options.DateTimeOptions = timezoneOptions;
                }
            }
            catch (Exception ex)
            {
                await Js.InvokeVoidAsync("console.error", ex.GetType().Name, ex.Message);
            }
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (module != null)
        {
            await module.DisposeAsync();
        }
    }
}
