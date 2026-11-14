namespace TableViewer.MewUI.Options;

public static class TeCustomEditorOptionExtensions
{
    public static bool TryGetCustomEditor(this TeOptions options, MemberInfo? memberInfo, Type? type, out TeCustomEditorOption? customEditorOption)
    {
        customEditorOption = TeCustomEditorPolicy.Find(memberInfo, type, options.CustomEditorOptions.CustomEditors);
        return customEditorOption is not null;
    }
}

public class TeCustomEditorOptionGroup : global::TableViewer.Share.Options.TeCustomEditorOptionGroup<TeCustomEditorOption> { }

public class TeCustomEditorOption : global::TableViewer.Share.Options.TeCustomEditorOption<Func<ICustomEditorArgument, Element>> { }
