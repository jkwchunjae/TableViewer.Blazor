using TableViewerBlazor.Internal.TeComponent;

namespace TableViewerBlazor.Options;


public static class TeImageUploaderOptionExtensions
{
    public static bool TryGetImageUploaderOption(this TeOptions options,
        MemberInfo? memberInfo, TeEditorBase teBase,
        out ITeImageUploaderOption? ImageUploaderOption)
    {
        var ImageUploaderAttribute = memberInfo?.GetCustomAttribute<TeImageUploaderAttribute>();
        if (ImageUploaderAttribute != null)
        {
            ImageUploaderOption = options.ImageUploaderOptions?
                .FirstOrDefault(o => o.Id == ImageUploaderAttribute.Id) ?? default;
            if (ImageUploaderOption != null)
            {
                return true;
            }
        }

        ImageUploaderOption = options.ImageUploaderOptions?
            .Where(option => string.IsNullOrEmpty(option.Id))
            .FirstOrDefault(o => o.Condition?.Invoke(teBase.Data, teBase.Depth, teBase.Path) ?? true) ?? default;
        if (ImageUploaderOption != null)
        {
            return true;
        }

        return ImageUploaderOption != null;
    }
}

public class TeImageUploaderAttribute : Attribute
{
    public string Id { get; }
    public TeImageUploaderAttribute(string id)
    {
        Id = id;
    }
}

public interface ITeImageUploaderOption : ITeFieldOption
{
    IEnumerable<ITeValidation> Validations { get; }
    long MaxAllowedSize { get; }
    string Accept { get; }
}

public class TeImageUploaderOption : ITeImageUploaderOption
{
    public string? Id { get; set; }
    public Func<object?, int, string, bool>? Condition { get; set; }
    public long MaxAllowedSize { get; set; } = 512_000L;
    public string Accept { get; set; } = ".png, .jpg";
    public List<ITeValidation> Validations = [];

    IEnumerable<ITeValidation> ITeImageUploaderOption.Validations => Validations;
}