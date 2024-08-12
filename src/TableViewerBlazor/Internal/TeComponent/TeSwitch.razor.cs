namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeSwitch : TeEditorBase
{
    [Parameter] public ITeSwitchOption SwitchOption { get; set; } = default!;

    private bool isChecked = false;

    protected override void OnInitialized()
    {
        isChecked = SwitchOption.Converter.ToField(Data!);
    }

    private async Task OnValueChanged(bool value)
    {
        isChecked = value;
        Data = SwitchOption.Converter.FromField(value);
        await DataChanged.InvokeAsync(Data);
    }
}
