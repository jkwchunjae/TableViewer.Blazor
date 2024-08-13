using BlazorMonaco.Editor;

namespace TableViewerBlazor.Options;

public interface IMonacoEditorOption
{
    /// <summary>
    /// The initial editor dimension (to avoid measuring the container).
    /// </summary>
    EditorDimension Dimension { get; }

    /// <summary>
    /// The initial language of the auto created model in the editor. To not automatically create a model, use model: null.
    /// </summary>
    public string? Language { get; }

    /// <summary>
    /// Control the behavior and rendering of the minimap.
    /// </summary>
    public EditorMinimapOptions Minimap { get; }

    /// <summary>
    /// Control the rendering of line numbers. If it is a function, it will be invoked when rendering a line number and the return value will be rendered. Otherwise, if it is a truthy, line numbers will be rendered normally (equivalent of using an identity function). Otherwise, line numbers will not be rendered. Defaults to on.
    /// </summary>
    public string LineNumbers { get; }

    /// <summary>
    /// Enable the rendering of the glyph margin. Defaults to true in vscode and to false in monaco-editor.
    /// </summary>
    public bool GlyphMargin { get; }

    /// <summary>
    /// Enable code folding. Defaults to true.
    /// </summary>
    public bool Folding { get; }

    /// <summary>
    /// Should the textarea used for input use the DOM readonly attribute. Defaults to false.
    /// </summary>
    public bool ReadOnly { get; }

    /// <summary>
    /// Initial theme to be used for rendering. The current out-of-the-box available themes are: 'vs' (default), 'vs-dark', 'hc-black', 'hc-light. You can create custom themes via monaco.editor.defineTheme. To switch a theme, use monaco.editor.setTheme. NOTE: The theme might be overwritten if the OS is in high contrast mode, unless autoDetectHighContrast is set to false.
    /// </summary>
    public string Theme { get; }
    /// <summary>
    /// Control the wrapping of the editor.When wordWrap = "off", the lines will never wrap.When wordWrap = "on", the lines will wrap at the viewport width. When wordWrap = "wordWrapColumn", the lines will wrap at wordWrapColumn. When wordWrap = "bounded", the lines will wrap at min(viewport width, wordWrapColumn). Defaults to "off".
    /// </summary>
    /// <remarks>
    /// wordWrap?: "off" | "on" | "wordWrapColumn" | "bounded"
    /// </remarks>
    public string WordWrap { get; }

    /// <summary>
    /// Enable that the editor will install a ResizeObserver to check if its container dom node size has changed. Defaults to false.
    /// </summary>
    public bool AutomaticLayout { get; }

    /// <summary>
    /// The font size
    /// </summary>
    public int FontSize { get; }

    /// <summary>
    /// Enable that scrolling can go one screen size after the last line. Defaults to true.
    /// </summary>
    public bool ScrollBeyondLastLine { get; }

    /// <summary>
    /// The number of spaces a tab is equal to. This setting is overridden based on the file contents when detectIndentation is on. Defaults to 4.
    /// </summary>
    public int TabSize { get; }

    /// <summary>
    /// Enable rendering of whitespace. Defaults to 'selection'.
    /// </summary>
    /// <remarks>
    /// renderWhitespace?: "all" | "none" | "boundary" | "selection" | "trailing"
    /// </remarks>
    public string RenderWhitespace { get; }

    /// <summary>
    /// Enable snippet suggestions.
    /// </summary>
    /// <remarks>
    /// snippetSuggestions?: "none" | "top" | "bottom" | "inline"
    /// </remarks>
    public string SnippetSuggestions { get; }

    /// <summary>
    /// Controls whether completions should be computed based on words in the document. Defaults to true.
    /// </summary>
    /// <remarks>
    /// wordBasedSuggestions?: "off" | "currentDocument" | "matchingDocuments" | "allDocuments"
    /// </remarks>
    public bool WordBasedSuggestions { get; }

    /// <summary>
    /// Enable that the editor animates scrolling to a position. Defaults to false.
    /// </summary>
    public bool SmoothScrolling { get; }

    /// <summary>
    /// Control the cursor animation style, possible values are 'blink', 'smooth', 'phase', 'expand' and 'solid'. Defaults to 'blink'.
    /// </summary>
    /// <remarks>
    /// cursorBlinking?: "blink" | "smooth" | "phase" | "expand" | "solid"
    /// </remarks>
    public string CursorBlinking { get; }
}

public class MonacoEditorOption : IMonacoEditorOption
{
    public EditorDimension Dimension { get; set; } = new();
    public string? Language { get; set; }
    public EditorMinimapOptions Minimap { get; set; } = new();
    public string LineNumbers { get; set; } = RenderLineNumbersType.On.ToString();
    public bool GlyphMargin { get; set; } = true;
    public bool Folding { get; set; } = true;
    public bool ReadOnly { get; set; } = false;
    public string Theme { get; set; } = "vs";
    public string WordWrap { get; set; } = "off";
    public bool AutomaticLayout { get; set; } = false;
    public int FontSize { get; set; } = 12;
    public bool ScrollBeyondLastLine { get; set; } = true;
    public int TabSize { get; set; } = 4;
    public string RenderWhitespace { get; set; } = "selection";
    public string SnippetSuggestions { get; set; } = "inline";
    public bool WordBasedSuggestions { get; set; } = true;
    public bool SmoothScrolling { get; set; } = true;
    public string CursorBlinking { get; set; } = "blink";
}

public class EditorDimension
{
    public int Height { get; set; }
    public int Width { get; set; }
}