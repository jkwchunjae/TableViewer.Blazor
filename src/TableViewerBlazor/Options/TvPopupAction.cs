namespace TableViewerBlazor.Options;

public interface ITvPopupAction : ITvAction
{
    Func<object, string> PopupTitle { get; }
    Func<object, string> PopupContent { get; }
    DialogOptions PopupStyle { get; }
    PopupInnerButtonOptions<object> PopupInnerButtonOptions { get; }
}

public class TvPopupAction<T> : TvAction<T>, ITvPopupAction
{
    public Func<T, string> PopupTitle { get; set; } = _ => string.Empty;
    public Func<T, string> PopupContent { get; set; } = _ => string.Empty;
    public DialogOptions PopupStyle { get; set; } = new()
    { 
        FullWidth = true,
        CloseOnEscapeKey = true,
        NoHeader = false,
    };
    public PopupInnerButtonOptions<T> PopupInnerButtonOptions { get; set; } = new();

    Func<object, string> ITvPopupAction.PopupTitle => o => o is T t ? PopupTitle(t) : string.Empty;

    Func<object, string> ITvPopupAction.PopupContent => o => o is T t ? PopupContent(t) : string.Empty;

    PopupInnerButtonOptions<object> ITvPopupAction.PopupInnerButtonOptions => new PopupInnerButtonOptions<object>
    {
        CloseButton = new TvAction<object>
        {
            Action = o => o is T t ? PopupInnerButtonOptions.CloseButton.Action(t) : Task.CompletedTask,
            Label = PopupInnerButtonOptions.CloseButton.Label,
            Condition = (o, i) => o is T t? PopupInnerButtonOptions.CloseButton.Condition(t, i) : false,
            Style = PopupInnerButtonOptions.CloseButton.Style,
        },
        Buttons = PopupInnerButtonOptions.Buttons
            .Select(button => new TvAction<object>
            {
                Action = o => o is T t ? button.Action(t) : Task.CompletedTask,
                Label = button.Label,
                Condition = (o, i) => o is T t ? button.Condition(t, i) : false,
                Style = button.Style,
                LabelAfterClick = button.LabelAfterClick,
            }).ToList()
    };
}

public class PopupInnerButtonOptions<T>
{
    public TvAction<T> CloseButton { get; set; } = new TvAction<T>
    {
        Action = _ => Task.CompletedTask,
        Label = "Close",
        Condition = (_, _) => true,
        Style =
        {
            Color = Color.Default,
            Variant = Variant.Outlined,
        }
    };
    public List<TvAction<T>> Buttons { get; set; } = [];
}