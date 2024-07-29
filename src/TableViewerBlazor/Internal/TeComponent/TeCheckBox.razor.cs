namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeCheckBox : TeEditorBase
{
    [Parameter] public ITeCheckBoxOption CheckBoxOption { get; set; } = default!;

    public object CheckBoxLabel { get; set; } = default!;
    protected override void OnInitialized()
    {
        CheckBoxLabel = Data!;
    }

    public async Task OnValueChanged(bool isChecked)
    {
        object? copyData = default;
        if (isChecked)
        {
            copyData = CheckBoxLabel;
        }
        await DataChanged.InvokeAsync(copyData);
    }

    private string GetLabel()
    {
        if (CheckBoxOption.Property?.HideText ?? false)
        {
            return string.Empty;
        }

        if (CheckBoxOption.Property?.Label != null)
        {
            return CheckBoxOption.Property.Label;
        }
        else
        {
            return CheckBoxLabel.ToString() ?? string.Empty;
        }
    }
}
