using TableViewerBlazor.Internal.TvComponent;

namespace TableViewerBlazor.Options;

public static class TvIgnoreOption
{
    public static bool IsIgnored(this TvOptions options, MemberInfo? memberInfo)
    {
        if (memberInfo != null)
        {
            return memberInfo.GetCustomAttribute<TvIgnoreAttribute>() != null;
        }
        else
        {
            return false;
        }
    }
}

public class TvIgnoreAttribute : Attribute
{
}
