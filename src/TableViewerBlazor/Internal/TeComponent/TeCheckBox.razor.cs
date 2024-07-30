namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeCheckBox : TeEditorBase
{
    [Parameter] public ITeCheckBoxOption CheckBoxOption { get; set; } = default!;
    [Parameter] public object? Parent { get; set; }

    private bool selected = false;

    public async Task OnValueChanged(bool value)
    {
        selected = value;
        await DataChanged.InvokeAsync(value);
    }

    private string GetLabel()
    {
        var labelOption = CheckBoxOption.Property?.LabelOptions;
        if (labelOption != null)
        {
            if (Parent != null && labelOption.Condition(Parent))
            {
                return labelOption.Label(selected, Parent);
            }
            else
            {
                return string.Empty;
            }
        }
        else
        {
            return selected ? "Selected" : "Not Selected";
        }
    }
}
