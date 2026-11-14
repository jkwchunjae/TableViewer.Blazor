using Aprillz.MewUI;
using Aprillz.MewUI.Controls;
using TableViewer.MewUI.Internal.ViewerModel;
using TableViewer.MewUI.Options;
using Xunit;

namespace TableViewer.MewUI.Tests;

public class ViewerStyleTests
{
    [Fact]
    public void CssAppearanceAndNativeCallbacksApplyToTheSameElement()
    {
        var label = new TextBlock().Text("Title");
        var style = new TvClassAndStyle
        {
            Class = ["mb-3"],
            Style = ["font-size: 17px; font-weight: bold; color: #123456;"],
            Configure = element => ((TextBlock)element).Text = "Native title"
        };
        Assert.Same(label, NativeStyle.ApplyClassAndStyle(label, style));
        Assert.Equal("Native title", label.Text);
        Assert.Equal(17, label.FontSize);
        Assert.Equal(FontWeight.Bold, label.FontWeight);
        Assert.Equal(12, label.Margin.Bottom);
        Assert.Equal(Aprillz.MewUI.Color.FromRgb(0x12, 0x34, 0x56), label.Foreground);
    }

    [Fact]
    public void CssBoxModelUsesCssOrderAndSupportsIndependentSpacingClasses()
    {
        var button = new Button();
        NativeStyle.ApplyCss(button, "padding: 1px 2px 3px 4px; margin: 4px 8px; width: 120px; min-height: 24px; opacity: .5;");
        Assert.Equal(new Thickness(4, 1, 2, 3), button.Padding);
        Assert.Equal(new Thickness(8, 4, 8, 4), button.Margin);
        Assert.Equal(120, button.Width);
        Assert.Equal(24, button.MinHeight);
        Assert.Equal(.5, button.Opacity);
        NativeStyle.ApplyClasses(button, ["px-2", "py-1", "mt-3"]);
        Assert.Equal(new Thickness(8, 4, 8, 4), button.Padding);
        Assert.Equal(new Thickness(8, 12, 8, 4), button.Margin);
    }

    [Fact]
    public void UnsupportedCssIsReportedForExplicitNativeMigration()
    {
        var unsupported = new List<string>();
        void Capture(string css) => unsupported.Add(css);
        NativeStyle.UnmappedCss += Capture;
        try
        {
            NativeStyle.ApplyCss(new TextBlock(), "position: fixed; font-size: 12px;");
            Assert.Equal(["position: fixed"], unsupported);
        }
        finally { NativeStyle.UnmappedCss -= Capture; }
    }
}
