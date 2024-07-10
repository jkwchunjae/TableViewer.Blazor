using Microsoft.AspNetCore.Components.Web;

namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeTextConvertableField : TeEditorBase
{
    [Parameter] public ITeTextFieldOption? TextFieldOption { get; set; }

    string tempValue = string.Empty;

    protected override void OnParametersSet()
    {
        tempValue = TextFieldOption!.Converter!.StringValue(Data!);
    }

    private async Task OnValueChanged(string value)
    {
        Data = TextFieldOption!.Converter!.FromString(value);
        if (TextFieldOption?.Event?.ValueChanged != null)
        {
            try
            {
                await TextFieldOption.Event.ValueChanged.Invoke(Data);
            }
            catch
            {
            }
        }
        await DataChanged.InvokeAsync(Data);
    }

    private async Task<IEnumerable<string>> TextFieldValidation(string value)
    {
        var data = TextFieldOption!.Converter!.FromString(value);
        var errors = new List<string>();
        if (TextFieldOption?.Validations != null)
        {
            // Do not use Task.WhenAll
            foreach (var validation in TextFieldOption.Validations)
            {
                try
                {
                    if (!await validation.Func(data!))
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

    private async Task OnClearButtonClick(MouseEventArgs args)
    {
        if (TextFieldOption?.Event?.OnClearButtonClick != null)
        {
            await TextFieldOption.Event.OnClearButtonClick.Invoke(args);
        }
    }

    public async Task OnDebounceIntervalElapsed(string args)
    {
        if (TextFieldOption?.Event?.OnDebounceIntervalElapsed != null)
        {
            await TextFieldOption.Event.OnDebounceIntervalElapsed.Invoke(args);
        }
    }

    public async Task OnAdornmentClick(MouseEventArgs args)
    {
        if (TextFieldOption?.Event?.OnAdornmentClick != null)
        {
            await TextFieldOption.Event.OnAdornmentClick.Invoke(args);
        }
    }

    public async Task OnBlur(FocusEventArgs args)
    {
        if (TextFieldOption?.Event?.OnBlur != null)
        {
            await TextFieldOption.Event.OnBlur.Invoke(args);
        }
    }

    public async Task OnInternalInputChanged(ChangeEventArgs args)
    {
        if (TextFieldOption?.Event?.OnInternalInputChanged != null)
        {
            await TextFieldOption.Event.OnInternalInputChanged.Invoke(args);
        }
    }
}
