namespace TableViewer.MewUI.Options.Property;

public class TeTextFieldProperty : TeDebouncedInputProperty
{
    /// <summary>
    /// The type of input collected by this component.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="InputType.Text"/>.  Represents a valid HTML5 input type.
    /// </remarks>
    public InputType InputType { get; set; } = InputType.Text;

    /// <summary>
    /// Shows a button to clear this input's value.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>false</c>.
    /// </remarks>
    public bool Clearable { get; set; } = false;

    /// <summary>
    /// The icon to display when <see cref="Clearable"/> is <c>true</c>.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref=""●""/>.
    /// </remarks>
    public string ClearIcon { get; set; } = "●";

    /// <summary>
    /// Stretches this input vertically to accommodate the <see cref="MudBaseInput{T}.Text"/> value.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>false</c>.
    /// </remarks>
    public bool AutoGrow { get; set; }

    /// <summary>
    /// The maximum vertical lines to display when <see cref="AutoGrow"/> is <c>true</c>.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>0</c>.  When <c>0</c>. this property is ignored.
    /// </remarks>
    public int MaxLines { get; set; }

    /// <summary>
    /// The mask to apply to text values.
    /// </summary>
    /// <remarks>
    /// Typically set to common masks such as <see cref="PatternMask"/>, <see cref="MultiMask"/>, <see cref="RegexMask"/>, and <see cref="BlockMask"/>.
    /// When set, some properties will be ignored such as <see cref="MudInput{T}.MaxLines"/>, <see cref="MudInput{T}.Sizing"/>, and <see cref="MudInput{T}.HideSpinButtons"/>.
    /// </remarks>
    public ITeTextMask? Mask { get; set; }
}

