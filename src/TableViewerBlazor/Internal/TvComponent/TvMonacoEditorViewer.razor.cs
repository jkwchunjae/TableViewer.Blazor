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
            await Task.Delay(10);
            await Editor.Layout(new Dimension
            {
                Height = height * 20,
                Width = width * 10,
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