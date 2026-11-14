using Aprillz.MewUI.Rendering;
using TableViewer.MewUI.Internal.ViewerModel;
using TableViewer.MewUI.Options.Property;

namespace TableViewer.MewUI.Internal.Editing;

/// <summary>Native select popup with bounded height, placement, keyboard navigation and source list styling.</summary>
internal sealed class TeSelectControl : DropDownBase
{
    private readonly TeSelectBoxProperty _property;
    private readonly ITeSelectItem[] _items;
    private ListBox? _list;
    private int _selectedIndex = -1;
    public event Action<ITeSelectItem?>? SelectionChanged;
    public int SelectedIndex
    {
        get => _selectedIndex;
        set { _selectedIndex = value >= 0 && value < _items.Length ? value : -1; InvalidateVisual(); }
    }
    public ITeSelectItem? SelectedItem => _selectedIndex >= 0 ? _items[_selectedIndex] : null;

    public TeSelectControl(ITeSelectItem[] items, TeSelectBoxProperty property) { _items = items; _property = property; ArrowForeground = Aprillz.MewUI.Color.Transparent; }

    protected override Aprillz.MewUI.Size MeasureHeader(Aprillz.MewUI.Size availableSize) => new(Math.Min(availableSize.Width, 200), 32);
    protected override void RenderHeaderContent(IGraphicsContext context, Rect headerBounds, Rect textBounds) =>
        DrawEngineText(context, SelectedItem?.Text ?? _property.Placeholder ?? "", textBounds, Foreground,
            TextAlignment.Left, TextAlignment.Center, TextWrapping.NoWrap, TextTrimming.CharacterEllipsis, null, false);

    protected override UIElement CreatePopupContent()
    {
        _list = new ListBox().Items(_items, item => item.Text).SelectedIndex(SelectedIndex).ItemHeight(_property.Dense ? 26 : 34);
        _list.Height = Math.Max(30, Math.Min(Math.Max(30, _property.MaxHeight), _items.Length * _list.ItemHeight));
        _list.OnMouseUp(args => { if (args.Button == MouseButton.Left) Accept(); });
        _list.OnKeyDown(args => { if (args.Key == Key.Enter) { Accept(); args.Handled = true; } });
        NativeStyle.ApplyClasses(_list, (_property.ListClass ?? "").Split(' ', StringSplitOptions.RemoveEmptyEntries));
        var border = new Border().Padding(3).Child(_list).BorderThickness(1)
            .WithTheme((theme, body) => { body.Background = theme.Palette.ControlBackground; body.BorderBrush = theme.Palette.ControlBorder; });
        NativeStyle.ApplyClasses(border, (_property.PopoverClass ?? "").Split(' ', StringSplitOptions.RemoveEmptyEntries));
        return border;
    }

    private void Accept()
    {
        if (_list is null) return;
        SelectedIndex = _list.SelectedIndex;
        IsDropDownOpen = false;
        SelectionChanged?.Invoke(SelectedItem);
        Focus();
    }
    protected override UIElement GetPopupFocusTarget(UIElement content) => _list ?? content;

    protected override Rect CalculatePopupBounds(Window window, UIElement content)
    {
        var anchor = TransformToAncestor(window).TransformBounds(new Rect(0, 0, ActualWidth, ActualHeight));
        var height = Math.Max(36, Math.Min(Math.Max(30, _property.MaxHeight), _items.Length * (_property.Dense ? 26 : 34) + 8));
        var above = _property.AnchorOrigin is (Origin.TopLeft or Origin.TopCenter or Origin.TopRight)
            && _property.TransformOrigin is (Origin.BottomLeft or Origin.BottomCenter or Origin.BottomRight);
        var y = above ? anchor.Y - height : anchor.Bottom;
        if (y + height > window.ActualHeight) y = Math.Max(0, anchor.Y - height);
        return new Rect(Math.Max(0, anchor.X), Math.Max(0, y), Math.Max(120, anchor.Width), height);
    }

    protected override void OnKeyDown(KeyEventArgs args)
    {
        if (_items.Length > 0 && !IsDropDownOpen && args.Key is (Key.Down or Key.Up) && !args.AltKey)
        {
            SelectedIndex = Math.Clamp(SelectedIndex + (args.Key == Key.Down ? 1 : -1), 0, _items.Length - 1);
            SelectionChanged?.Invoke(SelectedItem);
            args.Handled = true;
            return;
        }
        base.OnKeyDown(args);
    }
}
