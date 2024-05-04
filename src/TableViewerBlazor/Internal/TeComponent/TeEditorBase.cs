namespace TableViewerBlazor.Internal.TeComponent;

public class TeEditorBase : ComponentBase
{
    [Parameter] public object? Data { get; set; }
    [Parameter] public EventCallback<object?> DataChanged { get; set; }
    [Parameter] public TeOptions Options { get; set; } = default!;
    [Parameter] public int Depth { get; set; }
    [Parameter] public string Path { get; set; } = string.Empty;
    [Parameter] public string? Label { get; set; }
}
