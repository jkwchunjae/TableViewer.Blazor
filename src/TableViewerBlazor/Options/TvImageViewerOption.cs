namespace TableViewerBlazor.Options;

public interface ITvImageViewerOption : ITvCustomOption
{
    Func<object, object, string> ImageUrl { get; }
    Func<object, object, string> FallbackUrl { get; }

    /// <summary>
    /// The alternate text for this image.
    /// </summary>
    [Category(CategoryTypes.Image.Behavior)]
    string? Alt { get; }

    /// <summary>
    /// Scales this image to the parent container.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>false</c>.
    /// </remarks>
    [Category(CategoryTypes.Image.Behavior)]
    bool Fluid { get; }

    /// <summary>
    /// The height of this image, in pixels.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>null</c>.
    /// </remarks>
    [Category(CategoryTypes.Image.Appearance)]
    int? Height { get; set; }

    /// <summary>
    /// The width of this image, in pixels.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>null</c>.
    /// </remarks>
    [Category(CategoryTypes.Image.Appearance)]
    int? Width { get; }

    /// <summary>
    /// The size of the drop shadow for this image.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>0</c>.  
    /// </remarks>
    [Category(CategoryTypes.Image.Appearance)]
    int Elevation { get; }

    /// <summary>
    /// Controls how this image is resized.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="ObjectFit.Fill"/>.
    /// </remarks>
    [Category(CategoryTypes.Image.Appearance)]
    ObjectFit ObjectFit { get; }

    /// <summary>
    /// Controls how this image is positioned within its container.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="ObjectPosition.Center"/>.
    /// </remarks>
    [Category(CategoryTypes.Image.Appearance)]
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
    [Category(CategoryTypes.Image.Behavior)]
    public bool Fluid { get; set; } = false;

    /// <summary>
    /// The alternate text for this image.
    /// </summary>
    [Category(CategoryTypes.Image.Behavior)]
    public string? Alt { get; set; }

    /// <summary>
    /// The height of this image, in pixels.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>null</c>.
    /// </remarks>
    [Category(CategoryTypes.Image.Appearance)]
    public int? Height { get; set; } = null;

    /// <summary>
    /// The width of this image, in pixels.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>null</c>.
    /// </remarks>
    [Category(CategoryTypes.Image.Appearance)]
    public int? Width { get; set; } = null;

    /// <summary>
    /// The size of the drop shadow for this image.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>0</c>.  
    /// </remarks>
    [Category(CategoryTypes.Image.Appearance)]
    public int Elevation { get; set; } = 0;

    /// <summary>
    /// Controls how this image is resized.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="ObjectFit.Fill"/>.
    /// </remarks>
    [Category(CategoryTypes.Image.Appearance)]
    public ObjectFit ObjectFit { get; set; } = ObjectFit.Fill;

    /// <summary>
    /// Controls how this image is positioned within its container.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="ObjectPosition.Center"/>.
    /// </remarks>
    [Category(CategoryTypes.Image.Appearance)]
    public ObjectPosition ObjectPosition { get; set; } = ObjectPosition.Center;

}
