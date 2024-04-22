namespace TableViewerBlazor.Options;

public class TeOptions
{
    public bool ReadProperty { get; set; } = true;
    public bool ReadField { get; set; } = true;
    public IEnumerable<ITeTextFieldOption> TextFieldOptions { get; set; } = new List<ITeTextFieldOption>();
    public IEnumerable<ITeSelectBoxOption> SelectBoxOptions { get; set; } = new List<ITeSelectBoxOption>();
    public IEnumerable<ITeRadioOption> RadioOptions { get; set; } = new List<ITeRadioOption>();
}

