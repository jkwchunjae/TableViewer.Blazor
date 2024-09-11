namespace TableViewerBlazor.Options;

public interface ITvPopupAction : ITvAction
{
    Func<object, string> PopupTitle { get; }
    Func<object, string> PopupContent { get; }
    DialogOptions PopupStyle { get; }
    InnerButtonsOptions InnerButtonOptions { get; }
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
    public InnerButtonsOptions InnerButtonOptions { get; set; } = new();

    Func<object, string> ITvPopupAction.PopupTitle => o => o is T t ? PopupTitle(t) : "제목을 입력하세요";

    Func<object, string> ITvPopupAction.PopupContent => o => o is T t ? PopupContent(t) : "내용을 입력하세요";
}

public class InnerButtonsOptions
{
    public string CloseLabel { get; set; } = "Close";
    public string ConfirmLabel { get; set; } = "Confirm";
    public TvButtonStyle CloseButtonStyle { get; set; } = new TvButtonStyle()
    {
        Color = Color.Default,
        Variant = Variant.Filled,
    };
    public TvButtonStyle ConfirmButtonStyle { get; set; } = new TvButtonStyle()
    {
        Color = Color.Primary,
        Variant = Variant.Filled,
    };
}