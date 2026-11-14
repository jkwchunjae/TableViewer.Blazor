using TableViewer.MewUI.Options;

namespace TableViewer.MewUI.Internal.Viewing;

public enum TvActionState { Ready, Armed, Running, Done }

/// <summary>Owns the same one-click/two-click lifecycle as the source action buttons.</summary>
public sealed class TvActionController(ITvAction action, object? item) : IDisposable
{
    private DateTimeOffset _armedUntil;
    private readonly CancellationTokenSource _lifetime = new();
    private bool _disposed;
    public TvActionState State { get; private set; }
    public event Action? Changed;
    public Exception? Error { get; private set; }

    public async Task InvokeAsync()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        if (State is TvActionState.Running or TvActionState.Done) return;
        if (action is ITvDoubleClickAction doubleClick && (State != TvActionState.Armed || DateTimeOffset.UtcNow >= _armedUntil))
        {
            _armedUntil = DateTimeOffset.UtcNow + doubleClick.ResetDelay;
            SetState(TvActionState.Armed);
            _ = ResetAsync(_armedUntil);
            return;
        }
        SetState(TvActionState.Running);
        Error = null;
        try
        {
            await action.Action(item);
            SetState(action is ITvOneClickAction { Reusable: false } ? TvActionState.Done : TvActionState.Ready);
        }
        catch (Exception exception)
        {
            Error = exception;
            SetState(TvActionState.Ready);
            throw;
        }
    }

    private async Task ResetAsync(DateTimeOffset deadline)
    {
        try
        {
            var delay = deadline - DateTimeOffset.UtcNow;
            if (delay > TimeSpan.Zero) await Task.Delay(delay, _lifetime.Token);
            if (_armedUntil == deadline && State == TvActionState.Armed) SetState(TvActionState.Ready);
        }
        catch (OperationCanceledException) { }
    }
    private void SetState(TvActionState state) { if (!_disposed) { State = state; Changed?.Invoke(); } }
    public void Dispose() { if (_disposed) return; _disposed = true; _lifetime.Cancel(); _lifetime.Dispose(); Changed = null; }
}
