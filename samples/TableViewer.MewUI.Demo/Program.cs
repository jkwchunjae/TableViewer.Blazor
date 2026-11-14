using Aprillz.MewUI;
using Aprillz.MewUI.Controls;

var name = new ObservableValue<string>("World");
var status = new ObservableValue<string>("Ready");
Application.Create().UseWin32().UseDirect2D()
    .BuildMainWindow(() => new Window().Title("TableViewer.MewUI port")
        .Resizable(1100, 800, minWidth: 800, minHeight: 560)
        .Content(new StackPanel().Vertical().Spacing(12).Margin(20).Children(
            new TextBox().BindText(name),
            new TextBlock().BindText(name, value => $"Hello, {value}"),
            new Button().Content("Save").OnClick(() => status.Value = $"Saved for {name.Value}"),
            new TextBlock().BindText(status))))
    .Run();
