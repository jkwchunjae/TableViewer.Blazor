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
        if (CheckBoxOption.Property.Label != null)
        {
            return CheckBoxOption.Property.Label;
        }
        else
        {
            return Data?.ToString() ?? "";
        }
    }
}