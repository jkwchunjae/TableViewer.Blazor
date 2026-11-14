using System.Globalization;
using System.Xml.Linq;
using Aprillz.MewUI;
using Aprillz.MewUI.Controls;
using TableViewer.MewUI.Options;
using MewColor = Aprillz.MewUI.Color;
using TvColor = TableViewer.MewUI.Options.Color;

namespace TableViewer.MewUI.Internal.ViewerModel;

/// <summary>Maps source appearance options to native properties and native style callbacks.</summary>
public static class NativeStyle
{
    /// <summary>Raised for CSS declarations that need an application-specific native Configure callback.</summary>
    public static event Action<string>? UnmappedCss;

    public static Button Button(Button button, TvButtonStyle style)
    {
        if (button is NativeActionButton feedback) feedback.Ripple = style.Ripple;
        var padding = style.SuperDense ? new Thickness(4, 1) : style.Dense ? new Thickness(8, 3) : new Thickness(12, 7);
        button.Padding = padding;
        button.FontSize = style.Size switch { Options.Size.Small => 12, Options.Size.Large => 16, _ => 14 };
        button.WithTheme((theme, control) =>
        {
            var accent = ThemeColor(style.Color, theme.Palette);
            var neutral = style.Color is TvColor.Default or TvColor.Inherit;
            control.Background = style.Variant == Variant.Filled ? (neutral ? theme.Palette.ButtonFace : accent) : MewColor.Transparent;
            control.Foreground = style.Variant == Variant.Filled ? (neutral ? theme.Palette.WindowText : theme.Palette.AccentText) : accent;
            control.BorderThickness = style.Variant == Variant.Outlined ? 1 : 0;
            control.BorderBrush = accent;
        });
        ApplyCss(button, style.Style);
        style.Configure?.Invoke(button);
        return button;
    }

    public static Element ButtonChrome(Button button, TvButtonStyle style) => style.DropShadow && style.Variant == Variant.Filled
        ? new ShadowDecorator { BlurRadius = 3, OffsetY = 1, ShadowColor = MewColor.FromArgb(45, 0, 0, 0), Child = button }
        : button;

    public static Element ButtonContent(string label, TvButtonStyle style)
    {
        var panel = new StackPanel().Horizontal().Spacing(4);
        if (!string.IsNullOrWhiteSpace(style.StartIcon)) panel.Children(Icon(style.StartIcon, style.IconSize, style.IconColor));
        if (!string.IsNullOrWhiteSpace(label)) panel.Children(new TextBlock().Text(label).CenterVertical());
        if (!string.IsNullOrWhiteSpace(style.EndIcon)) panel.Children(Icon(style.EndIcon, style.IconSize, style.IconColor));
        return panel;
    }

    private static Element Icon(string value, Options.Size size, TvColor color)
    {
        var length = size switch { Options.Size.Small => 16, Options.Size.Large => 26, _ => 20 };
        string? path = null;
        if (value.TrimStart().StartsWith('<'))
        {
            try
            {
                var xml = XElement.Parse("<svg>" + value + "</svg>");
                path = string.Join(" ", xml.Descendants().Where(element => element.Name.LocalName == "path").Select(element => (string?)element.Attribute("d")));
            }
            catch (System.Xml.XmlException) { }
        }
        else if (value.TrimStart().StartsWith('M') || value.TrimStart().StartsWith('m')) path = value;
        if (!string.IsNullOrWhiteSpace(path))
            return new PathShape().Data(path).Stretch(Stretch.Uniform).Size(length).CenterVertical()
                .WithTheme((theme, shape) => shape.Fill(ThemeColor(color, theme.Palette)));
        return new TextBlock().Text(value).FontSize(length).CenterVertical();
    }

    public static MewColor ThemeColor(TvColor color, Palette palette) => color switch
    {
        TvColor.Primary or TvColor.Secondary or TvColor.Tertiary => palette.Accent,
        TvColor.Error => palette.Error,
        TvColor.Success => MewColor.FromRgb(46, 125, 50),
        TvColor.Warning => MewColor.FromRgb(237, 108, 2),
        TvColor.Info => MewColor.FromRgb(2, 136, 209),
        TvColor.Transparent => MewColor.Transparent,
        TvColor.Surface => palette.ContainerBackground,
        _ => palette.WindowText
    };

    public static T ApplyClassAndStyle<T>(T element, TvClassAndStyle? style) where T : Element
    {
        if (style is null) return element;
        ApplyClasses(element, style.Class);
        foreach (var declaration in style.Style) ApplyCss(element, declaration);
        style.Configure?.Invoke(element);
        return element;
    }

    public static void ApplyClasses(Element element, IEnumerable<string> classes)
    {
        foreach (var token in classes.SelectMany(value => value.Split(' ', StringSplitOptions.RemoveEmptyEntries)))
        {
            var parts = token.Split('-');
            if (parts.Length == 2 && double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var scale)
                && parts[0] is "m" or "mx" or "my" or "mt" or "mb" or "ml" or "mr" or "ms" or "me" or "p" or "px" or "py" or "pt" or "pb" or "pl" or "pr" or "ps" or "pe")
            {
                var isPadding = parts[0][0] == 'p';
                var old = element is FrameworkElement framework ? framework.Margin : default;
                if (isPadding && element is Control control) old = control.Padding;
                var amount = scale * 4;
                var side = parts[0].Length > 1 ? parts[0][1] : 'a';
                var thickness = new Thickness(side is 'a' or 'x' or 'l' or 's' ? amount : old.Left,
                    side is 'a' or 'y' or 't' ? amount : old.Top,
                    side is 'a' or 'x' or 'r' or 'e' ? amount : old.Right,
                    side is 'a' or 'y' or 'b' ? amount : old.Bottom);
                if (isPadding && element is Control paddingControl) paddingControl.Padding = thickness;
                else if (!isPadding && element is FrameworkElement marginElement) marginElement.Margin = thickness;
            }
            else if (token == "font-weight-bold") SetWeight(element, FontWeight.Bold);
            else if (token == "d-none" && element is UIElement ui) ui.IsVisible = false;
            else if (token.StartsWith("text-", StringComparison.Ordinal) && element is TextBlock text)
                text.TextAlignment = token switch { "text-center" => TextAlignment.Center, "text-right" or "text-end" => TextAlignment.Right, _ => TextAlignment.Left };
            else if (element is Control styled && !token.StartsWith("d-", StringComparison.Ordinal)) styled.StyleName = token;
        }
    }

    public static void ApplyCss(Element element, string? css)
    {
        if (string.IsNullOrWhiteSpace(css)) return;
        foreach (var declaration in css.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var split = declaration.IndexOf(':');
            if (split < 0) { UnmappedCss?.Invoke(declaration); continue; }
            var name = declaration[..split].Trim().ToLowerInvariant();
            var value = declaration[(split + 1)..].Trim();
            var number = Number(value);
            switch (name)
            {
                case "font-size" when number.HasValue: SetSize(element, number.Value); break;
                case "font-weight": SetWeight(element, value is "bold" or "bolder" or "600" or "700" or "800" or "900" ? FontWeight.Bold : FontWeight.Normal); break;
                case "font-family" when element is Control font: font.FontFamily = value.Trim('\'', '"'); break;
                case "color" when TryColor(value, out var foreground): SetForeground(element, foreground); break;
                case "background" or "background-color" when element is Control background && TryColor(value, out var fill): background.Background = fill; break;
                case "border-color" when element is Control border && TryColor(value, out var stroke): border.BorderBrush = stroke; break;
                case "border-width" when element is Control border && number.HasValue: border.BorderThickness = number.Value; break;
                case "border-radius" when element is Control rounded && number.HasValue: rounded.CornerRadius = number.Value; break;
                case "padding" when element is Control padding && TryThickness(value, out var inset): padding.Padding = inset; break;
                case "margin" when element is FrameworkElement margin && TryThickness(value, out var outside): margin.Margin = outside; break;
                case "width" when element is FrameworkElement width && number.HasValue: width.Width = number.Value; break;
                case "height" when element is FrameworkElement height && number.HasValue: height.Height = number.Value; break;
                case "min-width" when element is FrameworkElement width && number.HasValue: width.MinWidth = number.Value; break;
                case "max-width" when element is FrameworkElement width && number.HasValue: width.MaxWidth = number.Value; break;
                case "min-height" when element is FrameworkElement height && number.HasValue: height.MinHeight = number.Value; break;
                case "max-height" when element is FrameworkElement height && number.HasValue: height.MaxHeight = number.Value; break;
                case "opacity" when element is UIElement opacity && number.HasValue: opacity.Opacity = Math.Clamp(number.Value, 0, 1); break;
                case "display" when value == "none" && element is UIElement hidden: hidden.IsVisible = false; break;
                case "gap" when element is StackPanel stack && number.HasValue: stack.Spacing = number.Value; break;
                case "text-align" when element is TextBlock text: text.TextAlignment = value switch { "center" => TextAlignment.Center, "right" or "end" => TextAlignment.Right, _ => TextAlignment.Left }; break;
                case "text-transform" when value == "none": break; // Native text is already case-preserving.
                default: UnmappedCss?.Invoke(declaration); break;
            }
        }
    }

    private static double? Number(string value)
    {
        var units = value.EndsWith("rem", StringComparison.Ordinal) ? 14 : value.EndsWith("em", StringComparison.Ordinal) ? 14 : value.EndsWith("pt", StringComparison.Ordinal) ? 96.0 / 72 : 1;
        var text = value.TrimEnd('p', 'x', 't', 'e', 'm', 'r');
        return double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var number) ? number * units : null;
    }

    private static bool TryThickness(string value, out Thickness thickness)
    {
        var values = value.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(Number).ToArray();
        thickness = default;
        if (values.Length is < 1 or > 4 || values.Any(value => !value.HasValue)) return false;
        var top = values[0]!.Value;
        var right = values.Length > 1 ? values[1]!.Value : top;
        var bottom = values.Length > 2 ? values[2]!.Value : top;
        var left = values.Length > 3 ? values[3]!.Value : right;
        thickness = new Thickness(left, top, right, bottom);
        return true;
    }

    private static bool TryColor(string value, out MewColor color)
    {
        try
        {
            color = value.ToLowerInvariant() switch
            {
                "transparent" => MewColor.Transparent,
                "white" => MewColor.FromRgb(255, 255, 255),
                "black" => MewColor.FromRgb(0, 0, 0),
                "red" => MewColor.FromRgb(255, 0, 0),
                "green" => MewColor.FromRgb(0, 128, 0),
                "blue" => MewColor.FromRgb(0, 0, 255),
                "gray" or "grey" => MewColor.FromRgb(128, 128, 128),
                _ => MewColor.FromHex(value)
            };
            return true;
        }
        catch (Exception exception) when (exception is FormatException or ArgumentException)
        {
            color = default;
            return false;
        }
    }

    private static void SetSize(Element element, double value) { if (element is Control control) control.FontSize = value; else if (element is TextBlock text) text.FontSize = value; }
    private static void SetWeight(Element element, FontWeight value) { if (element is Control control) control.FontWeight = value; else if (element is TextBlock text) text.FontWeight = value; }
    private static void SetForeground(Element element, MewColor value) { if (element is Control control) control.Foreground = value; else if (element is TextBlock text) text.Foreground = value; }
}
