using Aprillz.MewUI;
using Aprillz.MewUI.Controls;
using TableViewer.MewUI.Internal.ViewerModel;
using Xunit;

namespace TableViewer.MewUI.Tests;

[Collection(NativeControlCollection.Name)]
public class ViewerLayoutTests
{
    [Fact]
    public void NativeTableSharesColumnWidthsAcrossRowsAndSumsRowHeights()
    {
        using var table = new NativeTable(2);
        var firstName = new Border().Width(30).Height(10);
        var firstValue = new Border().Width(80).Height(20);
        var secondName = new Border().Width(60).Height(15);
        var secondValue = new Border().Width(50).Height(35);
        var row1 = new Border();
        var row2 = new Border();
        table.AddRow(row1, [firstName, firstValue]);
        table.AddRow(row2, [secondName, secondValue]);
        table.Measure(new Aprillz.MewUI.Size(200, 400));
        table.Arrange(new Rect(10, 25, 200, table.DesiredSize.Height));
        Assert.Equal(200, table.DesiredSize.Width);
        Assert.Equal(55, table.DesiredSize.Height);
        Assert.Equal(25, row1.Bounds.Y);
        Assert.Equal(45, row2.Bounds.Y);
        Assert.Equal(firstValue.Bounds.X, secondValue.Bounds.X);
        Assert.Equal(70, firstValue.Bounds.X);
    }

    [Fact]
    public void RippleFlagReachesTheNativeActionButton()
    {
        using var button = new NativeActionButton();
        NativeStyle.Button(button, new() { Ripple = false });
        Assert.False(button.Ripple);
        NativeStyle.Button(button, new() { Ripple = true });
        Assert.True(button.Ripple);
    }
}
