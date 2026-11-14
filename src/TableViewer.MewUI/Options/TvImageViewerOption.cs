namespace TableViewer.MewUI.Options;

public interface ITvImageViewerOption : ITvCustomOption
{
    Func<object, object, string> ImageUrl { get; }
    Func<object, object, string> FallbackUrl { get; }

    /// <summary>
    /// The alternate text for this image.
    /// </summary>

    string? Alt { get; }

    /// <summary>
    /// Scales this image to the parent container.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>false</c>.
    /// </remarks>

    bool Fluid { get; }

    /// <summary>
    /// The height of this image, in pixels.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>null</c>.
    /// </remarks>

    int? Height { get; set; }

    /// <summary>
    /// The width of this image, in pixels.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>null</c>.
    /// </remarks>

    int? Width { get; }

    /// <summary>
    /// The size of the drop shadow for this image.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>0</c>.  
    /// </remarks>

    int Elevation { get; }

    /// <summary>
    /// Controls how this image is resized.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="ObjectFit.Fill"/>.
    /// </remarks>

    ObjectFit ObjectFit { get; }

    /// <summary>
    /// Controls how this image is positioned within its container.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="ObjectPosition.Center"/>.
    /// </remarks>

    ObjectPosition ObjectPosition { get; }


}

public class TvImageViewerOption<TParent, TValue> : ITvImageViewerOption
{
    public Func<TParent, TValue, string> ImageUrl { get; set; } = (_, _) => string.Empty;
    public Func<TParent, TValue, string> FallbackUrl { get; set; } = (_, _) => string.Empty;
    public Func<TParent, TValue, int, string, bool> Condition { get; set; } = (_, _, _, _) => true;

    Func<object, object, string> ITvImageViewerOption.ImageUrl => (parent, data) => parent is TParent p && data is TValue t ? ImageUrl(p, t) : string.Empty;
    Func<object, object, string> ITvImageViewerOption.FallbackUrl => (parent, data) => parent is TParent p && data is TValue t ? FallbackUrl(p, t) : string.Empty;
    Func<object?, object, int, string, bool> ITvCustomOption.Condition => (parent, data, depth, path) => parent is TParent p && data is TValue t ? Condition.Invoke(p, t, depth, path) : false;


    /// <summary>
    /// Scales this image to the parent container.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>false</c>.
    /// </remarks>

    public bool Fluid { get; set; } = false;

    /// <summary>
    /// The alternate text for this image.
    /// </summary>

    public string? Alt { get; set; }

    /// <summary>
    /// The height of this image, in pixels.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>null</c>.
    /// </remarks>

    public int? Height { get; set; } = null;

    /// <summary>
    /// The width of this image, in pixels.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>null</c>.
    /// </remarks>

    public int? Width { get; set; } = null;

    /// <summary>
    /// The size of the drop shadow for this image.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>0</c>.  
    /// </remarks>

    public int Elevation { get; set; } = 0;

    /// <summary>
    /// Controls how this image is resized.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="ObjectFit.Fill"/>.
    /// </remarks>

    public ObjectFit ObjectFit { get; set; } = ObjectFit.Fill;

    /// <summary>
    /// Controls how this image is positioned within its container.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="ObjectPosition.Center"/>.
    /// </remarks>

    public ObjectPosition ObjectPosition { get; set; } = ObjectPosition.Center;

}
