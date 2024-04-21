namespace TableViewerBlazor.Options;

public class TeOptions
{
    public bool ReadProperty { get; set; } = true;
    public bool ReadField { get; set; } = true;
    public IEnumerable<ITeSelectBoxOption> SelectBoxOptions { get; set; } = new List<ITeSelectBoxOption>();
}

