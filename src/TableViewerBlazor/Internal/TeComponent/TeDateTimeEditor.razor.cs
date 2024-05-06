namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeDateTimeEditor : TeEditorBase
{
    [Parameter] public DateTime? DateTimeValue { get; set; }

    private async Task OnDateTimeChanged(DateTime? value)
    {
        DateTimeValue = value;
        Data = value;
        await DataChanged.InvokeAsync(value);
    }
}