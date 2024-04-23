namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeTextField : TeEditorBase
{
    [Parameter] public ITeTextFieldOption? TextFieldOption { get; set; }

    private async Task OnValueChanged(object? value)
    {
        Data = value;
        await DataChanged.InvokeAsync(value);
    }

    private async Task<IEnumerable<string>> TextFieldValidation<T>(T value)
    {
        var errors = new List<string>();
        if (TextFieldOption?.Validations != null)
        {
            // Do not use Task.WhenAll
            foreach (var validation in TextFieldOption.Validations)
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
