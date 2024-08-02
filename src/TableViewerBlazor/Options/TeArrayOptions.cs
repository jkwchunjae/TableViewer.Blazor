using TableViewerBlazor.Internal.TeComponent;

namespace TableViewerBlazor.Options;

public static class TeArrayOptionExtensions
{
    public static bool TryGetArrayOption(this TeOptions options, TeEditorBase teBase, out TeArrayOption? arrayOption)
    {
        if (options.ArrayOptions.Any())
        {
            arrayOption = options.ArrayOptions.FirstOrDefault();
            return true;
        }
        else
        {
            arrayOption = null;
            return false;
        }
    }
}

public class TeArrayOption
{
    public bool ShowNumber { get; set; } = false;
    public ITvAction AddItemAction { get; set; } = CreateDefaultAddAction();
    public ITvAction RemoveItemAction { get; set; } = CreateDefaultRemoveAction();

    private static TvAction<object> CreateDefaultAddAction()
    {
        return new TvAction<object>
        {
            Action = _ => Task.CompletedTask,
            Label = "Add Item",
            Style = new TvButtonStyle
            {
                StartIcon = Icons.Material.Outlined.PlusOne,
                IconSize = Size.Medium,
                Size = Size.Medium,
                Dense = false,
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
