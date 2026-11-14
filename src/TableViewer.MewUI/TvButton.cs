using Aprillz.MewUI.Controls;
using TableViewer.MewUI.Options;

namespace TableViewer.MewUI;

/// <summary>Shared viewer/editor action control with typed conditions, confirmation and progress.</summary>
public sealed class TvButton : UserControl
{
    private readonly TableViewer _renderer;
    public object? Item { get; }
    public ITvButton Button { get; }
    public int Depth { get; }
    public Func<string, string, Task>? LinkRequested { get => _renderer.LinkRequested; set => _renderer.LinkRequested = value; }
    public event Action<Exception>? Error { add => _renderer.Error += value; remove => _renderer.Error -= value; }

    public TvButton(object? item, ITvButton button, int depth = 1)
    {
        Item = item; Button = button; Depth = depth;
        _renderer = new TableViewer { ActionOwner = this };
    }
    protected override Element? OnBuild() => _renderer.Buttons(Item, [Button], Depth, "$");
    protected override void OnDispose() { _renderer.Dispose(); base.OnDispose(); }
}
