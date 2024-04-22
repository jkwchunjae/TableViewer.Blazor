using TableViewerBlazor.Options;

namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeNumericField : TeEditorBase
{
    [Parameter] public ITeNumericFieldOption NumericFieldOption { get; set; } = default!;

    private async Task OnValueChanged(object? value)
    {
        Data = value;
        await DataChanged.InvokeAsync(value);
    }

    private IEnumerable<string> NumericFieldValidation<T>(T value)
    {
        if (NumericFieldOption?.Validations != null)
        {
            foreach (var validation in NumericFieldOption.Validations)
            {
                if (!validation.Func(value!))
                {
                    yield return validation.Message;
                }
            }
        }
    }
}
