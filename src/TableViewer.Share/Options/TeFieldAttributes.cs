namespace TableViewer.Share.Options;

public class TeAutocompleteAttribute : TeFieldAttribute
{
    public TeAutocompleteAttribute(string id) : base(id) { }
}

public class TeCheckBoxAttribute : TeFieldAttribute
{
    public TeCheckBoxAttribute(string id)
        : base(id)
    {
    }
}

public class TeCustomEditorAttribute : TeFieldAttribute
{
    public TeCustomEditorAttribute(string id)
        : base(id)
    {
    }
}

public class TeIgnoreAttribute : Attribute
{
}

public class TeListEditorAttribute : TeFieldAttribute
{
    public TeListEditorAttribute(string id)
        : base(id)
    {
    }
}

public class TeNumericFieldAttribute : TeFieldAttribute
{
    public TeNumericFieldAttribute(string id)
        : base(id)
    {
    }
}

public class TeObjectListAttribute : TeFieldAttribute
{
    public TeObjectListAttribute(string id)
        : base(id)
    {
    }
}

public class TeRadioAttribute : TeFieldAttribute
{
    public TeRadioAttribute(string id)
        : base(id)
    {
    }
}

public class TeSelectBoxAttribute : TeFieldAttribute
{
    public TeSelectBoxAttribute(string id)
        : base(id)
    {
    }
}

public class TeSwitchAttribute(string id) : TeFieldAttribute(id) { }

public class TeTextFieldAttribute : TeFieldAttribute
{
    public TeTextFieldAttribute(string id)
        : base(id)
    {
    }
}
