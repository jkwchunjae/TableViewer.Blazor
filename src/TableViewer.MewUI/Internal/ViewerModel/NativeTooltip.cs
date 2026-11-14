using Aprillz.MewUI;
using Aprillz.MewUI.Controls;
using TableViewer.MewUI.Options;
using TableViewer.MewUI.Options.Property;

namespace TableViewer.MewUI.Internal.ViewerModel;

/// <summary>A native tooltip popup supporting the source visibility, delay, placement and trigger settings.</summary>
public sealed class NativeTooltip : IDisposable
{
    private readonly FrameworkElement _target;
    private readonly TvTooltipOption _option;
    private readonly Popup _popup = new() { StaysOpen = true };
    private readonly Border _body;
    private CancellationTokenSource? _pending;
    private bool _disposed;

    public NativeTooltip(FrameworkElement target, TvTooltipOption option)
    {
        _target = target;
        _option = option;
        var settings = option.TooltipStyleProperty;
        _body = new Border().Padding(8, 5).CornerRadius(4).BorderThickness(1)
            .WithTheme((theme, body) =>
            {
                body.Background(settings.Color == Options.Color.Default ? theme.Palette.ControlBackground : NativeStyle.ThemeColor(settings.Color, theme.Palette));
                body.BorderBrush(theme.Palette.ControlBorder);
            })
            .Child(new TextBlock().Text(option.Text));
        NativeStyle.ApplyClasses(_body, settings.RootClass.Split(' ', StringSplitOptions.RemoveEmptyEntries));
        NativeStyle.ApplyCss(_body, settings.RootStyle);
        var content = new StackPanel().Spacing(0);
        if (settings.Placement is Placement.Left or Placement.Right or Placement.Start or Placement.End) content.Horizontal(); else content.Vertical();
        var arrow = new TextBlock().Text(settings.Placement switch
        {
            Placement.Top => "▼",
            Placement.Left or Placement.Start => "▶",
            Placement.Right or Placement.End => "◀",
            _ => "▲"
        }).Center();
        var arrowFirst = settings.Placement is Placement.Bottom or Placement.Right or Placement.End;
        if (settings.Arrow && arrowFirst) content.Children(arrow);
        content.Children(_body);
        if (settings.Arrow && !arrowFirst) content.Children(arrow);
        _popup.Content = content;
        _popup.Closed += PopupClosed;
        if (!settings.Disabled)
        {
            if (settings.ShowOnHover) { target.MouseEnter += Enter; target.MouseLeave += Leave; }
            if (settings.ShowOnFocus) { target.GotFocus += Enter; target.LostFocus += Leave; }
            if (settings.ShowOnClick) target.MouseUp += Click;
            if (settings.Visible) Application.Current?.Dispatcher?.BeginInvoke(() => SetVisible(true));
        }
    }

    public bool IsVisible => _popup.IsOpen;

    public void SetVisible(bool visible)
    {
        if (_disposed || _option.TooltipStyleProperty.Disabled) return;
        _pending?.Cancel();
        _pending?.Dispose();
        _pending = new();
        ChangeVisible(visible, _pending.Token);
    }

    private void Enter() => SetVisible(true);
    private void Leave() => SetVisible(false);
    private void Click(MouseEventArgs _) => SetVisible(!IsVisible);

    private async void ChangeVisible(bool visible, CancellationToken cancellationToken)
    {
        try
        {
            var settings = _option.TooltipStyleProperty;
            if (visible)
            {
                if (settings.Delay > 0) await Task.Delay(TimeSpan.FromMilliseconds(settings.Delay), cancellationToken);
                if (_disposed || cancellationToken.IsCancellationRequested || _target.FindVisualRoot() is not Window window) return;
                var anchor = _target.TransformToAncestor(window).TransformBounds(new Rect(0, 0, _target.ActualWidth, _target.ActualHeight));
                var wasOpen = _popup.IsOpen;
                _popup.ShowAt(_target, anchor, settings.Placement switch
                {
                    Placement.Top => PopupAnchorSide.Above,
                    Placement.Left or Placement.Start => PopupAnchorSide.Left,
                    Placement.Right or Placement.End => PopupAnchorSide.Right,
                    _ => PopupAnchorSide.Below
                });
                if (!_popup.IsOpen) return;
                if (!wasOpen) settings.VisibleChanged?.Invoke(true);
            }
            if (settings.Duration > 0)
            {
                const int frames = 6;
                for (var frame = 0; frame <= frames; frame++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    _body.Opacity = visible ? frame / (double)frames : 1 - frame / (double)frames;
                    await Task.Delay(TimeSpan.FromMilliseconds(settings.Duration / frames), cancellationToken);
                }
            }
            if (!visible) _popup.Close();
        }
        catch (OperationCanceledException) { }
    }

    private void PopupClosed(object? sender, PopupClosedEventArgs args) => _option.TooltipStyleProperty.VisibleChanged?.Invoke(false);

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        _pending?.Cancel();
        _pending?.Dispose();
        _target.MouseEnter -= Enter;
        _target.MouseLeave -= Leave;
        _target.GotFocus -= Enter;
        _target.LostFocus -= Leave;
        _target.MouseUp -= Click;
        _popup.Close();
        _popup.Closed -= PopupClosed;
    }
}
