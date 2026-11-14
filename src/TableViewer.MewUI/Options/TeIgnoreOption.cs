using TableViewer.MewUI.Internal.Editing;

namespace TableViewer.MewUI.Options;

public static class TeIgnoreOptionExtensions
{
    public static bool IsIgnored(this TeOptions options,
        MemberInfo? memberInfo, TeEditorBase teBase)
    {
        if (memberInfo != null)
        {
            var ignored = memberInfo.GetCustomAttribute<TeIgnoreAttribute>() != null;
            return ignored;
        }
        else
        {
           return false;
        }
    }
}

public class TeIgnoreAttribute : Attribute
{
}


