using TableViewer.MewUI.Options.Property;
using TableViewer.MewUI.Internal.ViewerModel;

namespace TableViewer.MewUI.Internal.Editing;

internal sealed class TeEditorRenderer : IDisposable
{
    private readonly TeEditSession _session;
    private readonly Dictionary<TeEditNode, TextBlock> _errorLabels = [];
    private readonly List<CancellationTokenSource> _pending = [];
    private readonly List<Popup> _popups = [];
    private readonly Dictionary<TeEditNode, Control> _fieldInputs = [];
    private readonly string _radioGroup = Guid.NewGuid().ToString("N");
    private bool _disposed;

    public TeEditorRenderer(TeEditSession session)
    {
        _session = session;
        session.ValidationChanged += RefreshErrors;
    }

    public UIElement Build()
    {
        var panel = new StackPanel().Vertical().Spacing(_session.Options.Style.SuperDense ? 4 : _session.Options.Style.Dense ? 8 : 14);
        if (!string.IsNullOrWhiteSpace(_session.Options.Title)) panel.Children(new TextBlock().Text(_session.Options.Title).FontSize(20).Bold());
        if (_session.Options.ToolbarButtons.Count > 0)
        {
            var toolbar = new StackPanel().Horizontal().Spacing(6);
            foreach (var option in _session.Options.ToolbarButtons) toolbar.Children(BuildToolbarButton(option));
            panel.Children(toolbar);
        }
        panel.Children(BuildNode(_session.Root, true));
        NativeStyle.ApplyClasses(panel, _session.Options.Style.RootClass);
        return panel;
    }

    private UIElement BuildNode(TeEditNode node, bool root = false)
    {
        if (node.Diagnostic is not null) return new TextBlock().Text(node.Diagnostic);
        if (node.Option is TeCustomEditorOption custom && custom.RenderFragment is { } factory) return RequireVisual(factory(_session.CustomArgument(node)));
        if (node.List is not null) return BuildList(node);
        if (node.Option is null && node.Children.Count > 0) return BuildObject(node, root);
        UIElement input = node.Option switch
        {
            ITeTextFieldOption option => BuildText(node, option),
            ITeNumericFieldOption option => BuildNumber(node, option),
            ITeSelectBoxOption option => BuildSelect(node, option),
            ITeRadioOption option => BuildRadio(node, option),
            ITeCheckBoxOption option => BuildCheckBox(node, option),
            ITeSwitchOption option => BuildSwitch(node, option),
            ITeAutocompleteOption option => BuildAutocomplete(node, option),
            _ when (Nullable.GetUnderlyingType(node.DeclaredType) ?? node.DeclaredType).IsEnum => BuildEnum(node),
            _ => new TextBlock().Text(node.Value?.ToString() ?? "null")
        };
        return WrapField(node, input);
    }

    private UIElement BuildObject(TeEditNode node, bool root)
    {
        var grid = new Grid().Columns("160,*").Rows(string.Join(',', Enumerable.Repeat("Auto", node.Children.Count))).Spacing(8);
        for (var index = 0; index < node.Children.Count; index++)
        {
            var child = node.Children[index];
            grid.Children(DecorateCell(new TextBlock().Text(child.Label).CenterVertical(), index).Row(index), DecorateCell(BuildNode(child), index).Row(index).Column(1));
        }
        return root ? grid : new Expander().Header(node.Label).IsExpanded(true).Content(grid);
    }

    private UIElement WrapField(TeEditNode node, UIElement input)
    {
        var panel = new StackPanel().Vertical().Spacing(3);
        var property = TeEditSession.Property(node.Option);
        if (property is TeBaseInputProperty { Label: { Length: > 0 } label }) panel.Children(new TextBlock().Text(label + (property.Required ? " *" : "")));
        panel.Children(input);
        var errors = new TextBlock().Text(string.Join("\n", node.Errors));
        errors.WithTheme((theme, text) => text.Foreground(theme.Palette.Error));
        _errorLabels[node] = errors;
        panel.Children(errors);
        if (property is TeBaseInputProperty { HelperText: { Length: > 0 } help } helper)
        {
            var helperLabel = new TextBlock().Text(help).IsVisible(!helper.HelperTextOnFocus);
            panel.Children(helperLabel);
            if (helper.HelperTextOnFocus && _fieldInputs.TryGetValue(node, out var ui))
            {
                ui.OnGotFocus(() => helperLabel.IsVisible = true).OnLostFocus(() => helperLabel.IsVisible = false);
            }
        }
        if (property is not null)
        {
            NativeStyle.ApplyClasses(input, (property.Class ?? "").Split(' ', StringSplitOptions.RemoveEmptyEntries));
            NativeStyle.ApplyCss(input, property.Style);
            if (!_fieldInputs.ContainsKey(node) && input is Control control) property.Configure?.Invoke(control);
        }
        return panel;
    }

    private UIElement BuildText(TeEditNode node, ITeTextFieldOption option)
    {
        var property = option.Property;
        if (property.InputType == InputType.Hidden) return new Border().IsVisible(false);
        if (property.InputType is InputType.Date or InputType.DateTimeLocal or InputType.Month or InputType.Time or InputType.Week or InputType.Color)
            return BuildSpecialText(node, option);
        var value = Convert.ToString(node.FieldValue, property.Culture ?? CultureInfo.InvariantCulture) ?? "";
        var state = new ObservableValue<string>(value);
        var counter = new TextBlock();
        Control input;
        Func<string> read;
        Action<string> write;
        if (property.InputType == InputType.Password)
        {
            var password = new PasswordBox().BindPassword(state).MaxLength(property.MaxLength).IsReadOnly(property.ReadOnly).IsEnabled(!property.Disabled);
            input = password; read = () => password.Password; write = text => password.Password = text;
        }
        else if (property.Lines > 1)
        {
            var multiline = new MultiLineTextBox().BindText(state).IsReadOnly(property.ReadOnly).IsEnabled(!property.Disabled)
                .Height(Math.Max(60, property.Lines * 22));
            input = multiline; read = () => multiline.Text; write = text => multiline.Text = text;
            if (property.AutoGrow)
                multiline.OnTextChanged(text => multiline.Height = Math.Max(60,
                    Math.Clamp(text.Count(character => character == '\n') + 1, property.Lines, Math.Max(property.Lines, property.MaxLines > 0 ? property.MaxLines : 1000)) * 22));
        }
        else
        {
            var text = new TextBox().BindText(state).Placeholder(property.Placeholder ?? "").MaxLength(property.MaxLength)
                .IsReadOnly(property.ReadOnly).IsEnabled(!property.Disabled);
            input = text; read = () => text.Text; write = value => text.Text = value;
        }
        CancellationTokenSource? debounce = null;
        var draft = value;
        var lastAcceptedDraft = value;
        var formatting = false;
        async Task Commit()
        {
            if (_disposed) return;
            try
            {
                var text = property.Mask?.Apply(draft) ?? draft;
                await _session.SetFieldValueAsync(node, text);
            }
            catch (Exception exception) { node.Errors = [exception.Message]; RefreshErrors(); }
        }
        async void Changed(string text)
        {
            if (_disposed || formatting) return;
            if (property.Mask is ITeInputMask inputMask)
            {
                try
                {
                    var formatted = inputMask.FormatInput(text);
                    if (formatted != text) { formatting = true; state.Value = formatted; write(formatted); formatting = false; text = formatted; }
                    lastAcceptedDraft = text;
                }
                catch (FormatException)
                {
                    formatting = true; state.Value = lastAcceptedDraft; write(lastAcceptedDraft); formatting = false;
                    return;
                }
            }
            draft = text;
            UpdateCounter();
            try
            {
                if (option.Event?.OnInternalInputChanged is { } internalChanged) await internalChanged(new(text));
                debounce?.Cancel();
                debounce = new(); _pending.Add(debounce);
                if (property.DebounceInterval > 0)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(property.DebounceInterval), debounce.Token);
                    if (option.Event?.OnDebounceIntervalElapsed is { } elapsed) await elapsed(text);
                }
                if (property.Immediate || property.DebounceInterval > 0) await Commit();
            }
            catch (OperationCanceledException) { }
            catch (Exception exception) { node.Errors = [exception.Message]; RefreshErrors(); }
        }
        state.Changed += () => Changed(state.Value);
        input.OnLostFocus(async () =>
        {
            if (!property.Immediate) { draft = read(); await Commit(); }
            if (option.Event?.OnBlur is { } blur) await Safe(() => blur(new()));
        }).OnKeyDown(async args =>
        {
            if (args.Key == Key.Enter && property.Lines <= 1) { draft = read(); await Commit(); }
            if (option.Event?.OnKeyDown is { } down) await Safe(() => down(args));
            if (option.Event?.OnKeyPress is { } press) await Safe(() => press(args));
        }).OnKeyUp(async args => { if (option.Event?.OnKeyUp is { } up) await Safe(() => up(args)); });
        if (property.AutoFocus && Application.Current is { } application) application.Dispatcher?.BeginInvoke(() => input.Focus());
        ApplyInputAppearance(input, property);
        _fieldInputs[node] = input;
        var result = new StackPanel().Vertical().Spacing(3);
        var adornment = new DockPanel().Spacing(4);
        if (property.Adornment != Adornment.None)
        {
            var adornmentButton = new Button().Content(property.AdornmentText ?? property.AdornmentIcon ?? "…")
                .OnClick(async () => { if (option.Event?.OnAdornmentClick is { } click) await Safe(() => click(new())); });
            if (property.Adornment == Adornment.Start) adornmentButton.DockLeft(); else adornmentButton.DockRight();
            adornment.Children(adornmentButton);
        }
        if (property.Clearable)
            adornment.Children(new Button().Content(string.IsNullOrWhiteSpace(property.ClearIcon) ? "×" : property.ClearIcon).DockRight().IsEnabled(!TeEditSession.IsReadOnly(node))
                .OnClick(async () => { write(""); state.Value = ""; draft = ""; await Commit(); if (option.Event?.OnClearButtonClick is { } clear) await Safe(() => clear(new())); }));
        adornment.Children(input);
        result.Children(adornment);
        if (property.Counter is not null) { UpdateCounter(); result.Children(counter); }
        return result;

        void UpdateCounter() => counter.Text = property.Counter > 0 ? $"{draft.Length} / {property.Counter}" : draft.Length.ToString();
        async Task Safe(Func<Task> callback) { try { await callback(); } catch (Exception exception) { node.Errors = [exception.Message]; RefreshErrors(); } }
    }

    private UIElement BuildNumber(TeEditNode node, ITeNumericFieldOption option)
    {
        var property = option.Property;
        var input = new TextBox().Text(node.FieldValue is IFormattable number ? number.ToString(property.Format, property.Culture ?? CultureInfo.InvariantCulture) : node.FieldValue?.ToString() ?? "")
            .Placeholder(property.Placeholder ?? "").IsReadOnly(property.ReadOnly).IsEnabled(!property.Disabled);
        async Task Commit()
        {
            try { await _session.SetFieldValueAsync(node, TeNumericOperations.Parse(input.Text, option)); }
            catch (Exception exception) { node.ConversionError = exception.InnerException?.Message ?? exception.Message; node.Errors = [node.ConversionError]; RefreshErrors(); }
        }
        CancellationTokenSource? debounce = null;
        input.OnTextChanged(async _ =>
        {
            try
            {
                debounce?.Cancel();
                if (property.DebounceInterval > 0)
                {
                    debounce = new(); _pending.Add(debounce);
                    await Task.Delay(TimeSpan.FromMilliseconds(property.DebounceInterval), debounce.Token);
                }
                if (!_disposed && (property.Immediate || property.DebounceInterval > 0)) await Commit();
            }
            catch (OperationCanceledException) { }
        })
            .OnLostFocus(async () => await Commit()).OnKeyDown(async args =>
            {
                if (args.Key == Key.Enter) await Commit();
                else if (args.Key is Key.Up or Key.Down)
                {
                    await Step(args.Key == Key.Up ? 1 : -1); args.Handled = true;
                }
            });
        input.OnMouseWheel(async args =>
        {
            if (TeEditSession.IsReadOnly(node) || args.Delta.Y == 0) return;
            await Step(args.Delta.Y > 0 ? 1 : -1); args.Handled = true;
        });
        var result = new DockPanel().Spacing(3);
        if (!property.HideSpinButtons)
            result.Children(new StackPanel().Horizontal().Spacing(2).DockRight().Children(
                new Button().Content("−").IsEnabled(!TeEditSession.IsReadOnly(node)).OnClick(async () => await Step(-1)),
                new Button().Content("+").IsEnabled(!TeEditSession.IsReadOnly(node)).OnClick(async () => await Step(1))));
        if (property.Clearable) result.Children(new Button().Content("×").DockRight().IsEnabled(!TeEditSession.IsReadOnly(node)).OnClick(async () =>
        { input.Text = Convert.ToString(option.DefaultValue, property.Culture) ?? "0"; await Commit(); }));
        result.Children(input);
        ApplyInputAppearance(input, property);
        _fieldInputs[node] = input;
        return result;

        async Task Step(int direction)
        {
            try
            {
                var next = TeNumericOperations.Step(node.FieldValue, option, direction);
                input.Text = Convert.ToString(next, property.Culture ?? CultureInfo.InvariantCulture) ?? "";
                await _session.SetFieldValueAsync(node, next);
            }
            catch (Exception exception) { node.Errors = [exception.InnerException?.Message ?? exception.Message]; RefreshErrors(); }
        }
    }

    private UIElement BuildSpecialText(TeEditNode node, ITeTextFieldOption option)
    {
        var property = option.Property;
        var original = Convert.ToString(node.FieldValue, property.Culture ?? CultureInfo.InvariantCulture) ?? "";
        if (property.InputType == InputType.Color)
        {
            var picker = new ColorPicker().IsEnabled(!property.Disabled && !property.ReadOnly);
            try { picker.SelectedColor = Aprillz.MewUI.Color.FromHex(original); } catch (Exception) { }
            picker.SelectedColorChanged += async color => await Commit($"#{color.R:X2}{color.G:X2}{color.B:X2}");
            ApplyInputAppearance(picker, property); _fieldInputs[node] = picker;
            return picker;
        }
        if (property.InputType == InputType.Time)
        {
            var editor = new TextBox().Text(original).Placeholder(property.Placeholder ?? "HH:mm:ss")
                .IsReadOnly(property.ReadOnly).IsEnabled(!property.Disabled);
            async Task CommitTime()
            {
                if (TimeOnly.TryParse(editor.Text, property.Culture ?? CultureInfo.InvariantCulture, DateTimeStyles.None, out var time)) await Commit(time.ToString("HH:mm:ss", CultureInfo.InvariantCulture));
                else { node.ConversionError = "Enter a time such as 14:30:00."; node.Errors = [node.ConversionError]; RefreshErrors(); }
            }
            editor.OnLostFocus(async () => await CommitTime()).OnKeyDown(async args => { if (args.Key == Key.Enter) await CommitTime(); });
            ApplyInputAppearance(editor, property); _fieldInputs[node] = editor;
            return editor;
        }
        var pickerDate = new DatePicker
        {
            DateFormat = property.InputType == InputType.Month ? "yyyy-MM" : "yyyy-MM-dd",
            Placeholder = property.Placeholder ?? "Choose a date",
            IsEnabled = !property.Disabled && !property.ReadOnly
        };
        var initialTime = "00:00:00";
        if (DateTime.TryParse(original, property.Culture ?? CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
        { pickerDate.SelectedDate = date; initialTime = date.ToString("HH:mm:ss", CultureInfo.InvariantCulture); }
        else if (property.InputType == InputType.Week && System.Text.RegularExpressions.Regex.Match(original, @"^(\d{4})-W(\d{2})$") is { Success: true } week)
        {
            try { pickerDate.SelectedDate = ISOWeek.ToDateTime(int.Parse(week.Groups[1].Value), int.Parse(week.Groups[2].Value), DayOfWeek.Monday); }
            catch (ArgumentOutOfRangeException) { }
        }
        var timeEditor = new TextBox().Text(initialTime).Width(100)
            .IsEnabled(!property.Disabled && !property.ReadOnly);
        async Task CommitDate()
        {
            if (pickerDate.SelectedDate is not { } selectedDate) { await Commit(""); return; }
            var formatted = property.InputType switch
            {
                InputType.Month => selectedDate.ToString("yyyy-MM", CultureInfo.InvariantCulture),
                InputType.Week => $"{ISOWeek.GetYear(selectedDate):0000}-W{ISOWeek.GetWeekOfYear(selectedDate):00}",
                _ => selectedDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
            };
            if (property.InputType == InputType.DateTimeLocal)
            {
                if (!TimeOnly.TryParse(timeEditor.Text, CultureInfo.InvariantCulture, DateTimeStyles.None, out var time))
                { node.ConversionError = "Enter a valid time."; node.Errors = [node.ConversionError]; RefreshErrors(); return; }
                formatted += "T" + time.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
            }
            await Commit(formatted);
        }
        pickerDate.SelectedDateChanged += async _ => await CommitDate();
        timeEditor.OnLostFocus(async () => await CommitDate()).OnKeyDown(async args => { if (args.Key == Key.Enter) await CommitDate(); });
        ApplyInputAppearance(pickerDate, property); _fieldInputs[node] = pickerDate;
        return property.InputType == InputType.DateTimeLocal
            ? new StackPanel().Horizontal().Spacing(5).Children(pickerDate, timeEditor) : pickerDate;

        async Task Commit(string text)
        {
            await _session.SetFieldValueAsync(node, text);
            if (option.Event?.OnInternalInputChanged is { } inputChanged) await inputChanged(new(text));
        }
    }

    private UIElement BuildSelect(TeEditNode node, ITeSelectBoxOption option)
    {
        var items = option.Items.ToArray();
        var input = new TeSelectControl(items, option.Property)
            .IsEnabled(!option.Property.Disabled && !option.Property.ReadOnly);
        var index = Array.FindIndex(items, item => Equals(item.Value, node.Value));
        if (index < 0 && node.Value is null) index = Array.FindIndex(items, item => item.Default);
        input.SelectedIndex = index;
        input.SelectionChanged += async selected => { if (selected is not null) await _session.SetValueAsync(node, selected.Value); };
        var result = new DockPanel().Spacing(3);
        result.Children(new Button().Content(new ContentControl().Bind(ContentControl.ContentProperty, input, PopupOwnerBase.IsDropDownOpenProperty,
                isOpen => ChoiceGlyph(isOpen ? option.Property.CloseIcon : option.Property.OpenIcon, option.Property.IconSize, option.Property.AdornmentColor)))
            .IsEnabled(!TeEditSession.IsReadOnly(node)).DockRight().OnClick(() => input.IsDropDownOpen = !input.IsDropDownOpen));
        if (option.Property.Clearable) result.Children(new Button().Content(ChoiceGlyph(option.Property.ClearIcon, option.Property.IconSize, option.Property.AdornmentColor)).DockRight().IsEnabled(!TeEditSession.IsReadOnly(node)).OnClick(async () =>
        { input.SelectedIndex = -1; await _session.SetValueAsync(node, null); }));
        result.Children(input);
        ApplyInputAppearance(input, option.Property);
        _fieldInputs[node] = input;
        NativeStyle.ApplyClasses(input, (option.Property.InputClass ?? "").Split(' ', StringSplitOptions.RemoveEmptyEntries));
        NativeStyle.ApplyClasses(result, (option.Property.OuterClass ?? "").Split(' ', StringSplitOptions.RemoveEmptyEntries));
        return result;
    }

    private UIElement BuildRadio(TeEditNode node, ITeRadioOption option)
    {
        var panel = new WrapPanel().Spacing(8);
        foreach (var item in option.Items)
        {
            var radio = new RadioButton().Content(item.Text).GroupName(_radioGroup + node.Path)
                .IsChecked(Equals(item.Value, node.Value) || node.Value is null && item.Default)
                .IsEnabled(!option.Property.Disabled && !option.Property.ReadOnly && !item.Property.Disabled)
                .OnChecked(async () => await _session.SetValueAsync(node, item.Value));
            ApplyChoiceAppearance(radio, item.Property.Color, item.Property.Size, item.Property.Dense);
            radio.Template = new DelegateControlTemplate<RadioButton>((owner, _) =>
            {
                var circle = new ContentControl().Bind(ContentControl.ContentProperty, owner, RadioButton.IsCheckedProperty,
                    value => ChoiceGlyph(value ? "◉" : "○", item.Property.Size, value ? item.Property.Color : item.Property.UncheckedColor ?? item.Property.Color));
                return PlaceChoiceLabel(circle, new TextBlock().Text(item.Text).CenterVertical(), item.Property.Placement);
            });
            NativeStyle.ApplyCss(radio, item.Property.Style);
            NativeStyle.ApplyClasses(radio, (item.Property.Class ?? "").Split(' ', StringSplitOptions.RemoveEmptyEntries));
            item.Property.Configure?.Invoke(radio);
            panel.Children(radio);
        }
        return panel;
    }

    private UIElement BuildCheckBox(TeEditNode node, ITeCheckBoxOption option)
    {
        var label = new TextBlock().Text(option.Property.Label ?? node.Value?.ToString() ?? "").CenterVertical();
        var input = new CheckBox().Content("")
            .IsThreeState(option.Property.TriState).IsChecked(option.Converter.ToField(node.Value!))
            .IsEnabled(!option.Property.Disabled && !option.Property.ReadOnly);
        input.OnCheckedChanged(async value =>
        {
            await _session.SetFieldValueAsync(node, value);
            label.Text = option.Property.Label ?? node.Value?.ToString() ?? "";
            ApplyChoiceAppearance(input, value ? option.Property.Color : option.Property.UncheckedColor ?? option.Property.Color, option.Property.Size, option.Property.Dense);
        });
        ApplyChoiceAppearance(input, option.Property.Color, option.Property.Size, option.Property.Dense);
        input.Template = new DelegateControlTemplate<CheckBox>((owner, _) =>
            new ContentControl().Bind(ContentControl.ContentProperty, owner, CheckBox.IsCheckedProperty,
                value => ChoiceGlyph(value is true ? option.Property.CheckedIcon : value is false ? option.Property.UncheckedIcon : option.Property.IndeterminateIcon,
                    option.Property.Size, value is true ? option.Property.Color : option.Property.UncheckedColor ?? option.Property.Color)));
        if (!option.Property.KeyboardEnabled) input.OnKeyDown(args => args.Handled = true);
        option.Property.Configure?.Invoke(input);
        NativeStyle.ApplyCss(input, option.Property.Style);
        NativeStyle.ApplyClasses(input, (option.Property.Class ?? "").Split(' ', StringSplitOptions.RemoveEmptyEntries));
        return PlaceChoiceLabel(input, label, option.Property.LabelPlacement);
    }

    private UIElement BuildSwitch(TeEditNode node, ITeSwitchOption option)
    {
        var label = new TextBlock().Text(option.Property.Label ?? node.Value?.ToString() ?? "").CenterVertical();
        var input = new ToggleSwitch().IsChecked(option.Converter.ToField(node.Value!))
            .IsEnabled(!option.Property.Disabled && !option.Property.ReadOnly)
            .OnCheckedChanged(async value => { await _session.SetFieldValueAsync(node, value); label.Text = option.Property.Label ?? node.Value?.ToString() ?? ""; });
        ApplyChoiceAppearance(input, option.Property.Color, option.Property.Size, false);
        input.WithTheme((theme, toggle) => toggle.ThumbBrush = NativeStyle.ThemeColor(option.Property.ThumbIconColor, theme.Palette));
        if (!string.IsNullOrWhiteSpace(option.Property.ThumbIcon) || !string.IsNullOrWhiteSpace(option.Property.ThumbIconUnChecked))
            input.Template = new DelegateControlTemplate<ToggleSwitch>((owner, _) =>
            {
                var thumb = new Border().Size(24).CornerRadius(12).Padding(3)
                    .Bind(Border.HorizontalAlignmentProperty, owner, ToggleSwitch.IsCheckedProperty, value => value ? HorizontalAlignment.Right : HorizontalAlignment.Left)
                    .WithTheme((theme, border) => border.Background = theme.Palette.ControlBackground);
                thumb.Child = new ContentControl().Bind(ContentControl.ContentProperty, owner, ToggleSwitch.IsCheckedProperty,
                    value => ChoiceGlyph(value || string.IsNullOrWhiteSpace(option.Property.ThumbIconUnChecked) ? option.Property.ThumbIcon : option.Property.ThumbIconUnChecked,
                        Options.Size.Small, option.Property.ThumbIconColor));
                return new Border().Width(48).Height(28).Padding(2).CornerRadius(14).Child(thumb)
                    .WithTheme((theme, track) => track.Bind(Border.BackgroundProperty, owner, ToggleSwitch.IsCheckedProperty,
                        value => NativeStyle.ThemeColor(value ? option.Property.Color : option.Property.UncheckedColor, theme.Palette)));
            });
        option.Property.Configure?.Invoke(input);
        return PlaceChoiceLabel(input, label, option.Property.LabelPlacement);
    }

    private UIElement BuildAutocomplete(TeEditNode node, ITeAutocompleteOption option)
    {
        var property = option.Property;
        var all = option.Items.ToArray();
        var selected = all.FirstOrDefault(item => Equals(item.Value, node.Value));
        var input = new TextBox().Text(selected is null ? node.Value?.ToString() ?? "" : option.StringConverter(selected))
            .Placeholder(property.Placeholder ?? "").IsReadOnly(property.ReadOnly).IsEnabled(!property.Disabled);
        var suggestions = new ListBox().Height(Math.Min(property.MaxHeight, 160));
        var popupContent = new StackPanel().Vertical().Spacing(3);
        var host = new StackPanel().Vertical().Spacing(3);
        var popup = new Popup { StaysOpen = false };
        _popups.Add(popup);
        var progress = property.ProgressIndicatorTemplate is { } progressTemplate ? RequireVisual(progressTemplate()) : new ProgressBar().IsIndeterminate().Height(3);
        progress.IsVisible = false;
        var popupProgress = property.ProgressIndicatorInPopoverTemplate is { } popupProgressTemplate ? RequireVisual(popupProgressTemplate()) : null;
        if (popupProgress is not null) popupProgress.IsVisible = false;
        host.Children(progress);
        var before = property.BeforeItemsTemplate is { } beforeTemplate ? RequireVisual(beforeTemplate()) : null;
        var after = property.AfterItemsTemplate is { } afterTemplate ? RequireVisual(afterTemplate()) : null;
        if (before is not null) popupContent.Children(before);
        popupContent.Children(suggestions);
        if (popupProgress is not null) popupContent.Children(popupProgress);
        var empty = property.NoItemsTemplate is { } noItems ? RequireVisual(noItems()) : new TextBlock().Text("No matching items");
        empty.IsVisible = false; popupContent.Children(empty);
        var more = property.MoreItemsTemplate is { } moreItems ? RequireVisual(moreItems()) : null;
        if (more is not null) { more.IsVisible = false; popupContent.Children(more); }
        if (after is not null) popupContent.Children(after);
        var popupBody = new Border().Padding(property.Dense ? 3 : 6).BorderThickness(1).Child(popupContent)
            .WithTheme((theme, border) => { border.Background = theme.Palette.ControlBackground; border.BorderBrush = theme.Palette.ControlBorder; });
        NativeStyle.ApplyClasses(popupBody, (property.PopoverClass ?? "").Split(' ', StringSplitOptions.RemoveEmptyEntries));
        NativeStyle.ApplyClasses(suggestions, (property.ListClass ?? "").Split(' ', StringSplitOptions.RemoveEmptyEntries));
        popup.Content = popupBody;
        var toggle = new Button().Content(property.OpenIcon).IsEnabled(!TeEditSession.IsReadOnly(node));
        var inputRow = new DockPanel().Spacing(3).Children(toggle.DockRight());
        if (property.Clearable) inputRow.Children(new Button().Content(property.ClearIcon ?? "×").DockRight().IsEnabled(!TeEditSession.IsReadOnly(node))
            .OnClick(async () => { input.Text = ""; popup.Close(); await _session.SetValueAsync(node, null); }));
        inputRow.Children(input);
        host.Children(inputRow);
        CancellationTokenSource? pending = null;
        ITeAutocompleteItem[] shown = [];
        var suppressFocusSearch = false;
        void OpenPopup()
        {
            if (_disposed || TeEditSession.IsReadOnly(node) || input.FindVisualRoot() is not Window window) return;
            popupBody.Width = Math.Max(input.ActualWidth + toggle.ActualWidth, 180);
            var anchor = input.TransformToAncestor(window).TransformBounds(new Rect(0, 0, input.ActualWidth, input.ActualHeight));
            popup.ShowAt(input, anchor, property.AnchorOrigin is (Origin.TopLeft or Origin.TopCenter or Origin.TopRight) && property.TransformOrigin is (Origin.BottomLeft or Origin.BottomCenter or Origin.BottomRight)
                ? PopupAnchorSide.Above : PopupAnchorSide.Below);
            toggle.Content(property.CloseIcon);
        }
        void ShowMatches(string text)
        {
            if (text.Length < property.MinCharacters) { popup.Close(); return; }
            var matches = all.Where(item => text.Length == 0 || option.CustomSearchFilter(item, text)).ToArray();
            shown = property.MaxItems is > 0 ? matches.Take(property.MaxItems.Value).ToArray() : matches;
            suggestions.Items(shown, option.StringConverter);
            suggestions.SelectedIndex = -1;
            suggestions.Height = Math.Max(30, Math.Min(Math.Max(30, property.MaxHeight), shown.Length * 30));
            suggestions.IsVisible = shown.Length > 0;
            empty.IsVisible = shown.Length == 0;
            if (more is not null) more.IsVisible = shown.Length < matches.Length;
            if (before is not null) before.IsVisible = shown.Length > 0;
            if (after is not null) after.IsVisible = shown.Length > 0;
            OpenPopup();
        }
        async Task Accept(ITeAutocompleteItem item)
        {
            pending?.Cancel();
            if (property.CoerceText) input.Text = option.StringConverter(item);
            await _session.SetValueAsync(node, item.Value);
            popup.Close();
            if (!input.IsFocused) { suppressFocusSearch = true; input.Focus(); }
        }
        popup.Closed += (_, _) => toggle.Content(property.OpenIcon);
        input.OnTextChanged(async text =>
        {
            try
            {
                pending?.Cancel(); pending = new(); _pending.Add(pending);
                if (property.ShowProgressIndicator) { progress.IsVisible = true; if (popupProgress is not null) popupProgress.IsVisible = true; }
                if (property.DebounceInterval > 0) await Task.Delay(property.DebounceInterval, pending.Token);
                if (_disposed) return;
                progress.IsVisible = false; if (popupProgress is not null) popupProgress.IsVisible = false;
                if (text.Length == 0 && property.ResetValueOnEmptyText) await _session.SetValueAsync(node, null);
                else if (property.CoerceValue && node.DeclaredType == typeof(string)) await _session.SetValueAsync(node, text);
                ShowMatches(text);
            }
            catch (OperationCanceledException) { }
            catch (Exception exception) { node.Errors = [exception.Message]; RefreshErrors(); }
        });
        suggestions.OnMouseUp(async args =>
        {
            if (args.Button == MouseButton.Left && suggestions.SelectedItem is ITeAutocompleteItem item) await Accept(item);
        });
        input.OnKeyDown(async args =>
        {
            if (args.Key == Key.Escape) { pending?.Cancel(); popup.Close(); args.Handled = true; }
            else if (args.Key is Key.Down or Key.Up)
            {
                if (!popup.IsOpen) ShowMatches(property.Strict ? input.Text : "");
                if (shown.Length > 0) suggestions.SelectedIndex = Math.Clamp(suggestions.SelectedIndex + (args.Key == Key.Down ? 1 : -1), 0, shown.Length - 1);
                args.Handled = true;
            }
            else if (args.Key == Key.Enter && popup.IsOpen && suggestions.SelectedItem is ITeAutocompleteItem item)
            { await Accept(item); args.Handled = true; }
        });
        suggestions.OnKeyDown(async args =>
        {
            if (args.Key == Key.Enter && suggestions.SelectedItem is ITeAutocompleteItem item) { await Accept(item); args.Handled = true; }
            else if (args.Key == Key.Escape) { popup.Close(); input.Focus(); args.Handled = true; }
        });
        input.OnGotFocus(() =>
        {
            if (suppressFocusSearch) { suppressFocusSearch = false; return; }
            ShowMatches(property.Strict ? input.Text : "");
        });
        input.OnLostFocus(() => Application.Current?.Dispatcher?.BeginInvoke(() =>
        { if (!popupBody.IsFocusWithin && !input.IsFocused) { pending?.Cancel(); popup.Close(); } }));
        toggle.OnClick(() => { if (popup.IsOpen) popup.Close(); else ShowMatches(property.Strict ? input.Text : ""); });
        ApplyInputAppearance(input, property);
        return host;
    }

    private UIElement BuildEnum(TeEditNode node)
    {
        var items = Enum.GetValues(Nullable.GetUnderlyingType(node.DeclaredType) ?? node.DeclaredType).Cast<object>().ToArray();
        return new ComboBox().Items(items, item => item.ToString() ?? "").SelectedIndex(Array.FindIndex(items, value => Equals(value, node.Value)))
            .OnSelectionChanged(async value => await _session.SetValueAsync(node, value));
    }

    private UIElement BuildList(TeEditNode node)
    {
        var panel = new StackPanel().Vertical().Spacing(6);
        var title = node.Option is ITeObjectListEditorOption objectList ? objectList.Title : node.Label;
        if (!string.IsNullOrWhiteSpace(title)) panel.Children(new TextBlock().Text(title).Bold());
        if (node.Option is ITeObjectListEditorOption) panel.Children(BuildObjectList(node));
        else
        {
            for (var index = 0; index < node.Children.Count; index++)
            {
                var row = new DockPanel().Spacing(6);
                if (node.Option is ITeListEditorOption { ShowNumber: true }) row.Children(new TextBlock().Text((index + 1).ToString()).Width(40).DockLeft());
                var remove = RemoveAction(node);
                if (remove is not null)
                {
                    var capturedIndex = index;
                    row.Children(CreateListAction(remove, node.Children[index].Value, () => _session.RemoveItemAsync(node, capturedIndex), "×").DockRight());
                }
                row.Children(BuildNode(node.Children[index])); panel.Children(DecorateCell(row, index));
            }
        }
        var add = node.Option switch { ITeListEditorOption option => option.AddItemAction, ITeObjectListEditorOption option => option.AddItemAction, _ => null };
        if (add is not null) panel.Children(CreateListAction(add, node.List, () => _session.AddItemAsync(node), "Add item"));
        return panel;
    }

    private UIElement BuildObjectList(TeEditNode node)
    {
        var columns = node.Children.SelectMany(row => row.Children).Select(child => child.Label).Distinct(StringComparer.Ordinal).ToArray();
        var canRemove = RemoveAction(node) is not null;
        if (columns.Length == 0) return new TextBlock().Text(node.Children.Count == 0 ? "No records" : "No editable members");
        var grid = new Grid().Columns(string.Join(',', columns.Select(_ => "*").Concat(canRemove ? new[] { "Auto" } : [])))
            .Rows(string.Join(',', Enumerable.Repeat("Auto", node.Children.Count + 1))).Spacing(6);
        for (var column = 0; column < columns.Length; column++) grid.Children(new TextBlock().Text(columns[column]).Bold().Column(column));
        for (var index = 0; index < node.Children.Count; index++)
        {
            var row = node.Children[index];
            for (var column = 0; column < columns.Length; column++)
            {
                var cell = row.Children.FirstOrDefault(child => child.Label == columns[column]);
                if (cell is not null) grid.Children(DecorateCell(BuildNode(cell), index).Row(index + 1).Column(column));
            }
            if (canRemove)
            {
                var capturedIndex = index;
                grid.Children(CreateListAction(RemoveAction(node)!, node.Children[index].Value, () => _session.RemoveItemAsync(node, capturedIndex), "×")
                    .Row(index + 1).Column(columns.Length));
            }
        }
        return new ScrollViewer().Content(grid);
    }

    private static ITvAction? RemoveAction(TeEditNode node) => node.Option switch
    { ITeListEditorOption option => option.RemoveItemAction, ITeObjectListEditorOption option => option.RemoveItemAction, _ => null };

    private UIElement BuildToolbarButton(ITvButton option)
    {
        var item = _session.Root.List is { } list
            ? list.Cast<object?>().Select((value, index) => (index, obj: value)).ToArray() : _session.Data;
        var button = new TvButton(item, option);
        button.Error += exception => { _session.Root.Errors = [exception.Message]; RefreshErrors(); };
        return button;
    }

    private TvButton CreateListAction(ITvAction source, object? item, Func<Task> action, string fallback)
    {
        TvAction<object> wrapper = source switch
        {
            ITvOneClickAction oneClick => new TvOneClickAction<object> { Action = _ => action(), Reusable = oneClick.Reusable, ProgressStyle = oneClick.ProgressStyle },
            ITvDoubleClickAction doubleClick => new TvDoubleClickAction<object> { Action = _ => action(), ResetDelay = doubleClick.ResetDelay, SecondStyle = doubleClick.SecondStyle },
            ITvPopupAction popup => new TvPopupAction<object>
            {
                Action = _ => action(), PopupTitle = _ => popup.PopupTitle(item!), PopupContent = _ => popup.PopupContent(item!),
                PopupStyle = popup.PopupStyle, InnerButtonOptions = popup.InnerButtonOptions
            },
            _ => new TvAction<object> { Action = _ => action() }
        };
        wrapper.Label = string.IsNullOrWhiteSpace(source.Label) ? fallback : source.Label;
        wrapper.LabelAfterClick = source.LabelAfterClick;
        wrapper.Style = source.Style;
        wrapper.TooltipOption = source.TooltipOption;
        wrapper.Condition = (_, depth) => source.Condition(item, depth);
        return new TvButton(new object(), wrapper);
    }

    private void RefreshErrors()
    {
        if (_disposed) return;
        foreach (var (node, label) in _errorLabels) label.Text = string.Join("\n", node.Errors);
    }

    private UIElement DecorateCell(UIElement child, int index)
    {
        var settings = _session.Options.Style;
        var normalBackground = Aprillz.MewUI.Color.Transparent;
        var hoverBackground = Aprillz.MewUI.Color.Transparent;
        var border = new Border().Padding(settings.SuperDense ? 3 : settings.Dense ? 6 : 10).BorderThickness(settings.Bordered ? 1 : 0).Child(child);
        border.WithTheme((theme, cell) =>
        {
            cell.BorderBrush = theme.Palette.ControlBorder;
            var background = settings.Striped && index % 2 == 1 ? theme.Palette.ControlBackground : theme.Palette.ContainerBackground;
            normalBackground = background;
            hoverBackground = theme.Palette.ButtonHoverBackground;
            cell.Background = background;
        });
        if (settings.Hover)
        {
            border.MouseEnter += () => border.Background = hoverBackground;
            border.MouseLeave += () => border.Background = normalBackground;
        }
        return border;
    }

    private static void ApplyInputAppearance(Control input, TeBaseInputProperty property)
    {
        input.Tag = property.Tag;
        input.FontSize = property.Typo switch { Typo.h1 => 32, Typo.h2 => 28, Typo.h3 => 24, Typo.h4 => 22, Typo.h5 => 20, Typo.h6 => 18, Typo.caption => 11, _ => 14 };
        input.Margin = new Thickness(property.Margin == Options.Property.Margin.Dense ? 2 : property.Margin == Options.Property.Margin.Normal ? 6 : 0);
        input.WithTheme((theme, control) =>
        {
            control.BorderThickness = property.Variant == Variant.Outlined || property.Underline ? 1 : 0;
            control.Background = property.Variant == Variant.Filled ? theme.Palette.ControlBackground : theme.Palette.ContainerBackground;
        });
        NativeStyle.ApplyClasses(input, (property.Class ?? "").Split(' ', StringSplitOptions.RemoveEmptyEntries));
        NativeStyle.ApplyCss(input, property.Style);
        property.Configure?.Invoke(input);
    }

    private static void ApplyChoiceAppearance(Control input, Options.Color color, Options.Size size, bool dense)
    {
        input.FontSize = size switch { Options.Size.Small => 12, Options.Size.Large => 18, _ => 14 };
        input.Padding = new Thickness(dense ? 2 : 5);
        input.WithTheme((theme, control) => control.Foreground = NativeStyle.ThemeColor(color, theme.Palette));
    }

    private static UIElement ChoiceGlyph(string? value, Options.Size size, Options.Color color)
    {
        var length = size switch { Options.Size.Small => 16, Options.Size.Large => 26, _ => 20 };
        var path = value;
        if (value?.TrimStart().StartsWith('<') == true)
        {
            try
            {
                var svg = System.Xml.Linq.XElement.Parse("<svg>" + value + "</svg>");
                path = string.Join(' ', svg.Descendants().Where(element => element.Name.LocalName == "path").Select(element => (string?)element.Attribute("d")));
            }
            catch (System.Xml.XmlException) { path = null; }
        }
        if (!string.IsNullOrWhiteSpace(path) && path.TrimStart().StartsWith('M'))
            return new PathShape().Data(path).Stretch(Stretch.Uniform).Size(length).CenterVertical()
                .WithTheme((theme, shape) => shape.Fill(NativeStyle.ThemeColor(color, theme.Palette)));
        return new TextBlock().Text(value ?? "").FontSize(length).CenterVertical()
            .WithTheme((theme, text) => text.Foreground(NativeStyle.ThemeColor(color, theme.Palette)));
    }

    private static StackPanel PlaceChoiceLabel(UIElement input, UIElement label, Placement placement)
    {
        var panel = new StackPanel().Spacing(5);
        if (placement is Placement.Top or Placement.Bottom) panel.Vertical(); else panel.Horizontal();
        return placement is Placement.Start or Placement.Left or Placement.Top ? panel.Children(label, input) : panel.Children(input, label);
    }

    private static UIElement RequireVisual(Element element) => element as UIElement ?? throw new ArgumentException("A custom editor must return a MewUI UIElement.");

    public void Dispose()
    {
        _disposed = true;
        _session.ValidationChanged -= RefreshErrors;
        foreach (var pending in _pending) { pending.Cancel(); pending.Dispose(); }
        _pending.Clear();
        foreach (var popup in _popups) popup.Close();
        _popups.Clear();
    }
}
