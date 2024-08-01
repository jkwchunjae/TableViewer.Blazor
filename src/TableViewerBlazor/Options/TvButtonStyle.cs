namespace TableViewerBlazor.Options;

public class TvButtonStyle
{
    public Variant Variant { get; set; } = Variant.Outlined;
    public Size Size { get; set; } = Size.Small;
    public Color Color { get; set; } = Color.Default;
    public string Style { get; set; } = "text-transform: none;";
    public string? StartIcon { get; set; } = null;
    public string? EndIcon { get; set; } = null;
    public Size IconSize { get; set; } = Size.Small;
    public Color IconColor { get; set; } = Color.Default;
    public bool Ripple { get; set; } = true;
    public bool DropShadow { get; set; } = true;
    public bool Dense { get; set; } = true;
    public bool SuperDense { get; set; } = false;
}
