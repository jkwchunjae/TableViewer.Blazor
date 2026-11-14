namespace TableViewer.Share.Options;

/// <summary>Presentation-neutral button contract. UI projects supply their own style and tooltip types.</summary>
public interface ITvButton<TStyle, TTooltip>
{
    Func<object?, int, bool> Condition { get; }
    string Label { get; }
    TStyle Style { get; }
    TTooltip? TooltipOption { get; }
}

public interface ITvAction<TStyle, TTooltip> : ITvButton<TStyle, TTooltip>
{
    Func<object?, Task> Action { get; }
    string LabelAfterClick { get; }
}

/// <summary>Shared typed action dispatch, labels and appearance configuration.</summary>
public class TvAction<T, TStyle, TTooltip> : ITvAction<TStyle, TTooltip>
    where TStyle : new()
{
    public Func<T, int, bool> Condition { get; set; } = (_, _) => true;
    public required Func<T, Task> Action { get; set; }
    public string Label { get; set; } = "ACTION";
    public string LabelAfterClick { get; set; } = "DONE";
    public TStyle Style { get; set; } = new();
    public TTooltip? TooltipOption { get; set; }

    Func<object?, int, bool> ITvButton<TStyle, TTooltip>.Condition => (item, depth) => item is T value && Condition(value, depth);
    Func<object?, Task> ITvAction<TStyle, TTooltip>.Action => item => item is T value ? Action(value) : Task.CompletedTask;
}

public interface ITvLink<TStyle, TTooltip> : ITvButton<TStyle, TTooltip>
{
    string Target { get; }
    Func<object?, string> Href { get; }
}

public class TvLink<T, TStyle, TTooltip> : ITvLink<TStyle, TTooltip>
    where TStyle : new()
{
    public Func<T, int, bool> Condition { get; set; } = (_, _) => true;
    public string Target { get; set; } = "_self";
    public required Func<T, string> Href { get; set; }
    public string Label { get; set; } = "LINK";
    public TStyle Style { get; set; } = new();
    public TTooltip? TooltipOption { get; set; }

    Func<object?, int, bool> ITvButton<TStyle, TTooltip>.Condition => (item, depth) => item is T value && Condition(value, depth);
    Func<object?, string> ITvLink<TStyle, TTooltip>.Href => item => item is T value ? Href(value) : string.Empty;
}

public interface ITvOneClickAction<TProgress>
{
    bool Reusable { get; }
    TProgress ProgressStyle { get; }
}

public sealed class TvOneClickOptions<TProgress>(TProgress progressStyle)
{
    public bool Reusable { get; set; }
    public TProgress ProgressStyle { get; set; } = progressStyle;
}

public interface ITvOneClickProgressStyle<TColor, TSize>
{
    TColor Color { get; }
    TSize Size { get; }
}

public class TvOneClickProgressStyle<TColor, TSize>(TColor color, TSize size) : ITvOneClickProgressStyle<TColor, TSize>
{
    public TColor Color { get; set; } = color;
    public TSize Size { get; set; } = size;
}

public interface ITvDoubleClickAction<TSecondStyle>
{
    TSecondStyle SecondStyle { get; }
    TimeSpan ResetDelay { get; }
}

public sealed class TvDoubleClickOptions<TSecondStyle> where TSecondStyle : new()
{
    public TSecondStyle SecondStyle { get; set; } = new();
    public TimeSpan ResetDelay { get; set; } = TimeSpan.FromSeconds(2);
}

public class TvSecondButtonStyle<TVariant, TColor>(TVariant variant, TColor color)
{
    public TVariant Variant { get; set; } = variant;
    public TColor Color { get; set; } = color;
    public TColor IconColor { get; set; } = color;
}

public interface ITvPopupAction<TDialogStyle, TInnerButtons>
{
    Func<object, string> PopupTitle { get; }
    Func<object, string> PopupContent { get; }
    TDialogStyle PopupStyle { get; }
    TInnerButtons InnerButtonOptions { get; }
}

public sealed class TvPopupOptions<T, TDialogStyle, TInnerButtons>(TDialogStyle dialogStyle)
    where TInnerButtons : new()
{
    public Func<T, string> PopupTitle { get; set; } = _ => string.Empty;
    public Func<T, string> PopupContent { get; set; } = _ => string.Empty;
    public TDialogStyle PopupStyle { get; set; } = dialogStyle;
    public TInnerButtons InnerButtonOptions { get; set; } = new();
    public string GetTitle(object item) => item is T value ? PopupTitle(value) : "제목을 입력하세요";
    public string GetContent(object item) => item is T value ? PopupContent(value) : "내용을 입력하세요";
}

public class InnerButtonsOptions<TStyle>(TStyle closeStyle, TStyle confirmStyle)
{
    public string CloseLabel { get; set; } = "Close";
    public string ConfirmLabel { get; set; } = "Confirm";
    public TStyle CloseButtonStyle { get; set; } = closeStyle;
    public TStyle ConfirmButtonStyle { get; set; } = confirmStyle;
}
