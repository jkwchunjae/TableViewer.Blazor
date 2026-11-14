using System.Diagnostics;
using Aprillz.MewUI;
using Aprillz.MewUI.Controls;
using Aprillz.MewUI.Rendering;

namespace TableViewer.MewUI.Internal.ViewerModel;

/// <summary>Native Material-like press feedback, controlled by the source Ripple option.</summary>
internal sealed class NativeActionButton : Button
{
    private CancellationTokenSource? _ripple;
    private Point _origin;
    private double _progress = 1;
    public bool Ripple { get; set; } = true;

    protected override void OnMouseDown(MouseEventArgs args)
    {
        base.OnMouseDown(args);
        if (!Ripple || !IsEffectivelyEnabled || args.Button != MouseButton.Left) return;
        _ripple?.Cancel(); _ripple?.Dispose();
        _ripple = new();
        _origin = args.GetPosition(this);
        Animate(_ripple.Token);
    }

    private async void Animate(CancellationToken cancellationToken)
    {
        try
        {
            var clock = Stopwatch.StartNew();
            do
            {
                _progress = Math.Clamp(clock.Elapsed.TotalMilliseconds / 300, 0, 1);
                InvalidateVisual();
                await Task.Delay(16, cancellationToken);
            } while (_progress < 1);
        }
        catch (OperationCanceledException) { }
    }

    protected override void OnRender(IGraphicsContext context)
    {
        base.OnRender(context);
        if (!Ripple || _progress >= 1) return;
        var radius = Math.Sqrt(Bounds.Width * Bounds.Width + Bounds.Height * Bounds.Height) * _progress;
        var opacity = (byte)(45 * (1 - _progress));
        context.Save();
        context.IntersectClip(Bounds);
        context.FillEllipse(new Rect(Bounds.X + _origin.X - radius, Bounds.Y + _origin.Y - radius, radius * 2, radius * 2), Foreground.WithAlpha(opacity));
        context.Restore();
    }

    protected override void OnDispose()
    {
        _ripple?.Cancel(); _ripple?.Dispose();
        base.OnDispose();
    }
}
