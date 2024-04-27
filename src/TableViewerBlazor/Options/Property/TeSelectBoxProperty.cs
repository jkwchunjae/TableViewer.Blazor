using System.Globalization;

namespace TableViewerBlazor.Options.Property;

public class TeSelectBoxProperty : ITeSelectBoxProperty
{
    public string? OuterClass { get; set; }
    public string? InputClass { get; set; }
    public string? PopoverClass { get; set; }
    public string? ListClass { get; set; }
    public bool Dense { get; set; }
    public string? OpenIcon { get; set; }
    public string? CloseIcon { get; set; }
    public bool SelectAll { get; set; }
    public string? SelectAllText { get; set; }
    public Func<List<string>, string>? MultiSelectionTextFunc { get; set; }
    public string? Delimiter { get; set; }
    public IEnumerable<object>? SelectedValues { get; set; }
    public IEqualityComparer<object>? Comparer { get; set; }
    public Func<object, string>? ToStringFunc { get; set; }
    public bool MultiSelection { get; set; }
    public int MaxHeight { get; set; }
    public Origin AnchorOrigin { get; set; }
    public Origin TransformOrigin { get; set; }
    public bool Strict { get; set; }
    public bool Clearable { get; set; }
    public bool LockScroll { get; set; }
    public string? CheckedIcon { get; set; }
    public string? UncheckedIcon { get; set; }
    public string? IndeterminateIcon { get; set; }
    public bool Disabled { get; set; }
    public bool ReadOnly { get; set; }
    public bool FullWidth { get; set; }
    public bool Immediate { get; set; }
    public bool Underline { get; set; } = true;
    public string? HelperText { get; set; }
    public bool HelperTextOnFocus { get; set; }
    public string? AdornmentIcon { get; set; }
    public string? AdornmentText { get; set; }
    public Adornment Adornment { get; set; }
    public bool OnlyValidateIfDirty { get; set; }
    public Color AdornmentColor { get; set; }
    public string? AdornmentAriaLabel { get; set; }
    public Size IconSize { get; set; }
    public Variant Variant { get; set; }
    public Margin Margin { get; set; }
    public string? Placeholder { get; set; }
    public int? Counter { get; set; }
    public int MaxLength { get; set; }
    public string? Label { get; set; }
    public bool AutoFocus { get; set; }
    public int Lines { get; set; }
    public string? Text { get; set; }
    public bool TextUpdateSuppression { get; set; }
    public InputMode InputMode { get; set; }
    public string? Pattern { get; set; }
    public bool ShrinkLabel { get; set; }
    public string? Format { get; set; }
    public string? InputId { get; set; }
    public bool Required { get; set; }
    public CultureInfo? Culture { get; set; }
}
public class TeSelectBoxProperty<T> : ITeSelectBoxProperty
{
    public string? OuterClass { get; set; }
    public string? InputClass { get; set; }
    public string? PopoverClass { get; set; }
    public string? ListClass { get; set; }
    public bool Dense { get; set; }
    public string? OpenIcon { get; set; }
    public string? CloseIcon { get; set; }
    public bool SelectAll { get; set; }
    public string? SelectAllText { get; set; }
    public Func<List<string>, string>? MultiSelectionTextFunc { get; set; }
    public string? Delimiter { get; set; }
    public IEnumerable<T>? SelectedValues { get; set; }
    public IEqualityComparer<T>? Comparer { get; set; }
    public Func<T, string>? ToStringFunc { get; set; }
    public bool MultiSelection { get; set; }
    public int MaxHeight { get; set; }
    public Origin AnchorOrigin { get; set; }
    public Origin TransformOrigin { get; set; }
    public bool Strict { get; set; }
    public bool Clearable { get; set; }
    public bool LockScroll { get; set; }
    public string? CheckedIcon { get; set; }
    public string? UncheckedIcon { get; set; }
    public string? IndeterminateIcon { get; set; }
    public bool Disabled { get; set; }
    public bool ReadOnly { get; set; }
    public bool FullWidth { get; set; }
    public bool Immediate { get; set; }
    public bool Underline { get; set; } = true;
    public string? HelperText { get; set; }
    public bool HelperTextOnFocus { get; set; }
    public string? AdornmentIcon { get; set; }
    public string? AdornmentText { get; set; }
    public Adornment Adornment { get; set; }
    public bool OnlyValidateIfDirty { get; set; }
    public Color AdornmentColor { get; set; }
    public string? AdornmentAriaLabel { get; set; }
    public Size IconSize { get; set; }
    public Variant Variant { get; set; }
    public Margin Margin { get; set; }
    public string? Placeholder { get; set; }
    public int? Counter { get; set; }
    public int MaxLength { get; set; }
    public string? Label { get; set; }
    public bool AutoFocus { get; set; }
    public int Lines { get; set; }
    public string? Text { get; set; }
    public bool TextUpdateSuppression { get; set; }
    public InputMode InputMode { get; set; }
    public string? Pattern { get; set; }
    public bool ShrinkLabel { get; set; }
    public string? Format { get; set; }
    public string? InputId { get; set; }
    public bool Required { get; set; }
    public CultureInfo? Culture { get; set; }

    IEnumerable<object>? ITeSelectBoxProperty.SelectedValues =>
        SelectedValues == default ? default :
        SelectedValues.Cast<object>();
    IEqualityComparer<object>? ITeSelectBoxProperty.Comparer =>
        Comparer == default ? default :
        new ObjectComparer(Comparer);
    Func<object, string>? ITeSelectBoxProperty.ToStringFunc =>
        ToStringFunc == default ? default :
        value =>
        {
            if (value is T tValue)
            {
            return ToStringFunc(tValue);
            }
            else
            {
            return value?.ToString() ?? string.Empty;
            }
        };
    class ObjectComparer : IEqualityComparer<object>
    {
        private readonly IEqualityComparer<T> comparer;
        public ObjectComparer(IEqualityComparer<T> comparer)
        {
            this.comparer = comparer;
        }
        public new bool Equals(object? x, object? y)
        {
            if (x is T tX && y is T tY)
            {
            return comparer.Equals(tX, tY);
            }
            else
            {
            return object.Equals(x, y);
            }
        }
        public int GetHashCode(object obj)
        {
            return obj?.GetHashCode() ?? 0;
        }
    }
}