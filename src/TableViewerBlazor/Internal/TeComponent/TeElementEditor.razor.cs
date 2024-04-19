namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeElementEditor : ComponentBase
{
    /// <summary>
    /// FieldInfo, PropertyInfo
    /// </summary>
    [Parameter] public Type Type { get; set; } = default!;
    [Parameter] public string? Name { get; set; }
    [Parameter] public object? Data { get; set; }
}
