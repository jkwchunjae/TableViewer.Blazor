using TableViewerBlazor.Internal.TeComponent;

namespace TableViewerBlazor.Options;

public static class TeArrayOptionExtensions
{
    public static bool TryGetListEditorOption(this TeOptions options,
        MemberInfo? memberInfo, TeEditorBase teBase,
        out ITeListEditorOption? listEditorOption)
    {
        var listEditorAttribute = memberInfo?.GetCustomAttribute<TeListEditorAttribute>();
        if (listEditorAttribute != null)
        {
            listEditorOption = options.ListEditorOptions?
                .FirstOrDefault(o => o.Id == listEditorAttribute.Id) ?? default;
            if (listEditorOption != null)
            {
                return true;
            }
        }

        listEditorOption = options.ListEditorOptions?
            .Where(option => string.IsNullOrEmpty(option.Id))
            .FirstOrDefault(o => o.Condition?.Invoke(teBase.Data, teBase.Depth, teBase.Path) ?? true) ?? default;
        if (listEditorOption != null)
        {
            return true;
        }

        listEditorOption = teBase.Data switch
        {
            IList<string> => new TeListEditorOption<string>(string.Empty),
            IList<bool> => new TeListEditorOption<bool>(default),

            // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/integral-numeric-types
            IList<sbyte> => new TeListEditorOption<sbyte>(default),
            IList<byte> => new TeListEditorOption<byte>(default),
            IList<short> => new TeListEditorOption<short>(default),
            IList<ushort> => new TeListEditorOption<ushort>(default),
            IList<int> => new TeListEditorOption<int>(default),
            IList<uint> => new TeListEditorOption<uint>(default),
            IList<long> => new TeListEditorOption<long>(default),
            IList<ulong> => new TeListEditorOption<ulong>(default),
            IList<nint> => new TeListEditorOption<nint>(default),
            IList<nuint> => new TeListEditorOption<nuint>(default),
            // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/floating-point-numeric-types
            IList<float> => new TeListEditorOption<float>(default),
            IList<double> => new TeListEditorOption<double>(default),
            IList<decimal> => new TeListEditorOption<decimal>(default),

            _ => default,
        };
        return listEditorOption != null;
    }
}

public class TeListEditorAttribute : Attribute
{
    public string Id { get; init; }
    public TeListEditorAttribute(string id)
    {
        Id = id;
    }
}

public interface ITeListEditorOption : ITeFieldOption
{
    bool ShowNumber { get; }
    Func<object?> CreateNew { get; }
    ITvAction AddItemAction { get; }
    ITvAction RemoveItemAction { get; }
}

public class TeListEditorOption<T> : ITeFieldOption<IList<T>>, ITeListEditorOption
{
    public string? Id { get; set; }
    public Func<IList<T>?, int, string, bool>? Condition { get; set; }
    public bool ShowNumber { get; set; } = false;
    public Func<T> CreateNew { get; set; } = () => Activator.CreateInstance<T>();
    public ITvAction AddItemAction { get; set; } = CreateDefaultAddAction();
    public ITvAction RemoveItemAction { get; set; } = CreateDefaultRemoveAction();

    Func<object?> ITeListEditorOption.CreateNew => () => CreateNew();

    public TeListEditorOption()
    {
    }
    public TeListEditorOption(T defaultValue)
    {
        CreateNew = () => defaultValue;
    }

    private static TvAction<object> CreateDefaultAddAction()
    {
        return new TvAction<object>
        {
            Action = _ => Task.CompletedTask,
            Label = "Add Item",
            Style = new TvButtonStyle
            {
                Size = Size.Medium,
                Dense = true,
                SuperDense = false,
            },
        };
    }

    private static TvAction<object> CreateDefaultRemoveAction()
    {
        return new TvAction<object>
        {
            Action = _ => Task.CompletedTask,
            Label = string.Empty,
            Style = new TvButtonStyle
            {
                StartIcon = Icons.Material.Outlined.Cancel,
                IconColor = Color.Error,
                Dense = false,
                SuperDense = false,
            }
        };
    }
}

