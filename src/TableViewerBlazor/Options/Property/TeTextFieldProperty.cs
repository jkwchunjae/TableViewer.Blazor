namespace TableViewerBlazor.Options.Property;

public class TeTextFieldProperty : TeDebouncedInputProperty
{
    /// <summary>
    /// The type of input collected by this component.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="InputType.Text"/>.  Represents a valid HTML5 input type.
    /// </remarks>
    [Category(CategoryTypes.FormComponent.Behavior)]
    public InputType InputType { get; set; } = InputType.Text;

    /// <summary>
    /// Shows a button to clear this input's value.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>false</c>.
    /// </remarks>
    [Category(CategoryTypes.FormComponent.Behavior)]
    public bool Clearable { get; set; } = false;

    /// <summary>
    /// The icon to display when <see cref="Clearable"/> is <c>true</c>.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="Icons.Material.Filled.Clear"/>.
    /// </remarks>
    [Category(CategoryTypes.FormComponent.Appearance)]
    public string ClearIcon { get; set; } = Icons.Material.Filled.Clear;

    /// <summary>
    /// Stretches this input vertically to accommodate the <see cref="MudBaseInput{T}.Text"/> value.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>false</c>.
    /// </remarks>
    [Category(CategoryTypes.General.Behavior)]
    public bool AutoGrow { get; set; }

    /// <summary>
    /// The maximum vertical lines to display when <see cref="AutoGrow"/> is <c>true</c>.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>0</c>.  When <c>0</c>. this property is ignored.
    /// </remarks>
    [Category(CategoryTypes.General.Behavior)]
    public int MaxLines { get; set; }

    /// <summary>
    /// The mask to apply to text values.
    /// </summary>
    /// <remarks>
    /// Typically set to common masks such as <see cref="PatternMask"/>, <see cref="MultiMask"/>, <see cref="RegexMask"/>, and <see cref="BlockMask"/>.
    /// When set, some properties will be ignored such as <see cref="MudInput{T}.MaxLines"/>, <see cref="MudInput{T}.AutoGrow"/>, and <see cref="MudInput{T}.HideSpinButtons"/>.
    /// </remarks>
    [Category(CategoryTypes.General.Data)]
    public IMask? Mask { get; set; }
}
