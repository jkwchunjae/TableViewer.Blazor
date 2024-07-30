namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeCheckBox : TeEditorBase
{
    [Parameter] public ITeCheckBoxOption CheckBoxOption { get; set; } = default!;

    public object copyData = default!;
    protected override void OnInitialized()
    {
        copyData = Data!;
    }

    public async Task OnValueChanged(bool isChecked)
    {
        object? valueChanged = default;
        if (isChecked)
        {
            valueChanged = copyData;
        }
        await DataChanged.InvokeAsync(valueChanged);
    }

    private string GetLabel()
    {
        var option = CheckBoxOption.Property?.LabelOptions;
        if (option != null)
        {
            if (option.Condition(copyData))
            {
                return option.Label(copyData);
            }
            else
            {
                return string.Empty;
            }
            
        }
        else
        {
            return copyData.ToString() ?? string.Empty;
        }
    }
}
