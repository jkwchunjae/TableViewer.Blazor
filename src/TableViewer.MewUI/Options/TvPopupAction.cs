namespace TableViewer.MewUI.Options;

public interface ITvPopupAction : ITvAction, global::TableViewer.Share.Options.ITvPopupAction<DialogOptions, InnerButtonsOptions>
{
}

public class TvPopupAction<T> : TvAction<T>, ITvPopupAction
{
    private readonly global::TableViewer.Share.Options.TvPopupOptions<T, DialogOptions, InnerButtonsOptions> _options = new(new()
    {
        FullWidth = true,
        CloseOnEscapeKey = true,
        NoHeader = false,
    });
    public Func<T, string> PopupTitle { get => _options.PopupTitle; set => _options.PopupTitle = value; }
    public Func<T, string> PopupContent { get => _options.PopupContent; set => _options.PopupContent = value; }
    public DialogOptions PopupStyle { get => _options.PopupStyle; set => _options.PopupStyle = value; }
    public InnerButtonsOptions InnerButtonOptions { get => _options.InnerButtonOptions; set => _options.InnerButtonOptions = value; }
    Func<object, string> global::TableViewer.Share.Options.ITvPopupAction<DialogOptions, InnerButtonsOptions>.PopupTitle => _options.GetTitle;
    Func<object, string> global::TableViewer.Share.Options.ITvPopupAction<DialogOptions, InnerButtonsOptions>.PopupContent => _options.GetContent;
}

public class InnerButtonsOptions() : global::TableViewer.Share.Options.InnerButtonsOptions<TvButtonStyle>(
    new() { Color = Color.Default, Variant = Variant.Filled },
    new() { Color = Color.Primary, Variant = Variant.Filled })
{
}
