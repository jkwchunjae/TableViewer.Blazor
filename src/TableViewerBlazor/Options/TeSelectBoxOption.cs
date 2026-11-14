using TableViewerBlazor.Options.Property;

namespace TableViewerBlazor.Options;



public interface ITeSelectBoxOption : ITeFieldOption
{
    IEnumerable<ITeSelectItem> Items { get; }
    TeSelectBoxProperty Property { get; }
}

public class TeSelectBoxOption<TValue> : global::TableViewer.Share.Options.TeFieldOption<TValue, TeSelectBoxProperty<TValue>>, ITeSelectBoxOption, ITeGenericTypeOption
{

    public List<TeSelectItem<TValue>> Items { get; set; } = [];

    IEnumerable<ITeSelectItem> ITeSelectBoxOption.Items => Items;
    TeSelectBoxProperty ITeSelectBoxOption.Property => Property;
}
