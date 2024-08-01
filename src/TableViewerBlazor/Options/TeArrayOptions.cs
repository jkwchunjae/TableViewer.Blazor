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
}
