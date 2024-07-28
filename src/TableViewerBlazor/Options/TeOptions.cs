namespace TableViewerBlazor.Options;

public class TeOptions
{
    public bool ReadProperty { get; set; } = true;
    public bool ReadField { get; set; } = true;
    public TeStyleOption Style { get; set; } = new();
    public List<ITeTextFieldOption> TextFieldOptions { get; set; } = [];
    public List<ITeNumericFieldOption> NumericFieldOptions { get; set; } = [];
    public List<ITeSelectBoxOption> SelectBoxOptions { get; set; } = [];
    public List<ITeRadioOption> RadioOptions { get; set; } = [];
    public List<TeIgnoreOption> IgnoreOptions { get; set; } = [];
}

