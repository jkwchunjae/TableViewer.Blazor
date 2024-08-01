namespace TableViewerBlazor.Options;

public class TeOptions
{
    public string? Title { get; set; }
    public bool ReadProperty { get; set; } = true;
    public bool ReadField { get; set; } = true;
    public TeStyleOption Style { get; set; } = new();
    public List<ITeTextFieldOption> TextFieldOptions { get; set; } = [];
    public List<ITeNumericFieldOption> NumericFieldOptions { get; set; } = [];
    public List<ITeSelectBoxOption> SelectBoxOptions { get; set; } = [];
    public List<ITeRadioOption> RadioOptions { get; set; } = [];
    public List<ITeCheckBoxOption> CheckBoxOptions { get; set; } = [];
    public List<TeIgnoreOption> IgnoreOptions { get; set; } = [];
    public TeCustomEditorOptionGroup CustomEditorOptions { get; set; } = new();
    public List<TeObjectArrayEditorOption> ObjectArrayEditorOptions { get; set; } = [new()];
    public List<TeArrayOption> ArrayOptions { get; set; } = []; 
    public List<ITvButton> ToolbarButtons { get; set; } = [];
}

