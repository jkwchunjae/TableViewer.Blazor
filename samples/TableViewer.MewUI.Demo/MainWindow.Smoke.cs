using System.Text.Json;
using Aprillz.MewUI;
using Aprillz.MewUI.Controls;

namespace TableViewer.MewUI.Demo;

internal sealed partial class MainWindow
{
    private readonly List<string> _smokeErrors = [];

    // Runs on the same UI thread and graphics backend as the interactive example.
    // This checks real layout and composition, not simulated clicks or a pixel comparison.
    public async void RunSmokeTest(string reportPath)
    {
        var results = new List<object>();
        var failures = 0;
        try
        {
            for (var index = 0; index < _cases.Length; index++)
            {
                _smokeErrors.Clear();
                try
                {
                    ShowCase(index);
                    PerformLayout();
                    await Task.Delay(150); // Allow asynchronous image fallback and deferred focus to finish.
                    PerformLayout();
                    var elements = Descendants(_pageHost).ToArray();
                    var errors = elements.OfType<TextBlock>().Select(text => text.Text)
                        .Where(text => text.StartsWith("Error:", StringComparison.Ordinal)).Concat(_smokeErrors).Distinct().ToList();
                    if (index == 3 && elements.OfType<Aprillz.MewUI.MewvalonEdit.TextEditor>().Count(editor => editor.SyntaxHighlighting is not null && editor.IsReadOnly) != 2)
                        errors.Add("JSON and YAML must both create read-only editors with highlighting definitions.");
                    if (index == 4 && !elements.OfType<Image>().Any(image => image.Source is ImageSource { PixelWidth: 96, PixelHeight: 48 }))
                        errors.Add("The failed primary image must load the 96×48 fallback image.");
                    if (index == 7 && (!elements.OfType<DatePicker>().Any() || !elements.OfType<ColorPicker>().Any()))
                        errors.Add("Date and color fields must create native pickers.");
                    var pass = errors.Count == 0 && elements.Length > 3 && _pageHost.ActualWidth > 0 && _pageHost.ActualHeight > 0;
                    if (!pass) failures++;
                    results.Add(new { name = _cases[index].Title, pass, count = elements.Length, width = _pageHost.ActualWidth, height = _pageHost.ActualHeight, errors,
                        diagnostics = pass ? null : elements.Select(element => element.GetType().Name + (element is TextBlock text ? ": " + text.Text : element is Image image ? ": " + image.Source?.GetType().Name : "")).ToArray() });
                }
                catch (Exception exception) { failures++; results.Add(new { name = _cases[index].Title, pass = false, error = exception.ToString() }); }
            }
            var absoluteReportPath = Path.GetFullPath(reportPath);
            Directory.CreateDirectory(Path.GetDirectoryName(absoluteReportPath)!);
            await File.WriteAllTextAsync(absoluteReportPath, JsonSerializer.Serialize(new { backend = "Windows Direct2D", failures, results }, new JsonSerializerOptions { WriteIndented = true }));
        }
        catch (Exception exception) { failures++; Console.Error.WriteLine(exception); }
        finally { Application.Shutdown(failures == 0 ? 0 : 1); }
    }

    private static IEnumerable<Element> Descendants(Element root)
    {
        yield return root;
        var children = new List<Element>();
        if (root is ILogicalTreeHost host) host.VisitLogicalChildren(child => { children.Add(child); return true; });
        if (root is ShadowDecorator { Child: { } shadowChild } && !children.Contains(shadowChild)) children.Add(shadowChild);
        foreach (var child in children)
            foreach (var descendant in Descendants(child)) yield return descendant;
    }
}
