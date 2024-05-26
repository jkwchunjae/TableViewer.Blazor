using Microsoft.AspNetCore.Components.Forms;
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
    TvButtonStyle ButtonStyle { get; }
}

public abstract class TeImageUploaderOption : ITeImageUploaderOption
{
    public string? Id { get; set; }
    public Func<object?, int, string, bool>? Condition { get; set; }
    public long MaxAllowedSize { get; set; } = 512_000L;
    public string Accept { get; set; } = ".png, .jpg";
    public TvButtonStyle ButtonStyle { get; set; } = new();
    public List<ITeValidation> Validations = [];

    IEnumerable<ITeValidation> ITeImageUploaderOption.Validations => Validations;
}

public class TeBase64ImageUploaderOption : TeImageUploaderOption
{
}

public class TeFilePathImageUploaderOption : TeImageUploaderOption
{
    public Func<IBrowserFile, Task<string>> SaveFileAsync { get; set; } = default!;
}

public enum TeImageType
{
    /// <summary>
    /// Base64 encoded string
    /// 별도의 파일을 저장하지 않고 Base64로 저장
    /// </summary>
    Base64,
    /// <summary>
    /// 사용자가 선택한 파일을 저장하는 방식
    /// 저장한 파일의 경로를 반환해 저장해야 한다.
    /// </summary>
    FilePath,
}