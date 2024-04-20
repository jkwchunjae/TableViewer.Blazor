namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeElementEditor : ComponentBase
{
    /// <summary>
    /// FieldInfo, PropertyInfo
    /// </summary>
    [Parameter] public MemberInfo? MemberInfo { get; set; }
    [Parameter] public Type Type { get; set; } = default!;
    [Parameter] public string? Name { get; set; }
    [Parameter] public object? Data { get; set; }
    [Parameter] public EventCallback<object?> DataChanged { get; set; }
    [Parameter] public TeOptions Options { get; set; } = default!;

    private bool TryGetSelectBoxOption(out ITeSelectBoxOption? selectBoxOption)
    {
        var selectAttribute = MemberInfo?.GetCustomAttribute<TeSelectBoxAttribute>();
        if (selectAttribute != null)
        {
            selectBoxOption = Options.SelectBoxOptions?
                .FirstOrDefault(o => o.Id == selectAttribute.Id) ?? default;
            if (selectBoxOption != null)
            {
                return true;
            }
        }
        selectBoxOption = Options.SelectBoxOptions?
            .Where(option => string.IsNullOrEmpty(option.Id))
            .FirstOrDefault(o => o.Condition(Data, 0, "path")) ?? default;
        return selectBoxOption != null;
    }
}
