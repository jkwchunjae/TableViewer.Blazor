using System.Text.Json;
using BlazorMonaco.Editor;
using Microsoft.JSInterop;

namespace TableViewerBlazor.Internal.TvComponent;

public partial class TvMonacoEditorViewer : TvViewBase
{
    [Inject] public IJSRuntime Js { get; set; } = null!;
    [Parameter] public object? Data { get; set; }
    [Parameter] public ITvEditorOption EditorOption { get; set; } = null!;

    static Random random = new Random((int)DateTime.Now.Ticks);
    string EditorId = $"tv-monaco-{random.Next(11111, 99999)}";
    StandaloneCodeEditor Editor { get; set; } = null!;

    int height = 1;
    int width = 1;

    protected override Task OnInitializedAsync()
    {
        InvokeAsync(async () =>
        {
            // TODO: 좀 더 좋은 방법을 찾아야 함.
            await Task.Delay(100);
            var editorHeight = Math.Min(height * 20, EditorOption.LayoutMaxSize?.Height ?? int.MaxValue);
            var editorWidth = Math.Min(width * 10, EditorOption.LayoutMaxSize?.Width ?? int.MaxValue);
            await Editor.Layout(new Dimension
            {
                Height = editorHeight,
                Width = editorWidth,
            });
            StateHasChanged();
        });
        return base.OnInitializedAsync();
    }

    private StandaloneEditorConstructionOptions EditorConstructionOptions(StandaloneCodeEditor editor)
    {
        Func<object, string> func = (data) => JsonSerializer.Serialize(Data, new JsonSerializerOptions
        {
            WriteIndented = true,
        });
        var serializer = EditorOption.Serializer ?? func;
        var value = serializer(Data!);
        var lines = value.Split('\n');
        var lineCount = lines.Count();
        var maxWidth = lines.Max(x => x.Length);
        height = lineCount;
        width = maxWidth;

        return new StandaloneEditorConstructionOptions
        {
            AutomaticLayout = true,
            Language = EditorOption.Language,
            Value = value,
            ReadOnly = true,
            LineNumbers = RenderLineNumbersType.Off.ToString(),
            Minimap = new EditorMinimapOptions
            {
                Enabled = false,
            },
            GlyphMargin = false,
            Folding = false,
        };
    }
}