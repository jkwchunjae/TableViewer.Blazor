namespace TableViewerBlazor.Options;

public static class TeIgnoreOptionExtensions
{
    public static bool TryGetIgnoreOption(this TeOptions options,
        MemberInfo? memberInfo, object? data,
        out TeIgnoreOption? ignoreOption)
    {
        ignoreOption = options.IgnoreOptions?
            .Where(option => option.Condition?.Invoke(data, 0, "path") ?? false)
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
