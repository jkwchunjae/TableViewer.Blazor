
namespace TableViewer.MewUI.Options;

public interface ITeTextFieldEvent
{
    /// <summary>
    /// Fired when the Value property changes.
    /// </summary>
    Func<object?, Task>? ValueChanged { get; }

    /// <summary>
    /// Button click event for clear button. Called after text and value has been cleared.
    /// </summary>
    Func<TePointerEventArgs, Task>? OnClearButtonClick { get; }

    /// <summary>
    /// callback to be called when the debounce interval has elapsed
    /// receives the Text as a parameter
    /// </summary>
    Func<string, Task>? OnDebounceIntervalElapsed { get; }

    /// <summary>
    /// Button click event if set and Adornment used.
    /// </summary>
    Func<TePointerEventArgs, Task>? OnAdornmentClick { get; }

    /// <summary>
    /// Fired when the element loses focus.
    /// </summary>
    Func<TeFocusEventArgs, Task>? OnBlur { get; }

    /// <summary>
    /// Fired when the element changes internally its text value.
    /// </summary>
    Func<TeInputChangedEventArgs, Task>? OnInternalInputChanged { get; }

    /// <summary>
    /// Fired on the KeyDown event.
    /// </summary>
    Func<KeyEventArgs, Task>? OnKeyDown { get; }

    /// <summary>
    /// Fired on the KeyPress event.
    /// </summary>
    Func<KeyEventArgs, Task>? OnKeyPress { get; }

    /// <summary>
    /// Fired on the KeyUp event.
    /// </summary>
    Func<KeyEventArgs, Task>? OnKeyUp { get; }
}

public class TeTextFieldEvent<T> : ITeTextFieldEvent
{
    /// <summary>
    /// Fired when the Value property changes.
    /// </summary>
    public Func<T, Task>? ValueChanged { get; set; }
    Func<object?, Task>? ITeTextFieldEvent.ValueChanged =>
        async (value) =>
        {
            if (value is T tValue)
            {
                if (ValueChanged != null)
                {
                    await ValueChanged.Invoke(tValue);
                }
            }
        };

    /// <summary>
    /// Button click event for clear button. Called after text and value has been cleared.
    /// </summary>
    public Func<TePointerEventArgs, Task>? OnClearButtonClick { get; set; }

    /// <summary>
    /// callback to be called when the debounce interval has elapsed
    /// receives the Text as a parameter
    /// </summary>
    public Func<string, Task>? OnDebounceIntervalElapsed { get; set; }

    /// <summary>
    /// Button click event if set and Adornment used.
    /// </summary>
    public Func<TePointerEventArgs, Task>? OnAdornmentClick { get; set; }

    /// <summary>
    /// Fired when the element loses focus.
    /// </summary>
    public Func<TeFocusEventArgs, Task>? OnBlur { get; set; }

    /// <summary>
    /// Fired when the element changes internally its text value.
    /// </summary>
    public Func<TeInputChangedEventArgs, Task>? OnInternalInputChanged { get; set; }

    /// <summary>
    /// Fired on the KeyDown event.
    /// </summary>
    public Func<KeyEventArgs, Task>? OnKeyDown { get; set; }

    /// <summary>
    /// Fired on the KeyPress event.
    /// </summary>
    public Func<KeyEventArgs, Task>? OnKeyPress { get; set; }

    /// <summary>
    /// Fired on the KeyUp event.
    /// </summary>
    public Func<KeyEventArgs, Task>? OnKeyUp { get; set; }
}


