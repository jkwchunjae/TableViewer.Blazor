using TableViewerBlazor.Internal.TeComponent;
using TableViewerBlazor.Options.Property;

namespace TableViewerBlazor.Options;

public static class TeCheckBoxOptionExtensions
{
    public static bool TryGetCheckBoxOption(
        this TeOptions options,
        MemberInfo? memberInfo,
        TeEditorBase teBase,
        out ITeCheckBoxOption? checkBoxOption
        )
    {
        var selectedAttribute = memberInfo?.GetCustomAttribute<TeCheckBoxAttribute>();
        if (selectedAttribute != null)
        {
            checkBoxOption = options.CheckBoxOptions?
                .FirstOrDefault(o => o.Id == selectedAttribute.Id) ?? default;
            if (checkBoxOption != null)
            {
                return true;
            }
        }
        checkBoxOption = options.CheckBoxOptions?
            .Where(option => string.IsNullOrEmpty(option.Id))
            .Where(option => option.Condition?.Invoke(teBase.Data, teBase.Depth, teBase.Path) ?? true)
            .FirstOrDefault() ?? default!;

        if (checkBoxOption != null)
        {
            return true;
        }

        checkBoxOption = teBase.Data switch
        {
            bool _ => new TeCheckBoxOption<object>(),
            _ => null
        };
        return checkBoxOption != null;
    }
}

public class TeCheckBoxAttribute : Attribute
{
    public string Id { get; init; }
    public TeCheckBoxAttribute(string id)
    {
        Id = id;
    }
}

public interface ITeCheckBoxOption : ITeFieldOption
{
    public ITeCheckBoxProperty Property { get; set; } 
}

public class TeCheckBoxOption<T> : ITeFieldOption<T>, ITeCheckBoxOption
{
    public string? Id { get; set; }
    public Func<T?, int, string, bool>? Condition { get; set; }
    public ITeCheckBoxProperty Property { get; set; } = new TeCheckBoxProperty();
}

public class TeCheckBoxOption : ITeCheckBoxOption
{
    public string? Id { get; set; }
    public Func<object?, int, string, bool>? Condition { get; set; }
    public ITeCheckBoxProperty Property { get; set; } = new TeCheckBoxProperty();
}