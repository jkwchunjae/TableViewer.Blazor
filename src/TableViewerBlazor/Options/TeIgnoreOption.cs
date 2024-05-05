using TableViewerBlazor.Internal.TeComponent;

namespace TableViewerBlazor.Options;

public static class TeIgnoreOptionExtensions
{
    public static bool TryGetIgnoreOption(this TeOptions options,
        MemberInfo? memberInfo, TeEditorBase teBase,
        out TeIgnoreOption? ignoreOption)
    {
        ignoreOption = options.IgnoreOptions?
            .Where(option => option.Condition?.Invoke(teBase.Data, teBase.Depth, teBase.Path) ?? false)
            .FirstOrDefault() ?? default;
        return ignoreOption != null;
    }
}

public class TeIgnoreAttribute : Attribute
{
}

public class TeIgnoreOption
{
    public Func<object?, int, string, bool>? Condition { get; set; }
}
