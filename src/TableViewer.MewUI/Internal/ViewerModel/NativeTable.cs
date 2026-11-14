using Aprillz.MewUI;
using Aprillz.MewUI.Controls;
using MewSize = Aprillz.MewUI.Size;

namespace TableViewer.MewUI.Internal.ViewerModel;

/// <summary>Coordinates native row panels so recursive cells share column widths and row hover state.</summary>
internal sealed class NativeTable : Panel
{
    private readonly List<(Border Surface, Grid Grid, IReadOnlyList<UIElement> Cells)> _rows = [];
    private readonly int _columns;

    public NativeTable(int columns) => _columns = columns;

    public void AddRow(Border surface, IReadOnlyList<UIElement> cells)
    {
        var grid = new Grid().Columns(string.Join(',', Enumerable.Repeat("Auto", _columns))).Rows("Auto");
        for (var index = 0; index < cells.Count; index++) grid.Children(cells[index].Column(index));
        surface.Child = grid;
        _rows.Add((surface, grid, cells));
        Add(surface);
    }

    protected override MewSize MeasureOverride(MewSize availableSize)
    {
        var widths = new double[_columns];
        foreach (var row in _rows)
            for (var column = 0; column < row.Cells.Count; column++)
            {
                row.Cells[column].Measure(new MewSize(double.PositiveInfinity, double.PositiveInfinity));
                widths[column] = Math.Max(widths[column], row.Cells[column].DesiredSize.Width);
            }
        var totalWidth = widths.Sum();
        if (double.IsFinite(availableSize.Width) && availableSize.Width > totalWidth)
        {
            widths[^1] += availableSize.Width - totalWidth;
            totalWidth = availableSize.Width;
        }
        var height = 0.0;
        foreach (var row in _rows)
        {
            for (var column = 0; column < widths.Length; column++)
            {
                var width = GridLength.Pixels(widths[column]);
                if (!row.Grid.ColumnDefinitions[column].Width.Equals(width)) row.Grid.ColumnDefinitions[column].Width = width;
            }
            row.Surface.Measure(new MewSize(totalWidth, double.PositiveInfinity));
            height += row.Surface.DesiredSize.Height;
        }
        return new MewSize(totalWidth, height);
    }

    protected override MewSize ArrangeOverride(MewSize finalSize)
    {
        var top = Bounds.Y;
        foreach (var row in _rows)
        {
            var height = row.Surface.DesiredSize.Height;
            row.Surface.Arrange(new Rect(Bounds.X, top, finalSize.Width, height));
            top += height;
        }
        return finalSize;
    }
}
