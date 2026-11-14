using TableViewer.MewUI.Options;
using TableViewer.Share.Public;

namespace TableViewer.MewUI.Demo;

public sealed class Person
{
    public string Name { get; set; } = "Ada";
    public int Age { get; set; } = 28;
    public bool Active { get; set; } = true;
    public Address Address { get; set; } = new();
    public string[] Tags = ["C#", "MewUI", "native", "table", "editor", "port"];
    [TvString] public Version Version { get; set; } = new(1, 2, 3);
    public Type IgnoredType => typeof(Person);
    public string Secret => "Hidden by DisableKeys";
}
public sealed class Address { public string City { get; set; } = "서울"; public int Zip { get; set; } = 12345; }
public sealed class JsonPayload { public string Format { get; set; } = "JSON"; public int[] Numbers { get; set; } = [1, 2, 3]; }
public sealed class YamlPayload { public string Format { get; set; } = "YAML"; public Address Location { get; set; } = new(); }
public sealed class Asset { public string Website { get; set; } = "https://github.com/jkwchunjae/TableViewer.Blazor"; public string Picture { get; set; } = "missing-image.png"; }
public sealed class EditModel
{
    [TeTextField("name")] public string Name { get; set; } = "Ada";
    [TeNumericField("age")] public int Age { get; set; } = 28;
    [TeCheckBox("active")] public bool Active { get; set; } = true;
    [TeSwitch("enabled")] public bool Enabled { get; set; } = true;
    [TeSelectBox("role")] public string Role { get; set; } = "Engineer";
    [TeRadio("level")] public int Level { get; set; } = 1;
    [TeAutocomplete("city")] public string City { get; set; } = "Seoul";
    [TeTextField("version")] public Version Version { get; set; } = new(1, 2, 3);
    [TeTextField("phone")] public string Phone { get; set; } = "010-1234-5678";
    [TeTextField("date")] public string Date { get; set; } = "2026-11-15";
    [TeTextField("color")] public string Color { get; set; } = "#3366CC";
    [TeIgnore] public string Hidden { get; set; } = "not editable";
}
public sealed class ListModel
{
    public List<string> Tags { get; set; } = ["first", "second"];
    public List<ListRow> People { get; set; } = [new()];
}
public sealed class ListRow { public string Name { get; set; } = "Grace"; public int Age { get; set; } = 30; }
public sealed class CustomModel
{
    [TeCustomEditor("slot1")] public int First { get; set; } = 1;
    [TeCustomEditor("slot2")] public int Second { get; set; } = 2;
    [TeCustomEditor("slot3")] public int Third { get; set; } = 3;
    [TeCustomEditor("slot4")] public int Fourth { get; set; } = 4;
    [TeCustomEditor("slot5")] public int Fifth { get; set; } = 5;
}
