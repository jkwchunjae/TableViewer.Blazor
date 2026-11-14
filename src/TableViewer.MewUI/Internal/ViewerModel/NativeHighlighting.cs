using Aprillz.MewUI.MewvalonEdit.Highlighting;
using Aprillz.MewUI.MewvalonEdit.Highlighting.Xshd;

namespace TableViewer.MewUI.Internal.ViewerModel;

internal static class NativeHighlighting
{
    private static readonly Lazy<IHighlightingDefinition> Yaml = new(() =>
    {
        using var reader = new StringReader("""
            <SyntaxDefinition name="YAML" extensions=".yaml;.yml" xmlns="http://icsharpcode.net/sharpdevelop/syntaxdefinition/2008">
              <Color name="Comment" foreground="#6A9955" />
              <Color name="String" foreground="#CE9178" />
              <Color name="Keyword" foreground="#569CD6" />
              <Color name="Number" foreground="#B5CEA8" />
              <Color name="Key" foreground="#4E9CCB" />
              <RuleSet>
                <Span color="Comment" begin="\#" />
                <Rule color="String">"(?:\\.|[^"\\])*"|'(?:''|[^'])*'</Rule>
                <Keywords color="Keyword"><Word>true</Word><Word>false</Word><Word>null</Word><Word>True</Word><Word>False</Word><Word>Null</Word></Keywords>
                <Rule color="Key">(?m)^[ \t]*(?:[\w.-]+)(?=[ \t]*:)</Rule>
                <Rule color="Number">(?&lt;![\w.])[-+]?\d+(?:\.\d+)?(?:[eE][-+]?\d+)?(?![\w.])</Rule>
              </RuleSet>
            </SyntaxDefinition>
            """);
        return HighlightingLoader.Load(reader, HighlightingManager.Instance);
    });

    public static IHighlightingDefinition? Get(string language)
    {
        var name = language.ToLowerInvariant() switch
        {
            "json" => "Json", "yaml" or "yml" => "YAML", "csharp" or "cs" => "C#", "xml" => "XML",
            "javascript" or "js" => "JavaScript", "html" => "HTML", "css" => "CSS", "python" => "Python",
            "markdown" or "md" => "MarkDown", "sql" => "TSQL", "powershell" or "ps1" => "PowerShell",
            _ => language
        };
        return name == "YAML" ? Yaml.Value : HighlightingManager.Instance.GetDefinition(name);
    }
}
