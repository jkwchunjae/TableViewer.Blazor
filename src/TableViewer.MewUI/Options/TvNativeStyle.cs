using Aprillz.MewUI.Controls;

namespace TableViewer.MewUI.Options;

// These neutral presentation values intentionally do not depend on MudBlazor.
public enum Variant { Text, Filled, Outlined }
public enum Size { Small, Medium, Large }
public enum Color { Default, Primary, Secondary, Tertiary, Info, Success, Warning, Error, Dark, Transparent, Inherit, Surface }
public enum Placement { Top, Bottom, Left, Right, Start, End }
public enum ObjectFit { Fill, Contain, Cover, None, ScaleDown }
public enum ObjectPosition { Center, Top, Bottom, Left, Right, TopLeft, TopRight, BottomLeft, BottomRight }
public enum MaxWidth { ExtraSmall, Small, Medium, Large, ExtraLarge, ExtraExtraLarge, False }

/// <summary>Native confirmation-window options corresponding to the source popup configuration.</summary>
public sealed class DialogOptions
{
    public bool FullWidth { get; set; } = true;
    public bool CloseOnEscapeKey { get; set; } = true;
    public bool NoHeader { get; set; }
    public bool CloseButton { get; set; } = true;
    public bool BackdropClick { get; set; } = true;
    public bool FullScreen { get; set; }
    public MaxWidth? MaxWidth { get; set; }
    public double Width { get; set; } = 480;
    public double Height { get; set; } = 260;
    public Action<Window>? Configure { get; set; }
}
