namespace TableViewer.MewUI.Internal.Editing;

/// <summary>The value and metadata used during editor-option resolution.</summary>
public class TeEditorBase
{
    public object? Data { get; set; }
    public Func<object?, Task>? DataChanged { get; set; }
    public TeOptions Options { get; set; } = new();
    public int Depth { get; set; }
    public string Path { get; set; } = "";
    public string? Label { get; set; }
}
