namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeElementEditor : TeEditorBase
{
    /// <summary>
    /// FieldInfo, PropertyInfo
    /// </summary>
    [Parameter] public MemberInfo? MemberInfo { get; set; }
}
