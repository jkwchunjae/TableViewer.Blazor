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

    private async Task<IEnumerable<string>> NumericFieldValidation<T>(T value)
    {
        var errors = new List<string>();
        if (NumericFieldOption?.Validations != null)
        {
            // Do not use Task.WhenAll
            foreach (var validation in NumericFieldOption.Validations)
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
