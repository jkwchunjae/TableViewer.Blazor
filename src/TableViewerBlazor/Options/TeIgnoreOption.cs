using TableViewerBlazor.Internal.TeComponent;

namespace TableViewerBlazor.Options;

public static class TeIgnoreOptionExtensions
{
    public static bool TryGetIgnoreOption(this TeOptions options,
        MemberInfo? memberInfo, TeEditorBase teBase,
        out TeIgnoreOption? ignoreOption)
    {
        ignoreOption = default;
        return ignoreOption != null;
    }
}

public class TeIgnoreAttribute : Attribute
{
}

public class TeIgnoreOption
{
}
