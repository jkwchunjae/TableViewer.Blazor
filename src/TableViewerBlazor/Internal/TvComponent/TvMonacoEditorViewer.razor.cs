using System.Text.Json;
using BlazorMonaco.Editor;

namespace TableViewerBlazor.Internal.TvComponent;

public partial class TvMonacoEditorViewer : TvViewBase
{
    [Parameter] public object? Data { get; set; }
    static Random random = new Random((int)DateTime.Now.Ticks);
    string EditorId = $"tv-monaco-{random.Next(11111, 99999)}";
    StandaloneCodeEditor Editor { get; set; } = null!;

    protected override Task OnInitializedAsync()
    {
        return base.OnInitializedAsync();
    }

    private StandaloneEditorConstructionOptions EditorConstructionOptions(StandaloneCodeEditor editor)
    {
        Func<object, string> func = (data) => JsonSerializer.Serialize(Data, new JsonSerializerOptions
        {
            WriteIndented = true,
        });
        var serializer = Options.Editor.Serializer ?? func;
        var value = serializer(Data!);
        var lines = value.Split('\n');
        var lineCount = lines.Count();
        var maxWidth = lines.Max(x => x.Length);

        return new StandaloneEditorConstructionOptions
        {
            AutomaticLayout = true,
            Language = Options.Editor.Language,
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