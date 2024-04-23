using Microsoft.AspNetCore.Components.Web;

namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeTextField : TeEditorBase
{
    [Parameter] public ITeTextFieldOption? TextFieldOption { get; set; }

    private async Task OnValueChanged(object? value)
    {
        Data = value;
        if (TextFieldOption?.Event?.ValueChanged != null)
        {
            try
            {
                await TextFieldOption.Event.ValueChanged.Invoke(value);
            }
            catch
            {
            }
        }
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

    public async Task OnKeyDown(KeyboardEventArgs args)
    {
        if (TextFieldOption?.Event?.OnKeyDown != null)
        {
            await TextFieldOption.Event.OnKeyDown.Invoke(args);
        }
    }

    public async Task OnKeyUp(KeyboardEventArgs args)
    {
        if (TextFieldOption?.Event?.OnKeyUp != null)
        {
            await TextFieldOption.Event.OnKeyUp.Invoke(args);
        }
    }
}
