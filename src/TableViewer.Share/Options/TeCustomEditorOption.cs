using System.Reflection;

namespace TableViewer.Share.Options;

public interface ITeConditionalFieldOption : ITeFieldOption
{
    Func<MemberInfo?, Type?, bool>? Condition { get; }
}

/// <summary>Shared custom editor matching; the presentation adapter supplies its render delegate.</summary>
public class TeCustomEditorOption<TFactory> : ITeConditionalFieldOption
{
    public string? Id { get; set; }
    public Func<MemberInfo?, Type?, bool>? Condition { get; set; }
    public TFactory? RenderFragment { get; set; }
}

public class TeCustomEditorOptionGroup<TOption> where TOption : class
{
    public TOption? CustomEditor1 { get; set; }
    public TOption? CustomEditor2 { get; set; }
    public TOption? CustomEditor3 { get; set; }
    public TOption? CustomEditor4 { get; set; }
    public TOption? CustomEditor5 { get; set; }
    public IEnumerable<TOption> CustomEditors => new[] { CustomEditor1, CustomEditor2, CustomEditor3, CustomEditor4, CustomEditor5 }.OfType<TOption>();
}

public static class TeCustomEditorPolicy
{
    public static TOption? Find<TOption>(MemberInfo? member, Type? type, IEnumerable<TOption> options)
        where TOption : class, ITeConditionalFieldOption
    {
        var candidates = options.ToArray();
        var id = member?.GetCustomAttribute<TeCustomEditorAttribute>()?.Id;
        return (id is not null ? candidates.FirstOrDefault(option => option.Id == id) : null)
            ?? candidates.FirstOrDefault(option => option.Condition?.Invoke(member, type) == true);
    }
}
