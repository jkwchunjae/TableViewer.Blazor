namespace TableViewerBlazor.Internal.TvComponent;

public partial class TvStringView : TvViewBase
{
    [Parameter] public string Data { get; set; } = null!;
}
