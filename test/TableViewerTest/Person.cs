using System.Text.Json.Serialization;
using TableViewerBlazor.Helper;
using TableViewerBlazor.Public;

namespace TableViewerTest;

public class Person
{
    public int Id { get; set; }
    public PersonName? Name { get; set; }
    public DateTime? Birth { get; set; }
    public DateTime? Death { get; set; }
    public Dictionary<string, object>? Attributes { get; set; }
    public TestEnum EnumValue { get; set; }

    public PersonRecord[]? People { get; set; }
    public OpenTest? OpenTest { get; set; }
}

public class OpenTest
{
    public required string Id { get; set; }
    public required int[] Numbers { get; set; }
    public PersonRecord[]? People { get; set; }
}

public record PersonRecord(int Id, PersonName? Name, Dictionary<string, object>? Attributes);

public enum TestEnum
{
    A,
    B,
    C,
}

[JsonConverter(typeof(TvStringJsonConverter<PersonName>))]
[TvString]
public record PersonName(string Name)
{
    public override string ToString() => Name;
}