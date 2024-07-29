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
}
