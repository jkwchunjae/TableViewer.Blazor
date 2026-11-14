using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using Aprillz.MewUI;
using Aprillz.MewUI.Controls;
using Aprillz.MewUI.MewvalonEdit;
using Aprillz.MewUI.MewvalonEdit.Highlighting;
using TableViewer.MewUI.Internal.ViewerModel;
using TableViewer.MewUI.Internal.Viewing;
using TableViewer.MewUI.Options;
using TableViewer.MewUI.Service;
using NativeColor = Aprillz.MewUI.Color;
using TvColor = TableViewer.MewUI.Options.Color;
using TvSize = TableViewer.MewUI.Options.Size;
using TvMaxWidth = TableViewer.MewUI.Options.MaxWidth;

namespace TableViewer.MewUI;

/// <summary>Native port of the recursive TableViewer.MudBlazor component.</summary>
public sealed class TableViewer : UserControl, IDisposable
{
    public static readonly MewProperty<object?> DataProperty = MewProperty<object?>.Register<TableViewer>(
        nameof(Data), null, MewPropertyOptions.AffectsLayout, static (view, _, _) => view.ResetData());
    private readonly Border _host = new();
    private readonly Dictionary<string, bool> _open = [];
    private readonly Dictionary<string, int> _visible = [];
    private readonly Dictionary<(string Path, ITvAction Action), TvActionController> _actions = [];
    private readonly List<Action> _unsubscriptions = [];
    private readonly List<ImageSource> _images = [];
    private readonly List<Action> _closeDialogs = [];
    private CancellationTokenSource _lifetime = new();
    private bool _built;
    private bool _disposed;
    private bool _loading;
    private TvOptions _options = new();

    public object? Data { get => GetValue(DataProperty); set => SetValue(DataProperty, value); }
    public TvOptions Options { get => _options; set { _options = value ?? new(); Refresh(); } }
    public bool Loading { get => _loading; set { _loading = value; Refresh(); } }
    public DateTimeService DateTimeService { get; set; } = DateTimeService.Default;
    public Func<string, string, Task>? LinkRequested { get; set; }
    public event Action<Exception>? Error;
    internal Element? ActionOwner { get; set; }

    public TableViewer(object? data = null, TvOptions? options = null)
    {
        _options = options ?? new();
        Data = data;
    }

    protected override Element? OnBuild()
    {
        _built = true;
        Refresh();
        return _host;
    }

    /// <summary>Re-read the current object after an external mutation or option update, on the UI thread.</summary>
    public void Refresh()
    {
        if (!_built || _disposed) return;
        if (_host.Child is IDisposable previous) { _host.Child = null; previous.Dispose(); }
        ClearVisualResources();
        try
        {
            var body = Render(null, Data, null, 1, Options.GlobalOpenDepth, "$", new(ReferenceEqualityComparer.Instance));
            NativeStyle.ApplyClasses(body, Options.Style.RootClass);
            Options.Style.ConfigureRoot?.Invoke(body);
            var progress = new ProgressBar().Height(3).IsIndeterminate()
                .WithTheme((theme, bar) => bar.Foreground(NativeStyle.ThemeColor(Options.Style.LoadingProgressColor, theme.Palette)));
            _host.Child = Loading
                ? new StackPanel().Vertical().Spacing(6).Children(
                    progress,
                    new TextBlock().Text(Options.Contents.LoadingContent), body)
                : body;
        }
        catch (Exception exception) { _host.Child = Failure(exception); }
    }

    private void ResetData()
    {
        _open.Clear(); _visible.Clear();
        foreach (var controller in _actions.Values) controller.Dispose();
        _actions.Clear();
        Refresh();
    }

    private UIElement Render(object? parent, object? data, MemberInfo? member, int depth, int inheritedOpenDepth, string path, HashSet<object> ancestors)
    {
        if (data is ViewerReadError readError) return new TextBlock().Text($"Error: {readError.Message}").Foreground(NativeColor.Red);
        var classification = ViewerData.Classify(parent, data, member, depth, path, Options);
        switch (classification.Kind)
        {
            case ViewerValueKind.Null: return new TextBlock { Text = "null", FontStyle = FontStyle.Italic, Foreground = NativeColor.Green };
            case ViewerValueKind.Boolean: return (bool)data! ? new TextBlock().Text("TRUE").Bold() : new TextBlock { Text = "FALSE", Opacity = .55 };
            case ViewerValueKind.DateTime:
                return Text(data is DateTimeOffset offset ? DateTimeService.Format(offset, Options.DateTime) : DateTimeService.Format((DateTime)data!, Options.DateTime));
            case ViewerValueKind.Number:
            case ViewerValueKind.Enum: return Text(data!.ToString()!);
            case ViewerValueKind.String: return data!.ToString() is { Length: > 0 } text ? Text(text) : new TextBlock { Text = "Empty", FontStyle = FontStyle.Italic, Opacity = .55 };
            case ViewerValueKind.Serialized: return Code(data!, (ITvEditorOption)classification.CustomOption!);
            case ViewerValueKind.StringLink:
                return new Button().Content(data!.ToString()!).OnClick(async () => await OpenLink(((ITvStringLinkOption)classification.CustomOption!).Href(parent!, data!), "_self"));
            case ViewerValueKind.Image: return Image(parent!, data!, (ITvImageViewerOption)classification.CustomOption!);
        }

        // Circular references terminate at the currently open ancestor, while shared references remain browsable.
        if (!ancestors.Add(data!)) return new TextBlock { Text = "↩ Circular reference", Opacity = .6 };
        if (depth > 128) { ancestors.Remove(data!); return Text("Maximum nesting depth reached"); }
        try
        {
            var openDepth = ViewerData.ResolveOpenDepth(data, depth, path, inheritedOpenDepth, Options);
            if (!_open.TryGetValue(path, out var open)) _open[path] = open = depth <= openDepth;
            var items = data is IEnumerable enumerable && data is not IDictionary ? ViewerData.Items(enumerable) : null;
            var members = classification.Kind == ViewerValueKind.Object ? ViewerData.Members(data!, Options) : null;
            var entries = data is IDictionary dictionary ? ViewerData.Entries(dictionary, Options) : null;
            var count = entries?.Count ?? items?.Count ?? members?.Count ?? 0;
            var name = classification.Kind is ViewerValueKind.Array or ViewerValueKind.ObjectArray ? "array" : "object";
            var unit = classification.Kind is ViewerValueKind.Array or ViewerValueKind.ObjectArray ? "items" : "keys";
            if (!open) return new Button().Content($"{name} ({count} {unit})").OnClick(() => { _open[path] = true; Refresh(); });

            var panel = new StackPanel().Vertical().Spacing(0);
            if (depth == 1 || Options.Style.VisibleInnerToolbar)
            {
                var toolbar = new StackPanel().Horizontal().Spacing(6);
                if (depth == 1 && !string.IsNullOrWhiteSpace(Options.Title)) toolbar.Children(NativeStyle.ApplyClassAndStyle(Text(Options.Title!).Bold(), Options.Style.TitleStyle));
                if (Options.VisibleItemCount) toolbar.Children(new Button().Content($"({count} {unit})").OnClick(() => { _open[path] = false; Refresh(); }));
                if (depth == 1) toolbar.Children(Buttons(Data, Options.TitleButtons, depth, path + ".title"));
                panel.Children(toolbar);
            }

            if (count == 0) panel.Children(Text(Options.Contents.NoRecordsContent ?? ""));
            else if (classification.Kind == ViewerValueKind.Object && depth == 1 && !Options.Style.UseTable)
            {
                foreach (var property in members!)
                    panel.Children(new StackPanel().Vertical().Spacing(6).Margin(0, 6).Children(Text(property.Name).Bold(),
                        Render(data, ViewerData.GetValue(data, property), property, depth + 1, openDepth, ChildPath(path, property.Name), ancestors)));
            }
            else
            {
                var rows = new List<Element[]>();
                string[]? headers = null;
                Button? footer = null;
                if (classification.Kind == ViewerValueKind.Object)
                {
                    foreach (var property in members!)
                        rows.Add([Text(property.Name).Bold(), Render(data, ViewerData.GetValue(data, property), property, depth + 1, openDepth, ChildPath(path, property.Name), ancestors)]);
                }
                else if (entries is not null)
                {
                    var index = 0;
                    foreach (var entry in entries)
                    {
                        rows.Add([Render(data, entry.Key, null, depth + 1, openDepth, path + $"[{index}].key", ancestors),
                            Render(data, entry.Value, null, depth + 1, openDepth, path + $"[{index}].value", ancestors)]);
                        index++;
                    }
                }
                else
                {
                    var objectArray = classification.Kind == ViewerValueKind.ObjectArray && !Options.HasEditorOption(data, items!.FirstOrDefault()!, depth, path);
                    var columns = objectArray ? ViewerData.Columns(items!, Options) : [];
                    var hasButtons = items!.Any(item => Options.Buttons.Any(button => button.Condition(item, depth)));
                    headers = objectArray ? [.. columns.Select(column => column.Name), .. hasButtons ? new[] { "…" } : Array.Empty<string>()] : null;
                    var visibleCount = objectArray ? items!.Count : _visible.GetValueOrDefault(path, Math.Max(2, Options.ArrayVisibleDepth));
                    for (var index = 0; index < Math.Min(visibleCount, items!.Count); index++)
                    {
                        var item = items[index];
                        var itemPath = path + $"[{index}]";
                        var cells = new List<Element>();
                        if (objectArray)
                        {
                            foreach (var column in columns)
                                cells.Add(Render(item, ViewerData.GetValue(item, column), column, depth + 1, openDepth, ChildPath(itemPath, column.Name), ancestors));
                        }
                        else
                        {
                            cells.Add(Text(index.ToString()));
                            cells.Add(Render(data, item, null, depth + 1, openDepth, itemPath, ancestors));
                        }
                        if (hasButtons) cells.Add(Buttons(item, Options.Buttons, depth, itemPath));
                        rows.Add(cells.ToArray());
                    }
                    if (visibleCount < items.Count)
                        footer = new Button().Content(Text("more")).OnClick(() => { _visible[path] = Math.Min(items.Count, visibleCount > int.MaxValue / 2 ? int.MaxValue : visibleCount * 2); Refresh(); });
                }
                // Keep the source order: table rows, then the incremental collection footer.
                panel.Children(Table(rows, headers));
                if (footer is not null) panel.Children(footer);
            }
            return panel;
        }
        finally { ancestors.Remove(data!); }
    }

    private UIElement Table(IReadOnlyList<Element[]> rows, string[]? headers)
    {
        var columnCount = Math.Max(1, Math.Max(headers?.Length ?? 0, rows.Select(row => row.Length).DefaultIfEmpty().Max()));
        var table = new NativeTable(columnCount);
        var rowIndex = 0;
        if (headers is not null)
        {
            table.AddRow(RowSurface(0, true), headers.Select(header => (UIElement)Cell(Text(header).Bold(), true)).ToArray());
            rowIndex++;
        }
        foreach (var row in rows)
        {
            table.AddRow(RowSurface(rowIndex, false), row.Select(cell => (UIElement)Cell(cell, false)).ToArray());
            rowIndex++;
        }
        return table;
    }

    private Border Cell(Element content, bool header)
    {
        var border = new Border().Padding(Options.Style.SuperDense ? new Thickness(8, 4) : Options.Style.Dense ? new Thickness(12, 6) : new Thickness(16, 12))
        .WithTheme((theme, border) =>
        {
            border.BorderBrush(theme.Palette.ControlBorder);
            border.BorderThickness(Options.Style.Bordered ? .5 : 0);
        }).Child((UIElement)content);
        if (header) NativeStyle.ApplyClassAndStyle(border, Options.Style.ThStyle);
        return border;
    }

    private Border RowSurface(int row, bool header)
    {
        var style = Style.DeriveFromDefault<Border>(
            setters: [Setter.Create(Control.BackgroundProperty, theme => header || Options.Style.Striped && row % 2 == 1 ? theme.Palette.ControlBackground : NativeColor.Transparent)],
            triggers: !header && Options.Style.Hover ? [new StateTrigger
            {
                Match = VisualStateFlags.Hot,
                Setters = [Setter.Create(Control.BackgroundProperty, theme => theme.Palette.ButtonHoverBackground)]
            }] : []);
        var sheet = new StyleSheet();
        sheet.Define("TableViewerRow", () => style);
        return new Border().StyleSheet(sheet).StyleName("TableViewerRow");
    }

    internal StackPanel Buttons(object? item, IEnumerable<ITvButton> options, int depth, string path)
    {
        var panel = new StackPanel().Horizontal().Spacing(4);
        foreach (var option in options.Where(button => button.Condition(item, depth)))
        {
            var button = NativeStyle.Button(new NativeActionButton().Content(NativeStyle.ButtonContent(option.Label, option.Style)), option.Style);
            if (option.TooltipOption is { } tip)
            {
                var tooltip = new NativeTooltip(button, tip);
                tooltip.Error += exception => Error?.Invoke(exception);
                _unsubscriptions.Add(tooltip.Dispose);
            }
            if (option is ITvLink link) button.OnClick(async () => await OpenLink(link.Href(item), link.Target));
            else if (option is ITvPopupAction popup) button.OnClick(async () => await Popup(item, popup));
            else if (option is ITvAction action)
            {
                var key = (path, action);
                if (!_actions.TryGetValue(key, out var controller)) _actions[key] = controller = new(action, item);
                void Update()
                {
                    button.IsEnabled = controller.State is not (TvActionState.Running or TvActionState.Done);
                    var label = controller.State switch { TvActionState.Done => action.LabelAfterClick, _ => option.Label };
                    var iconStyle = action is ITvDoubleClickAction second && controller.State == TvActionState.Armed
                        ? new TvButtonStyle { StartIcon = option.Style.StartIcon, EndIcon = option.Style.EndIcon, IconColor = second.SecondStyle.IconColor, IconSize = option.Style.IconSize }
                        : option.Style;
                    var progressStyle = (action as ITvOneClickAction)?.ProgressStyle;
                    var progressSize = progressStyle?.Size switch { TvSize.Large => 28, TvSize.Medium => 22, _ => 16 };
                    var progress = new ProgressRing().Size(progressSize).WithTheme((theme, ring) => ring.Foreground(NativeStyle.ThemeColor(progressStyle?.Color ?? TvColor.Default, theme.Palette)));
                    button.Content(controller.State == TvActionState.Running ? progress : NativeStyle.ButtonContent(label, iconStyle));
                    if (action is ITvDoubleClickAction doubleClick && controller.State == TvActionState.Armed)
                        NativeStyle.Button(button, new TvButtonStyle { Color = doubleClick.SecondStyle.Color, Variant = doubleClick.SecondStyle.Variant, Size = option.Style.Size, IconColor = doubleClick.SecondStyle.IconColor });
                    else NativeStyle.Button(button, option.Style);
                }
                Update();
                controller.Changed += Update;
                _unsubscriptions.Add(() => controller.Changed -= Update);
                button.OnClick(async () => { try { await controller.InvokeAsync(); } catch (Exception exception) { button.ToolTip(exception.Message); Error?.Invoke(exception); } });
            }
            panel.Children(NativeStyle.ButtonChrome(button, option.Style));
        }
        return panel;
    }

    private async Task Popup(object? item, ITvPopupAction action)
    {
        try
        {
            var owner = OwnerWindow() ?? throw new InvalidOperationException("Popup actions require a viewer attached to a window.");
            var style = action.PopupStyle;
            var completion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            var previousFocus = owner.FocusManager.FocusedElement;
            var underlying = owner.Content as UIElement;
            var wasEnabled = underlying?.IsEnabled ?? true;
            var closed = false;
            var body = new Border().Padding(20).CornerRadius(style.FullScreen ? 0 : 5)
                .WithTheme((theme, panel) => panel.Background(theme.Palette.ControlBackground));
            var backdrop = new Border().Background(NativeColor.FromArgb(100, 0, 0, 0)).Child(body);
            var error = Text("").Foreground(NativeColor.Red);
            var confirm = NativeStyle.Button(new NativeActionButton().Content(NativeStyle.ButtonContent(action.InnerButtonOptions.ConfirmLabel, action.InnerButtonOptions.ConfirmButtonStyle)), action.InnerButtonOptions.ConfirmButtonStyle);
            var close = NativeStyle.Button(new NativeActionButton().Content(NativeStyle.ButtonContent(action.InnerButtonOptions.CloseLabel, action.InnerButtonOptions.CloseButtonStyle)), action.InnerButtonOptions.CloseButtonStyle);
            var x = new Button().Content("×");
            var focusable = style.CloseButton && !style.NoHeader ? new[] { x, confirm, close } : new[] { confirm, close };
            void Finish()
            {
                if (closed) return;
                closed = true;
                owner.PreviewKeyDown -= OnKey;
                owner.ClientSizeChanged -= Resize;
                owner.Closed -= Finish;
                _closeDialogs.Remove(Finish);
                owner.OverlayLayer.Remove(backdrop);
                if (underlying is not null) underlying.IsEnabled = wasEnabled;
                previousFocus?.Focus();
                backdrop.Dispose();
                completion.TrySetResult();
            }
            void OnKey(KeyEventArgs args)
            {
                if (args.Key == Key.Escape) { if (style.CloseOnEscapeKey) Finish(); args.Handled = true; }
                else if (args.Key == Key.Tab)
                {
                    var current = Array.FindIndex(focusable, button => button.IsFocused);
                    var step = args.ShiftKey ? -1 : 1;
                    for (var offset = 1; offset <= focusable.Length; offset++)
                    {
                        var next = focusable[(current + step * offset + focusable.Length * 2) % focusable.Length];
                        if (next.IsEnabled) { next.Focus(); break; }
                    }
                    args.Handled = true;
                }
            }
            void Resize(Aprillz.MewUI.Size size)
            {
                backdrop.Width = size.Width;
                backdrop.Height = size.Height;
                var maximum = style.MaxWidth switch
                {
                    TvMaxWidth.ExtraSmall => 444, TvMaxWidth.Small => 600, TvMaxWidth.Medium => 960,
                    TvMaxWidth.Large => 1280, TvMaxWidth.ExtraLarge => 1920, TvMaxWidth.ExtraExtraLarge => 2560,
                    TvMaxWidth.False => double.PositiveInfinity, _ => style.Width
                };
                var width = style.FullScreen ? size.Width : Math.Max(0, Math.Min(size.Width - 48, style.FullWidth ? maximum : Math.Min(style.Width, maximum)));
                body.Width = width;
                body.MaxHeight = Math.Max(0, size.Height - (style.FullScreen ? 0 : 48));
                body.Height = style.FullScreen ? size.Height : double.NaN;
                body.HorizontalAlignment = HorizontalAlignment.Center;
                body.VerticalAlignment = VerticalAlignment.Center;
            }
            confirm.OnClick(async () =>
            {
                confirm.IsEnabled = false;
                try { await action.Action(item); Finish(); }
                catch (Exception exception)
                {
                    if (!closed) { confirm.IsEnabled = true; error.Text = exception.Message; }
                    Error?.Invoke(exception);
                }
            });
            close.OnClick(Finish); x.OnClick(Finish);
            var content = new DockPanel().Spacing(16).Children(
                new StackPanel().Vertical().Spacing(4).DockBottom().Children(error, new StackPanel().Horizontal().Spacing(8).Children(
                    NativeStyle.ButtonChrome(confirm, action.InnerButtonOptions.ConfirmButtonStyle), NativeStyle.ButtonChrome(close, action.InnerButtonOptions.CloseButtonStyle))),
                new ScrollViewer().Content(Text(action.PopupContent(item!))));
            if (!style.NoHeader)
            {
                var header = new Grid().Columns("*,Auto").Spacing(8).Children(Text(action.PopupTitle(item!)).FontSize(20).Bold());
                if (style.CloseButton) header.Children(x.Column(1));
                content.Insert(0, header.DockTop());
            }
            body.Child = content;
            Resize(owner.ClientSize);
            style.Configure?.Invoke(body);
            backdrop.OnMouseDown(args =>
            {
                if (ReferenceEquals(args.Source, backdrop) && style.BackdropClick) Finish();
                args.Handled = true;
            });
            if (underlying is not null) underlying.IsEnabled = false;
            owner.PreviewKeyDown += OnKey;
            owner.ClientSizeChanged += Resize;
            owner.Closed += Finish;
            _closeDialogs.Add(Finish);
            try
            {
                owner.OverlayLayer.Add(backdrop);
                confirm.Focus();
                await completion.Task;
            }
            finally { Finish(); }
        }
        catch (Exception exception) { Error?.Invoke(exception); }
    }

    private UIElement Code(object data, ITvEditorOption option)
    {
        var text = option.Serializer?.Invoke(data) ?? JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        return new TextEditor { Text = text, IsReadOnly = true, ShowLineNumbers = false, SyntaxHighlighting = NativeHighlighting.Get(option.Language) }
            .Height(Math.Max(24, Math.Min(text.Split('\n').Length * 20 + 8, option.LayoutMaxSize?.Height ?? 600)))
            .Width(Math.Max(100, Math.Min(text.Split('\n').Max(line => line.Length) * 9 + 24, option.LayoutMaxSize?.Width ?? 900)));
    }

    private UIElement Image(object parent, object data, ITvImageViewerOption option)
    {
        var host = new Border().Child(Text(option.Alt ?? "Loading image…"));
        if (option.Width is { } width) host.Width(width);
        if (option.Height is { } height) host.Height(height);
        var token = _lifetime.Token;
        _ = LoadImage();
        return option.Elevation > 0
            ? new ShadowDecorator { BlurRadius = Math.Min(30, option.Elevation * 2), OffsetY = Math.Max(1, option.Elevation / 2.0), ShadowColor = NativeColor.FromArgb(65, 0, 0, 0), Child = host }
            : host;
        async Task LoadImage()
        {
            foreach (var urlFactory in new Func<string>[] { () => option.ImageUrl(parent, data), () => option.FallbackUrl(parent, data) })
            {
                ImageSource? pendingSource = null;
                try
                {
                    token.ThrowIfCancellationRequested();
                    var url = urlFactory();
                    if (string.IsNullOrWhiteSpace(url)) continue;
                    byte[] bytes;
                    if (url.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
                    {
                        var comma = url.IndexOf(',');
                        if (comma < 0) throw new FormatException("Invalid image data URI.");
                        bytes = url[..comma].EndsWith(";base64", StringComparison.OrdinalIgnoreCase)
                            ? Convert.FromBase64String(url[(comma + 1)..]) : System.Text.Encoding.UTF8.GetBytes(Uri.UnescapeDataString(url[(comma + 1)..]));
                    }
                    else if (Uri.TryCreate(url, UriKind.Absolute, out var uri) && uri.Scheme is "http" or "https") bytes = await Http.GetByteArrayAsync(uri, token);
                    else bytes = await File.ReadAllBytesAsync(uri?.IsFile == true ? uri.LocalPath : url, token);
                    token.ThrowIfCancellationRequested();
                    var source = pendingSource = ImageSource.FromBytes(bytes);
                    source.EnsureDecode();
                    if (source.PixelWidth <= 0 || source.PixelHeight <= 0) throw new InvalidDataException("The image could not be decoded.");
                    token.ThrowIfCancellationRequested();
                    _images.Add(source);
                    pendingSource = null;
                    var image = new ViewerImage(option.Fluid, option.ObjectFit == ObjectFit.ScaleDown)
                    {
                        Source = source,
                        StretchMode = option.ObjectFit switch { ObjectFit.Contain or ObjectFit.ScaleDown => Stretch.Uniform, ObjectFit.Cover => Stretch.UniformToFill, ObjectFit.None => Stretch.None, _ => Stretch.Fill }
                    };
                    if (option.Width is { } w) image.Width(w);
                    if (option.Height is { } h) image.Height(h);
                    image.ToolTip(option.Alt ?? "");
                    image.AlignmentX = option.ObjectPosition is ObjectPosition.Left or ObjectPosition.TopLeft or ObjectPosition.BottomLeft ? ImageAlignmentX.Left : option.ObjectPosition is ObjectPosition.Right or ObjectPosition.TopRight or ObjectPosition.BottomRight ? ImageAlignmentX.Right : ImageAlignmentX.Center;
                    image.AlignmentY = option.ObjectPosition is ObjectPosition.Top or ObjectPosition.TopLeft or ObjectPosition.TopRight ? ImageAlignmentY.Top : option.ObjectPosition is ObjectPosition.Bottom or ObjectPosition.BottomLeft or ObjectPosition.BottomRight ? ImageAlignmentY.Bottom : ImageAlignmentY.Center;
                    host.Child = image;
                    return;
                }
                catch (OperationCanceledException) { pendingSource?.Dispose(); return; }
                catch (Exception exception)
                {
                    pendingSource?.Dispose();
                    if (token.IsCancellationRequested) return;
                    host.Child = Text(option.Alt ?? exception.Message);
                }
            }
        }
    }

    private sealed class ViewerImage : ContentControl
    {
        private readonly Image _image = new();
        private readonly bool _fluid;
        private readonly bool _scaleDown;
        public ViewerImage(bool fluid, bool scaleDown)
        {
            _fluid = fluid; _scaleDown = scaleDown;
            Content = _image;
        }
        public ImageSource? Source { get => _image.Source as ImageSource; set => _image.Source = value; }
        public Stretch StretchMode { get => _image.StretchMode; set => _image.StretchMode = value; }
        public ImageAlignmentX AlignmentX { get => _image.AlignmentX; set => _image.AlignmentX = value; }
        public ImageAlignmentY AlignmentY { get => _image.AlignmentY; set => _image.AlignmentY = value; }
        protected override Aprillz.MewUI.Size MeasureOverride(Aprillz.MewUI.Size availableSize)
        {
            var desired = base.MeasureOverride(availableSize);
            if (!_fluid || Source is null || !double.IsFinite(availableSize.Width) || desired.Width <= availableSize.Width) return desired;
            var factor = availableSize.Width / Math.Max(1, desired.Width);
            return new Aprillz.MewUI.Size(availableSize.Width, desired.Height * factor);
        }

        protected override Aprillz.MewUI.Size ArrangeOverride(Aprillz.MewUI.Size finalSize)
        {
            if (_scaleDown && Source is { } source)
            {
                var desired = finalSize.Width >= source.PixelWidth && finalSize.Height >= source.PixelHeight ? Stretch.None : Stretch.Uniform;
                if (StretchMode != desired) StretchMode = desired;
            }
            return base.ArrangeOverride(finalSize);
        }
    }

    private static readonly HttpClient Http = new();
    private async Task OpenLink(string href, string target)
    {
        try
        {
            if (LinkRequested is not null) await LinkRequested(href, target);
            else if (!string.IsNullOrWhiteSpace(href)) Process.Start(new ProcessStartInfo(href) { UseShellExecute = true });
        }
        catch (Exception exception) { Error?.Invoke(exception); }
    }
    private Window? OwnerWindow() { for (Element? current = ActionOwner ?? this; current is not null; current = current.Parent) if (current is Window window) return window; return null; }
    private static TextBlock Text(string text) => new TextBlock().Text(text).TextWrapping(TextWrapping.Wrap);
    private static string ChildPath(string path, string key) => path + "[" + JsonSerializer.Serialize(key) + "]";
    private UIElement Failure(Exception exception) { Error?.Invoke(exception); return Text($"Error: {exception.Message}").Foreground(NativeColor.Red); }
    private void ClearVisualResources()
    {
        _lifetime.Cancel(); _lifetime.Dispose(); _lifetime = new();
        foreach (var unsubscribe in _unsubscriptions) unsubscribe();
        _unsubscriptions.Clear();
        foreach (var image in _images) image.Dispose();
        _images.Clear();
    }
    protected override void OnDispose()
    {
        _disposed = true;
        foreach (var close in _closeDialogs.ToArray()) close();
        foreach (var controller in _actions.Values) controller.Dispose();
        _actions.Clear();
        ClearVisualResources(); _lifetime.Cancel(); _lifetime.Dispose();
        base.OnDispose();
    }
}
