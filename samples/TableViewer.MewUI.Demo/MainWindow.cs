using System.Collections;
using Aprillz.MewUI;
using Aprillz.MewUI.Controls;
using TableViewer.MewUI.Internal.Editing;
using TableViewer.MewUI.Options;
using TableViewer.MewUI.Options.Property;
using TvColor = TableViewer.MewUI.Options.Color;

namespace TableViewer.MewUI.Demo;

internal sealed partial class MainWindow : Window
{
    private readonly ObservableValue<string> _status = new("원본 TableViewer.MudBlazor의 기능별 포팅 예제");
    private readonly ObservableValue<string> _description = new("");
    private readonly Border _pageHost = new();
    private readonly DemoCase[] _cases;
    private int _selected;

    public MainWindow()
    {
        this.Title("TableViewer.MewUI — MudBlazor port").Resizable(1200, 850, minWidth: 850, minHeight: 600);
        _cases = [
            new("01 · 값, 객체, 필드와 중첩 표", "V01–V07, V11 · null/Empty/숫자/enum/날짜/public 필드/특성 · 개수 버튼으로 접기", ObjectExample),
            new("02 · 객체 배열과 행 액션", "V08, A01–A04 · 단일 실행/재사용/두 번 클릭/팝업 확인/링크와 제목 액션", ArrayExample),
            new("03 · 컬렉션과 사전", "V09–V10 · more는 표시 수를 두 배로, 사전 키/값은 재귀 표", CollectionExample),
            new("04 · JSON / YAML 코드 보기", "C01, C04 · serializer + 네이티브 구문 강조 + 최대 크기", CodeExample),
            new("05 · 문자열 링크와 이미지", "C02–C03 · parent/value/path 조건, 이미지 fallback/크기/fit/위치", MediaExample),
            new("06 · 열 숨김, 펼침, 비표 레이아웃", "O01–O04, S01 · ColumnVisible/DisableKeys/OpenDepth/UseTable/스타일", LayoutExample),
            new("07 · 로딩과 빈 데이터", "V12 · LoadingContent/NoRecordsContent", EmptyExample),
            new("08 · 편집, 변환, 선택과 검증", "E01–E09, E12–E14 · 텍스트/숫자/checkbox/switch/select/radio/autocomplete/converter/검증", EditorExample),
            new("09 · 목록과 객체 목록 편집", "E10–E11 · 항목 추가/삭제와 중첩 객체 편집", ListExample),
            new("10 · 형식 지정 사용자 편집기 1–5", "E15 · 다섯 typed slot, DataChanged와 ParentChanged", CustomExample)
        ];
    }

    protected override void OnBuild()
    {
        ShowCase(_selected);
        this.Content(new DockPanel().Margin(20).Spacing(12).Children(
            new StackPanel().Vertical().Spacing(10).DockTop().Children(
                new Grid().Columns("*,Auto,Auto").Spacing(8).Children(
                    new TextBlock().Text("TableViewer.MewUI").FontSize(26).Bold(),
                    new Button().Content("Light").OnClick(() => Application.Current.SetThemeMode(ThemeVariant.Light)).Column(1),
                    new Button().Content("Dark").OnClick(() => Application.Current.SetThemeMode(ThemeVariant.Dark)).Column(2)),
                new ComboBox().Items(_cases, item => item.Title).SelectedIndex(_selected).OnSelectionChanged(item => { if (item is DemoCase demo) ShowCase(Array.IndexOf(_cases, demo)); }),
                new TextBlock().BindText(_description).TextWrapping(TextWrapping.Wrap)),
            new TextBlock().BindText(_status).TextWrapping(TextWrapping.Wrap).DockBottom(),
            new ScrollViewer().Content(_pageHost)));
    }

    private void ShowCase(int index)
    {
        _selected = index;
        if (_pageHost.Child is IDisposable previous) { _pageHost.Child = null; previous.Dispose(); }
        _description.Value = _cases[index].Description;
        _pageHost.Child = _cases[index].Build();
        _status.Value = _cases[index].Title;
    }

    private TableViewer Viewer(object? data, TvOptions? options = null)
    {
        var viewer = new TableViewer(data, options ?? new() { GlobalOpenDepth = 3 });
        viewer.Error += exception => { _status.Value = exception.Message; _smokeErrors.Add(exception.Message); };
        viewer.LinkRequested = (url, target) => { _status.Value = $"LinkRequested: {url} ({target})"; return Task.CompletedTask; };
        return viewer;
    }

    private UIElement ObjectExample() => Viewer(new
    {
        Text = "Spread anything!! · 한글", Empty = "", Null = (object?)null,
        Number = 1234567890.123456789m, True = true, False = false, Enum = DayOfWeek.Sunday,
        Date = new DateTime(2026, 11, 15, 12, 34, 56), Person = new Person()
    }, new() { Title = "값과 중첩 객체", GlobalOpenDepth = 3, DisableKeys = ["Secret"], DateTime = new() { Format = "yyyy-MM-dd HH:mm:ss" } });

    private UIElement ArrayExample()
    {
        var people = new[] { new Person(), new Person { Name = "Grace", Age = 32, Active = false } };
        Task Changed(Person person) { _status.Value = $"{person.Name} 액션 실행 · {DateTime.Now:T}"; return Task.CompletedTask; }
        var options = new TvOptions { Title = "사용자 표", GlobalOpenDepth = 2, DisableKeys = ["Secret"] };
        options.Buttons.Add(new TvOneClickAction<Person> { Label = "Once", LabelAfterClick = "Done", Action = async person => { await Task.Delay(500); await Changed(person); }, TooltipOption = new() { Text = "진행 중 비활성화, 완료 후 재실행 불가", TooltipStyleProperty = new() { Arrow = true } } });
        options.Buttons.Add(new TvOneClickAction<Person> { Label = "Repeat", Reusable = true, Action = Changed });
        options.Buttons.Add(new TvDoubleClickAction<Person> { Label = "Two clicks", Action = Changed, SecondStyle = new() { Color = TvColor.Warning, Variant = Variant.Filled } });
        options.Buttons.Add(new TvPopupAction<Person> { Label = "Confirm…", PopupTitle = person => person.Name, PopupContent = _ => "Confirm 후에만 액션이 실행됩니다.", Action = Changed });
        options.Buttons.Add(new TvLink<Person> { Label = "Link", Href = person => $"https://example.com/people/{person.Name}" });
        options.TitleButtons.Add(new TvOneClickAction<Person[]> { Label = "All rows", Reusable = true, Action = rows => { _status.Value = $"OriginalData: {rows.Length} people"; return Task.CompletedTask; } });
        return Viewer(people, options);
    }

    private UIElement CollectionExample() => Viewer(new
    {
        Numbers = Enumerable.Range(0, 18).ToArray(),
        Mixed = new object?[] { "text", null, true, 5, new Address() },
        Dictionary = new Hashtable { ["Address"] = new Address(), [42] = new[] { "A", "B", "C", "D" }, ["Null"] = null }
    });

    private UIElement CodeExample() => Viewer(new { JSON = new JsonPayload(), YAML = new YamlPayload() }, new()
    {
        Title = "직렬화 보기", GlobalOpenDepth = 2,
        EditorOptions = [new TvJsonEditorOption<JsonPayload> { LayoutMaxSize = new(240, 650) }, new TvYamlEditorOption<YamlPayload> { LayoutMaxSize = new(240, 650) }]
    });

    private UIElement MediaExample() => Viewer(new Asset(), new()
    {
        GlobalOpenDepth = 2,
        StringLinkOptions = [new TvStringLinkOption<Asset, string> { Condition = (_, _, _, path) => path.EndsWith("[\"Website\"]"), Href = (_, value) => value }],
        ImageViewerOptions = [new TvImageViewerOption<Asset, string>
        {
            Condition = (_, _, _, path) => path.EndsWith("[\"Picture\"]"), ImageUrl = (_, value) => value,
            FallbackUrl = (_, _) => ImageData(), Alt = "네이티브 이미지 fallback", Width = 240, Height = 120,
            ObjectFit = ObjectFit.Cover, ObjectPosition = ObjectPosition.Center, Elevation = 2
        }]
    });

    private UIElement LayoutExample() => Viewer(new Person(), new()
    {
        Title = "독립 섹션 레이아웃", GlobalOpenDepth = 1, DisableKeys = ["Secret"],
        ColumnVisible = [new() { Before = ["Name", "Age"], After = ["Name", "Age", "Address", "Tags"] }],
        OpenDepth = [new TvOpenDepthOption<Address> { OpenDepth = 2 }],
        Style = new() { UseTable = false, Dense = false, SuperDense = false, Striped = true, Hover = true }
    });

    private UIElement EmptyExample() => new StackPanel().Vertical().Spacing(20).Children(
        new TableViewer(Array.Empty<Person>(), new() { Title = "Loading", Contents = new() { LoadingContent = "사용자를 읽는 중…", NoRecordsContent = "사용자가 없습니다." } }) { Loading = true },
        Viewer(Array.Empty<int>(), new() { Title = "Empty", Contents = new() { NoRecordsContent = "데이터가 없습니다." } }));

    private UIElement EditorExample()
    {
        var model = new EditModel();
        var options = new TeOptions
        {
            Title = "입력 폼",
            TextFieldOptions = [
                new TeTextFieldOption { Id = "name", Property = new() { Required = true, RequiredError = "이름을 입력하세요.", Immediate = true }, Validations = [new TeValidation<string> { Func = name => name.Length >= 2, Message = "이름은 두 글자 이상입니다." }] },
                new TeTextFieldOption<Version> { Id = "version", Converter = new() { StringValue = version => version.ToString(), FromString = text => Version.Parse(text) } },
                new TeTextFieldOption { Id = "phone", Property = new() { Mask = new TePatternMask("000-0000-0000"), Immediate = true } },
                new TeTextFieldOption { Id = "date", Property = new() { InputType = InputType.Date } },
                new TeTextFieldOption { Id = "color", Property = new() { InputType = InputType.Color } }
            ],
            NumericFieldOptions = [new TeNumericFieldOption<int> { Id = "age", Property = new TeNumericFieldProperty<int> { Min = 0, Max = 120, Step = 1 } }],
            CheckBoxOptions = [new TeCheckBoxOption { Id = "active" }],
            SwitchOptions = [new TeSwitchOption { Id = "enabled" }],
            SelectBoxOptions = [new TeSelectBoxOption<string> { Id = "role", Items = [new("Engineer", "Engineer"), new("Architect", "Architect")] }],
            RadioOptions = [new TeRadioOption<int> { Id = "level", Items = [new(1, "Junior"), new(2, "Senior")] }],
            AutocompleteOptions = [new TeAutocompleteOption { Id = "city", Items = ["Seoul", "Seattle", "Tokyo", "London"] }]
        };
        return EditorWithPreview(model, options);
    }

    private UIElement ListExample()
    {
        var tags = TeListEditorOption<string>.Create("new tag");
        tags.ShowNumber = true;
        tags.RemoveItemAction = new TvAction<object> { Label = "Remove tag", Action = _ => Task.CompletedTask };
        var options = new TeOptions
        {
            Title = "목록 편집",
            ListEditorOptions = [tags],
            ObjectListEditorOptions = [new TeObjectListEditorOption<ListRow>
            {
                Title = "People",
                AddItemAction = new TvAction<List<ListRow>> { Label = "Add person", Action = rows => { rows.Add(new()); return Task.CompletedTask; } },
                RemoveItemAction = new TvAction<ListRow> { Label = "Remove", Action = _ => Task.CompletedTask }
            }]
        };
        return EditorWithPreview(new ListModel(), options);
    }

    private UIElement EditorWithPreview<T>(T model, TeOptions options) where T : class
    {
        var preview = Viewer(model);
        var editor = new TableEditor<T>(model, options)
        {
            DataChanged = _ => { preview.Refresh(); _status.Value = "DataChanged · 모델과 미리보기가 갱신되었습니다."; return Task.CompletedTask; },
            IsValidChanged = valid => { _status.Value = $"IsValidChanged: {valid}"; return Task.CompletedTask; },
            ErrorsChanged = errors => { if (errors.Length > 0) _status.Value = string.Join(" / ", errors); return Task.CompletedTask; }
        };
        return new StackPanel().Vertical().Spacing(16).Children(editor,
            new Button().Content("Validate").OnClick(async () => { await editor.Validate(); _status.Value = editor.IsValid ? "Valid" : string.Join(" / ", editor.Errors); }),
            new TextBlock().Text("같은 모델의 TableViewer").Bold(), preview);
    }

    private UIElement CustomExample()
    {
        var options = new TeOptions { Title = "다섯 사용자 편집 슬롯", CustomEditorOptions = new()
        {
            CustomEditor1 = new() { Id = "slot1" }, CustomEditor2 = new() { Id = "slot2" }, CustomEditor3 = new() { Id = "slot3" },
            CustomEditor4 = new() { Id = "slot4" }, CustomEditor5 = new() { Id = "slot5" }
        } };
        return new TableEditor5<CustomModel, CustomModel, int, CustomModel, int, CustomModel, int, CustomModel, int, CustomModel, int>(new(), options)
        {
            CustomEditor1 = Counter, CustomEditor2 = Counter, CustomEditor3 = Counter, CustomEditor4 = Counter, CustomEditor5 = Counter,
            DataChanged = value => { _status.Value = $"Custom slots: {value.First}, {value.Second}, {value.Third}, {value.Fourth}, {value.Fifth}"; return Task.CompletedTask; }
        };
        static Element Counter(CustomEditorArgument<CustomModel, int> argument)
        {
            var button = new Button().Content($"Value {argument.Value} · +1");
            button.OnClick(async () => { await argument.DataChanged(argument.Value + 1); button.Content($"Value {argument.Value} · +1"); });
            return button;
        }
    }

    private static string ImageData()
    {
        using var stream = new MemoryStream();
        stream.Write(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 });
        byte[] header = [0, 0, 0, 96, 0, 0, 0, 48, 8, 2, 0, 0, 0];
        Chunk("IHDR", header);
        using var pixels = new MemoryStream();
        using (var compressed = new System.IO.Compression.ZLibStream(pixels, System.IO.Compression.CompressionLevel.Optimal, leaveOpen: true))
            for (var y = 0; y < 48; y++)
            {
                compressed.WriteByte(0);
                for (var x = 0; x < 96; x++) compressed.Write(new byte[] { 100, (byte)(y * 4), (byte)(x * 2) });
            }
        Chunk("IDAT", pixels.ToArray());
        Chunk("IEND", []);
        return "data:image/png;base64," + Convert.ToBase64String(stream.ToArray());

        void Chunk(string type, byte[] data)
        {
            var name = System.Text.Encoding.ASCII.GetBytes(type);
            Span<byte> number = stackalloc byte[4];
            System.Buffers.Binary.BinaryPrimitives.WriteInt32BigEndian(number, data.Length);
            stream.Write(number); stream.Write(name); stream.Write(data);
            uint crc = uint.MaxValue;
            foreach (var value in name.Concat(data))
            {
                crc ^= value;
                for (var bit = 0; bit < 8; bit++) crc = (crc >> 1) ^ ((crc & 1) != 0 ? 0xEDB88320U : 0U);
            }
            System.Buffers.Binary.BinaryPrimitives.WriteUInt32BigEndian(number, ~crc);
            stream.Write(number);
        }
    }

    private sealed record DemoCase(string Title, string Description, Func<UIElement> Build);
}
