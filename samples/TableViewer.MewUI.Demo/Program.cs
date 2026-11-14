using Aprillz.MewUI;
using Aprillz.MewUI.Controls;

Application.Create().UseWin32().UseDirect2D().UseTheme(ThemeVariant.System)
    .BuildMainWindow(() =>
    {
        var window = new TableViewer.MewUI.Demo.MainWindow();
        if (args is ["--smoke-test", var reportPath]) window.OnLoaded(() => window.RunSmokeTest(reportPath));
        return window;
    })
    .Run();
