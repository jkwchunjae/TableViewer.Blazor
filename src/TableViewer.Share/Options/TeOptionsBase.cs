namespace TableViewer.Share.Options;

/// <summary>Editor-wide options shared by every presentation adapter.</summary>
public abstract class TeOptionsBase
{
    public string? Title { get; set; }
    public TeStyleOption Style { get; set; } = new();
}
