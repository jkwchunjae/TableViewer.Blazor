namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeSwitch : TeEditorBase
{
    [Parameter] public ITeSwitchOption SwitchOption { get; set; } = default!;

    private bool isChecked = false;
    private string switchLabel = string.Empty;
    private string thumIcon = string.Empty;

    protected override void OnInitialized()
    {
        isChecked = SwitchOption.Converter.ToField(Data!);
        switchLabel = GetLabel();
        thumIcon = GetThumbIcon();
    }

    private async Task OnValueChanged(bool value)
    {
        isChecked = value;
        Data = SwitchOption.Converter.FromField(value);
        await DataChanged.InvokeAsync(Data);
        switchLabel = GetLabel();
        thumIcon = GetThumbIcon();
    }

    private string GetLabel()
    {
        if (SwitchOption.Property.Label != null)
        {
            return SwitchOption.Property.Label;
        }
        else
        {
            return Data?.ToString() ?? "";
        }
    }

    private string GetThumbIcon()
    {
        if (isChecked)
        {
            return SwitchOption.Property.ThumbIcon;
        }
        else
        {
            if (string.IsNullOrEmpty(SwitchOption.Property.ThumbIconUnChecked))
            {
                return SwitchOption.Property.ThumbIcon;
            }
            else
            {
                return SwitchOption.Property.ThumbIconUnChecked;
            };
        }
    }
}
