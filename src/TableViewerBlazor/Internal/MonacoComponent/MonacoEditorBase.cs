using BlazorMonaco.Editor;
using Microsoft.JSInterop;

namespace TableViewerBlazor.Internal.MonacoComponent;

public class MonacoEditorBase : ComponentBase
{
    [Inject] public IJSRuntime Js { get; set; } = default!;
    [Parameter] required public string Data { get; set; } = string.Empty;
    [Parameter] public EventCallback<string> DataChanged { get; set; }
    [Parameter] public IMonacoEditorOption? Option { get; set; }
    [Parameter] public string? Label { get; set; }

    protected StandaloneCodeEditor Editor { get; set; } = null!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await Task.Delay(100);
            if (Editor != null)
            {
                var dimension = GetEditorDimension();
                await Editor.Layout(dimension);
            }
        }
    }

    private Dimension GetEditorDimension()
    {
        var lines = Data.Split('\n');
        int editorHeight;
        int editorWidth;
        if (Option == null)
        {
            editorHeight = lines.Count() * 20;
            editorWidth = lines.Max(x => x.Length) * 10;
        }
        else
        {
            editorHeight = Option.Dimension.Height != 0
                ? Option.Dimension.Height
                : lines.Count() * 20;
            editorWidth = Option.Dimension.Width != 0
                ? Option.Dimension.Width
                : lines.Max(x => x.Length) * 10;
        }
        return new Dimension
        {
            Height = editorHeight,
            Width = editorWidth,
        };
    }
}
