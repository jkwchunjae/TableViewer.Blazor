namespace TableViewerBlazor.Options;

public class TeStyleOption
{
    public bool Dense { get; set; } = true;
    public bool SuperDense { get; set; } = true;
    public bool Hover { get; set; } = false;
    public bool Striped { get; set; } = false;
    public bool Bordered { get; set; } = true;
    public List<string> RootClass { get; set; } = [];
}
