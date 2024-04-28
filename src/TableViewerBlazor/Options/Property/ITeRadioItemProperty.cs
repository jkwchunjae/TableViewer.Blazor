namespace TableViewerBlazor.Options.Property;

// https://mudblazor.com/api/radio#properties
public interface ITeRadioItemProperty
{
    bool Disabled { get; }
    Placement Placement { get; }
    Color Color { get; }
    bool Dense { get; }    
    bool DisableRipple { get; }
    Size? Size { get; }
    Color? UnCheckedColor { get; }
}

public class TeRadioItemProperty : ITeRadioItemProperty
{
    public bool Disabled { get; set; } = false;
    public Placement Placement { get; set; } = Placement.End;
    public Color Color { get; set; } = Color.Default;
    public bool Dense { get; set; } = true;
    public bool DisableRipple { get; set; }
    public Size? Size { get; set; } = MudBlazor.Size.Medium;
    public Color? UnCheckedColor { get; set; }
}