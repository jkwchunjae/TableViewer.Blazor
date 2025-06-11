using MudBlazor;

namespace TableViewerBlazor.Options;

public interface ITvHtmlViewerOption : ITvCustomOption
{
    /// <summary>
    /// HTML 컨텐츠를 가져오는 함수
    /// </summary>
    Func<object, object, string> HtmlContent { get; }

    /// <summary>
    /// HTML 컨텐츠가 없을 때 표시할 대체 텍스트
    /// </summary>
    string? FallbackText { get; }

    /// <summary>
    /// HTML 컨텐츠의 최대 높이 (픽셀)
    /// </summary>
    int? MaxHeight { get; }

    /// <summary>
    /// HTML 컨텐츠의 최대 너비 (픽셀)
    /// </summary>
    int? MaxWidth { get; }

    /// <summary>
    /// HTML 컨텐츠의 스타일
    /// </summary>
    string? Style { get; }
}

public class TvHtmlViewerOption<TParent, TValue> : ITvHtmlViewerOption
{
    public Func<TParent, TValue, string> HtmlContent { get; set; } = (_, _) => string.Empty;
    public Func<TParent, TValue, int, string, bool> Condition { get; set; } = (_, _, _, _) => true;

    Func<object, object, string> ITvHtmlViewerOption.HtmlContent => 
        (parent, data) => parent is TParent p && data is TValue t ? HtmlContent(p, t) : string.Empty;
    
    Func<object?, object, int, string, bool> ITvCustomOption.Condition => 
        (parent, data, depth, path) => parent is TParent p && data is TValue t ? Condition.Invoke(p, t, depth, path) : false;

    /// <summary>
    /// HTML 컨텐츠가 없을 때 표시할 대체 텍스트
    /// </summary>
    public string? FallbackText { get; set; }

    /// <summary>
    /// HTML 컨텐츠의 최대 높이 (픽셀)
    /// </summary>
    public int? MaxHeight { get; set; }

    /// <summary>
    /// HTML 컨텐츠의 최대 너비 (픽셀)
    /// </summary>
    public int? MaxWidth { get; set; }

    /// <summary>
    /// HTML 컨텐츠의 스타일
    /// </summary>
    public string? Style { get; set; }
} 