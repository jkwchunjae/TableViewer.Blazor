namespace TableViewerBlazor.Internal.TvComponent;

public partial class TvObjectArrayView : TvViewBase
{
    [Parameter] public IEnumerable<object?> Data { get; set; } = null!;
    [Parameter] public bool Loading { get; set; }

    bool? Open = null;

    IEnumerable<object?> array => Data ?? Enumerable.Empty<object?>();

    private MemberInfo[] MemberInfos = Array.Empty<MemberInfo>();
    private bool HasAnyAction;
    private int TableColumns => MemberInfos.Length + (HasAnyAction ? 1 : 0);

    protected override void OnInitialized()
    {
        HasAnyAction = Data.Any(x => HasAction(x) || HasLink(x));
        var firstData = Data.FirstOrDefault(x => x != null);
        if (firstData != null)
        {
            MemberInfos = GetKeys(firstData).ToArray();
        }
        if (Options != null)
        {
            Open ??= Depth <= OpenDepth;
        }
    }

    private object? GetValue(object? item, string key)
    {
        var dataType = item?.GetType();
        var property = dataType?.GetProperty(key);
        if (property != null)
        {
            return property.GetValue(item);
        }
        var field = dataType?.GetField(key);
        if (field != null)
        {
           return field.GetValue(item);
        }
        return null;
    }

    private void ToggleOpen()
    {
        Open = !Open;
    }

    private IEnumerable<MemberInfo> GetKeys(object? data)
    {
        if (data == null)
            yield break;

        var dataType = data.GetType();

        if (Options?.ReadProperty ?? false)
        {
            var properties = dataType.GetProperties()
                .Where(p => p.CanRead)
                .Where(p => p.PropertyType != typeof(Type))
                .Where(p => p.GetCustomAttribute<TvIgnoreAttribute>() == null);
            foreach (var property in properties)
            {
                yield return property;
            }
        }

        if (Options?.ReadField ?? false)
        {
            var fields = dataType.GetFields()
                .Where(f => f.IsPublic)
                .Where(f => f.FieldType != typeof(Type))
                .Where(f => f.GetCustomAttribute<TvIgnoreAttribute>() == null);
            foreach (var field in fields)
            {
                yield return field;
            }
        }
    }

    private bool HasAction(object? item)
    {
        return Options?.Actions?.Any(action => action.Condition(item, Depth)) ?? false;
    }

    private bool HasLink(object? item)
    {
        return Options?.Links?.Any(link => link.Condition(item, Depth)) ?? false;
    }
}