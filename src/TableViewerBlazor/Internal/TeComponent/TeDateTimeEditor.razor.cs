using Microsoft.JSInterop;
using TableViewerBlazor.Options;

namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeDateTimeEditor : TeEditorBase
{
    [Inject] DateTimeService DateTimeService { get; set; } = default!;
    [Parameter] public DateTime? DateTimeValue { get; set; }
    [Parameter] public ITeDateTimeOption DateTimeOption { get; set; } = default!;

    DateTime dateForVisible => DateTimeValue?.Date ?? DateTime.Today;
    TimeSpan timeForVisible => DateTimeValue?.TimeOfDay ?? TimeSpan.Zero;
    bool UseDate => DateTimeOption.UseDate;
    bool UseTime => DateTimeOption.UseTime;

    private async Task OnDateChanged(DateTime? value)
    {
        if (dateForVisible.Date == value?.Date)
            return;

        var nextValue = value!.Value.Date + DateTimeValue!.Value.TimeOfDay;
        await OnDateTimeChanged(nextValue);
    }

    private async Task OnTimeChanged(TimeSpan? value)
    {
        if (timeForVisible == value)
            return;

        var nextValue = DateTimeValue!.Value.Date + value!.Value;
        await OnDateTimeChanged(nextValue);
    }

    private async Task OnDateTimeChanged(DateTime value)
    {
        DateTimeValue = value;
        Data = value;
        await DataChanged.InvokeAsync(value);
    }

    private async Task<IEnumerable<string>> DateTimeValidation<T>(T value)
    {
        var errors = new List<string>();
        if (DateTimeOption?.Validations != null)
        {
            // Do not use Task.WhenAll
            foreach (var validation in DateTimeOption.Validations)
            {
                try
                {
                    if (!await validation.Func(value!))
                    {
                        errors.Add(validation.Message);
                    }
                }
                catch (Exception ex)
                {
                    errors.Add(ex.Message);
                }
            }
        }
        return errors;
    }
}