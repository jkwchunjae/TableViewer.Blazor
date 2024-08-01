namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeCheckBox : TeEditorBase
{
    [Parameter] public ITeCheckBoxOption CheckBoxOption { get; set; } = default!;
    [Parameter] public object Parent { get; set; } = default!;

    private bool selected = false;

    public async Task OnValueChanged(bool value)
    {
        selected = value;
        await DataChanged.InvokeAsync(new { BoolValue = value });
    }

    private string GetLabel()
    {
        var labelOption = CheckBoxOption.Property?.LabelOptions;
        if (labelOption?.Condition(Parent) ?? false)
        {
            if (selected)
            {
                return labelOption.SelectedLabel(selected, Parent);
            }
            else
            {
                return labelOption.NotSelectedLabel(selected, Parent);
            }
        }
        return string.Empty;
    }
}