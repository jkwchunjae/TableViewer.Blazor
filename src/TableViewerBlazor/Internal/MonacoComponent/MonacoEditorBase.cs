using BlazorMonaco.Editor;
using Microsoft.JSInterop;

namespace TableViewerBlazor.Internal.MonacoComponent;

public class MonacoEditorBase : ComponentBase
{
    [Inject] public IJSRuntime Js { get; set; } = default!;
    [Parameter] required public string Data { get; set; } = string.Empty;
    [Parameter] public EventCallback<string> DataChanged { get; set; }
    [Parameter] public IMonacoEditorOption Option { get; set; } = default!;
    [Parameter] public string? Label { get; set; }

    protected StandaloneCodeEditor Editor { get; set; } = null!;
    private bool isLayoutSet = false;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await Task.Delay(100);
        if (!isLayoutSet && Editor != null)
        {
            var dimension = GetEditorDimension();
            await Editor.Layout(dimension);
            isLayoutSet = true;
        }
    }

    private Dimension GetEditorDimension()
    {
        var lines = Data.Split('\n');
        var editorHeight = Option.Dimension.Height != 0
            ? Option.Dimension.Height
            : lines.Count() * 20;
        var editorWidth = Option.Dimension.Width != 0
            ? Option.Dimension.Width
            : lines.Max(x => x.Length) * 10;
        return new Dimension
        {
            Height = editorHeight,
            Width = editorWidth,
        };
    }
}
