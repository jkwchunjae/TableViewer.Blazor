namespace TableViewerBlazor.Options;


public static class TeCustomEditorOptionExtensions
{
    public static bool TryGetCustomEditor(
        this TeOptions options,
        MemberInfo? memberInfo,
        Type? type,
        out TeCustomEditorOption? customEditorOption
        )
    {
        var customEditorAttribute = memberInfo?.GetCustomAttribute<TeCustomEditorAttribute>();
        if (customEditorAttribute != null)
        {
            var id = customEditorAttribute.Id;
            customEditorOption = options.CustomEditorOptions.CustomEditors
                .FirstOrDefault(option => option?.Id == id);

            if (customEditorOption != default)
            {
                return true;
            }
        }

        customEditorOption = options.CustomEditorOptions?.CustomEditors
            .Where(option => option?.Condition?.Invoke(memberInfo, type) ?? false)
            .FirstOrDefault();
        return customEditorOption != default;
    }
}
public class TeCustomEditorOptionGroup
{
    public TeCustomEditorOption? CustomEditor1 { get; set; }
    public TeCustomEditorOption? CustomEditor2 { get; set; }
    public TeCustomEditorOption? CustomEditor3 { get; set; }
    public TeCustomEditorOption? CustomEditor4 { get; set; }
    public TeCustomEditorOption? CustomEditor5 { get; set; }

    public IEnumerable<TeCustomEditorOption?> CustomEditors =>
    [
        CustomEditor1,
        CustomEditor2,
        CustomEditor3,
        CustomEditor4,
        CustomEditor5,
    ];
}

public class TeCustomEditorOption
{
    public string? Id { get; set; }
    public Func<MemberInfo?, Type?, bool> Condition { get; set; } = default!;
    public RenderFragment<ICustomEditorArgument>? RenderFragment { get; set; } = default;
}

public class TeCustomEditorAttribute : Attribute
{
    public string Id { get; init; }
    public TeCustomEditorAttribute(string id)
    {
        Id = id;
    }
}

