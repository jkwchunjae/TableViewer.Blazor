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
        CheckBoxLabel = GetLabel();
    }

    public async Task OnValueChanged(bool value)
    {
        isChecked = value;
        Data = CheckBoxOption.Converter.FromField(value);
        CheckBoxLabel = GetLabel();
        await DataChanged.InvokeAsync(Data);
    }

    private string GetLabel()
    {
        if (!string.IsNullOrEmpty(CheckBoxOption.Property.Label))
        {
            return CheckBoxOption.Property.Label;
        }
        if (CheckBoxOption.Property.ShowLabel)
        {
            if (Data is bool boolData)
            {
                return boolData.ToString();
            }
            if (Data is string stringData)
            {
                return stringData;
            }
        }
        return string.Empty;
    }
}