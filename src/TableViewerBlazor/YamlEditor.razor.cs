using BlazorMonaco.Editor;
using TableViewerBlazor.Internal.MonacoComponent;

namespace TableViewerBlazor;

public partial class YamlEditor : MonacoEditorBase
{
    private static readonly Random random = new Random((int)DateTime.Now.Ticks);
    private readonly string editorId = $"-monaco-yaml-{random.Next(11111, 99999)}";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender)
        {
            await UpdateEditorOptions();
            await Editor.SetValue(Data);
        }
    }

    private StandaloneEditorConstructionOptions EditorConstructionOptions(StandaloneCodeEditor editor)
    {
        return new StandaloneEditorConstructionOptions
        {
            Value = Data,
            Language = "yaml",
        };
    }

    private async Task UpdateEditorOptions()
    {
        if (Option != null)
        {
            await Editor.UpdateOptions(new EditorUpdateOptions
            {
                Minimap = Option.Minimap,
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
            });
        }
    }

    private async Task OnYamlContentChanged(ModelContentChangedEvent e)
    {
        var valueChanged = await Editor.GetValue();
        await DataChanged.InvokeAsync(valueChanged);
    }
}