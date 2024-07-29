namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeRadio : TeEditorBase
{
    [Parameter] public ITeRadioOption RadioOption { get; set; } = default!;

    object? CopiedData;

    protected override void OnInitialized()
    {
        CopiedData = Data;
    }
    private async Task OnValueChanged(object? value)
    {
        CopiedData = value;
        await DataChanged.InvokeAsync(value);
    }

    private async Task<IEnumerable<string>> RadioGroupValidation<T>(T value)
    {
        var errors = new List<string>();
        if (RadioOption.Validations != null)
        {
            // Do not use Task.WhenAll
            foreach (var validation in RadioOption.Validations)
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
