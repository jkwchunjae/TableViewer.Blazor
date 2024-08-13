using BlazorMonaco.Editor;
using TableViewerBlazor.Internal.MonacoComponent;

namespace TableViewerBlazor;

public partial class YamlEditor : MonacoEditorBase
{
    private static readonly Random random = new Random((int)DateTime.Now.Ticks);
    private readonly string editorId = $"-monaco-yaml-{random.Next(11111, 99999)}";

    private StandaloneEditorConstructionOptions EditorConstructionOptions(StandaloneCodeEditor editor)
    {
        return new StandaloneEditorConstructionOptions
        {
            Value = Data,
            Language = Option.Language == null ? "yaml" : Option.Language,
            Minimap = Option.MiniMap,
            LineNumbers = Option.LineNumbers,
            GlyphMargin = Option.GlyphMargin,
            Folding = Option.Folding,
            ReadOnly = Option.ReadOnly,
            Theme = Option.Theme,
            WordWrap = Option.WordWrap,
            AutomaticLayout = Option.AutomaticLayout,
            FontSize = Option.FontSize,
            ScrollBeyondLastLine = Option.ScrollBeyondLastLine,
            TabSize = Option.TabSize,
            RenderWhitespace = Option.RenderWhitespace,
            SnippetSuggestions = Option.SnippetSuggestions,
            WordBasedSuggestions = Option.WordBasedSuggestions,
            SmoothScrolling = Option.SmoothScrolling,
            CursorBlinking = Option.CursorBlinking,
        };
    }

    private async Task OnYamlContentChanged(ModelContentChangedEvent e)
    {
        var valueChanged = await Editor.GetValue();
        await DataChanged.InvokeAsync(valueChanged);
    }
}