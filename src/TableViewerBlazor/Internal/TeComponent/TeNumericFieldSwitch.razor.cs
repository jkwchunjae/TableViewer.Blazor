namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeNumericFieldSwitch : TeEditorBase
{
    [Parameter] public ITeNumericFieldOption NumericFieldOption { get; set; } = default!;
    [Parameter] public object NumericDefaultValue { get; set; } = default!;
}
