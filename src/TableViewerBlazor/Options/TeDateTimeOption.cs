using TableViewerBlazor.Internal.TeComponent;
using TableViewerBlazor.Options.Property;

namespace TableViewerBlazor.Options;

public static class TeDateTimeOptionExtensions
{
    public static bool TryGetDateTimeOption(this TeOptions options,
        MemberInfo? memberInfo, TeEditorBase teBase,
        out ITeDateTimeOption? dateTimeOption)
    {
        var dateTimeAttribute = memberInfo?.GetCustomAttribute<TeDateTimeAttribute>();
        if (dateTimeAttribute != null)
        {
            dateTimeOption = options.DateTimeOptions?
                .FirstOrDefault(o => o.Id == dateTimeAttribute.Id) ?? default;
            if (dateTimeOption != null)
            {
                return true;
            }
        }
        dateTimeOption = options.DateTimeOptions?
            .Where(option => string.IsNullOrEmpty(option.Id))
            .Where(option => option.Condition?.Invoke(teBase.Data, teBase.Depth, teBase.Path) ?? true)
            .FirstOrDefault() ?? default;
        return dateTimeOption != null;
    }
}

public class TeDateTimeAttribute : Attribute
{
    public string Id { get; init; }
    public TeDateTimeAttribute(string id)
    {
        Id = id;
    }
}

public interface ITeDateTimeOption : ITeFieldOption
{
    IEnumerable<ITeValidation> Validations { get; }
    TeDateTimeProperty? Property { get; }
    bool UseDate { get; }
    bool UseTime { get; }
}

public class TeDateTimeOption : ITeFieldOption<DateTime>, ITeDateTimeOption
{
    public string? Id { get; set; }
    public Func<DateTime, int, string, bool>? Condition { get; set; }
    public List<ITeValidation> Validations { get; set; } = [];
    public TeDateTimeProperty? Property { get; set; }
    public bool UseDate { get; set; } = true;
    public bool UseTime { get; set; } = false;

    IEnumerable<ITeValidation> ITeDateTimeOption.Validations => Validations;
}

public class TeDateTimeProperty
{

}
