using TableViewer.MewUI.Internal.Editing;

namespace TableViewer.MewUI;

/// <summary>Base for native editors. Changes are applied to the source model and reported through typed callbacks.</summary>
public abstract class TableEditorBase<T> : UserControl, IDisposable
{
    private T _data = default!;
    private TeOptions _options = new();
    private TeEditSession? _session;
    private TeEditorRenderer? _renderer;
    private readonly Border _host = new();
    private readonly List<Action> _subscriptions = [];
    public T Data { get => _data; set { _data = value; if (_session is not null) _session.Load(value); } }
    public TeOptions Options { get => _options; set { _options = value ?? throw new ArgumentNullException(nameof(value)); ResetSession(); } }
    public Func<T, Task>? DataChanged { get; set; }
    public Func<bool, Task>? IsValidChanged { get; set; }
    public Func<string[], Task>? ErrorsChanged { get; set; }
    public bool IsValid => Session.IsValid;
    public string[] Errors => Session.Errors;
    public TeEditSession EditSession => Session;

    protected TableEditorBase() { }
    protected TableEditorBase(T data, TeOptions? options = null) { _data = data; _options = options ?? new(); }

    private TeEditSession Session
    {
        get
        {
            if (_session is null)
            {
                _session = new(_data, _options, typeof(T))
                {
                    DataChanged = async value => { _data = value is T typed ? typed : default!; if (DataChanged is not null) await DataChanged(_data); },
                    IsValidChanged = valid => IsValidChanged?.Invoke(valid) ?? Task.CompletedTask,
                    ErrorsChanged = errors => ErrorsChanged?.Invoke(errors) ?? Task.CompletedTask
                };
                _session.StructureChanged += RebuildContent;
            }
            return _session;
        }
    }

    protected override Element? OnBuild()
    {
        ClearSubscriptions();
        SetCustomEditor();
        Session.Rebuild();
        RebuildContent();
        return _host;
    }

    private void RebuildContent()
    {
        ClearSubscriptions();
        _renderer?.Dispose();
        _renderer = new TeEditorRenderer(Session);
        _host.Child = _renderer.Build();
    }

    private void ResetSession()
    {
        if (_session is null) return;
        if (_session is not null) _session.StructureChanged -= RebuildContent;
        _session = null;
        Refresh();
    }

    public void Refresh()
    {
        ClearSubscriptions();
        SetCustomEditor();
        Session.Rebuild();
        RebuildContent();
    }

    public Task Validate() => Session.ValidateAsync();
    protected virtual void SetCustomEditor() { }

    protected Func<ICustomEditorArgument, Element> ConvertRenderFragment<TParent, TItem>(Func<CustomEditorArgument<TParent, TItem>, Element> factory)
    {
        return context =>
        {
            if (context.Parent is not TParent parent) throw new ArgumentException($"Custom editor parent must be {typeof(TParent)}.");
            if (context.Value is not TItem && context.Value is not null) throw new ArgumentException($"Custom editor value must be {typeof(TItem)}.");
            var argument = new CustomEditorArgument<TParent, TItem>
            {
                Parent = parent,
                Value = context.Value is TItem item ? item : default,
            };
            argument.DataChanged = async value =>
            {
                await context.DataChanged(value);
                argument.Value = context.Value is TItem current ? current : default;
            };
            EventHandler<object> handler = (_, changedParent) =>
            {
                argument.Value = context.Value is TItem current ? current : default;
                argument.OnParentChanged(changedParent);
            };
            context.ParentChanged += handler;
            _subscriptions.Add(() => context.ParentChanged -= handler);
            return factory(argument);
        };
    }

    private void ClearSubscriptions() { foreach (var unsubscribe in _subscriptions) unsubscribe(); _subscriptions.Clear(); }
    protected override void OnDispose()
    {
        ClearSubscriptions();
        _renderer?.Dispose();
        if (_session is not null) _session.StructureChanged -= RebuildContent;
        base.OnDispose();
        GC.SuppressFinalize(this);
    }
}
