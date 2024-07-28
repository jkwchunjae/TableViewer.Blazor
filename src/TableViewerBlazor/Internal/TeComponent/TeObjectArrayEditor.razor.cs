namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeObjectArrayEditor : TeEditorBase
{
    [Parameter] public IEnumerable<object> Items { get; set; } = default!;
    private bool Addable { get; } = true;

    private MemberInfo[] MemberInfos = Array.Empty<MemberInfo>();

    protected override void OnInitialized()
    {
        MemberInfos = GetMembers(Items.GetType().GenericTypeArguments[0]);
    }

    private MemberInfo[] GetMembers(Type itemType)
    {
        var members = new List<MemberInfo>();
        members.AddRange(itemType.GetProperties());
        members.AddRange(itemType.GetFields());
        return members.ToArray();
    }
}
