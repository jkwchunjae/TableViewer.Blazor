namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeElementEditor : TeEditorBase
{
    /// <summary>
    /// FieldInfo, PropertyInfo
    /// </summary>
    [Parameter] public MemberInfo? MemberInfo { get; set; }
    [Parameter] public Type Type { get; set; } = default!;
    [Parameter] public string? Name { get; set; }
}
