namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeTextEditor : TeEditorBase
{
    [Parameter] public ITeTextFieldOption? TextFieldOption { get; set; }

    private async Task OnValueChanged(object? value)
    {
        Data = value;
        await DataChanged.InvokeAsync(value);
    }

    private IEnumerable<string> TextFieldValidation<T>(T value)
    {
        if (TextFieldOption?.Validations != null)
        {
            foreach (var validation in TextFieldOption.Validations)
            {
                if (!validation.Func(value!))
                {
                    yield return validation.Message;
                }
            }
        }
    }
}
