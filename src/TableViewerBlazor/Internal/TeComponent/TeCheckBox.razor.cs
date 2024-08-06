namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeCheckBox : TeEditorBase
{
    [Parameter] public ITeCheckBoxOption CheckBoxOption { get; set; } = default!;
    [Parameter] public object Parent { get; set; } = default!;

    private bool isChecked = false;
    private string CheckBoxLabel = string.Empty;

    protected override void OnInitialized()
    {
        isChecked = CheckBoxOption.Converter.ToField(Data!);
        CheckBoxLabel = GetLabel(isChecked);
    }

    public async Task OnValueChanged(bool value)
    {
        isChecked = value;
        CheckBoxLabel = GetLabel(isChecked);
        Data = CheckBoxOption.Converter.FromField(value);
        await DataChanged.InvokeAsync(Data);
    }

    private string GetLabel(bool isChecked)
    {
        var labelOption = CheckBoxOption.Property.LabelOptions;
        if (labelOption?.Condition(Parent) ?? false)
        {
            if (isChecked)
            {
                return labelOption.SelectedLabel(isChecked, Parent);
            }
            else
            {
                return labelOption.NotSelectedLabel(isChecked, Parent);
            }
        }
        return string.Empty;
    }
}