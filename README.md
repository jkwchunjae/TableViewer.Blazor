# TableViewer.MudBlazor
Spread anything!!

임의의 .NET 객체를 중첩 표로 표시하고, 옵션에 따라 편집하는 컴포넌트입니다.

| 버전 | 프로젝트 | UI |
| --- | --- | --- |
| `TableViewer.MudBlazor` | `src/TableViewerBlazor` | Blazor / MudBlazor |
| `TableViewer.MewUI` | `src/TableViewer.MewUI` | 네이티브 MewUI 컨트롤 라이브러리 |

MewUI 버전은 원본의 뷰어·편집기·조건부 옵션·액션·커스텀 편집기를 포팅합니다. 웹 프레임워크를 참조하지 않고 C# Markup으로 중첩 표를 구성합니다.

- [TableViewer 기능·옵션·포팅 대응과 검증](docs/TableViewer-기능.md)
- [MewUI 자체의 특징](docs/MewUI-특징.md)

## MewUI 데모 실행

.NET 10 SDK와 Windows에서 실행합니다.

```powershell
dotnet build TableViewer.MewUI.slnx
dotnet test test/TableViewer.MewUI.Tests
dotnet run --project samples/TableViewer.MewUI.Demo
```

실제 Windows Direct2D 백엔드에서 10개 예제의 구성·레이아웃과 코드 강조·이미지 fallback·picker 생성을 자동 검사할 수도 있습니다. 앱이 검사 후 종료하고 JSON 보고서를 저장합니다. 마우스·키보드 조작을 대신하는 검사는 아닙니다.

```powershell
dotnet run --project samples/TableViewer.MewUI.Demo -c Release -- --smoke-test artifacts/mewui-smoke.json
```

데모의 열 가지 예제는 값·중첩 객체, 객체 배열·액션, 컬렉션·사전, JSON/YAML 코드 보기, 링크·이미지, 레이아웃·열 선택, 로딩·빈 상태, 필드별 편집·검증, 목록 편집, 사용자 편집기 1~5를 보여줍니다.

## 컨트롤 사용

플랫폼 앱은 MewUI의 플랫폼/렌더링 백엔드를 등록하고 라이브러리의 컨트롤을 창에 배치합니다. 라이브러리 자체는 특정 OS 백엔드를 등록하지 않습니다.

```csharp
using Aprillz.MewUI;
using Aprillz.MewUI.Controls;
using TableViewer.MewUI.Options;
using NativeTableViewer = TableViewer.MewUI.TableViewer;

var people = new[] { new { Name = "Ada", Address = new { City = "Seoul" } } };
var viewer = new NativeTableViewer(people, new TvOptions
{
    Title = "People",
    GlobalOpenDepth = 2
});

Application.Create().UseWin32().UseDirect2D()
    .BuildMainWindow(() => new Window()
        .Title("People").Resizable(900, 640)
        .Content(new ScrollViewer().Content(viewer)))
    .Run();
```

`TableEditor<T>`는 같은 방식으로 배치합니다. `TeOptions`로 필드별 변환·입력 속성·검증을 설정하고 `DataChanged`, `IsValidChanged`, `ErrorsChanged`를 구독합니다. 외부에서 객체를 변경한 뒤에는 UI 스레드에서 `Refresh()`를 호출합니다.

```csharp
var editor = new TableViewer.MewUI.TableEditor<Person>(person, options)
{
    DataChanged = changed => SaveAsync(changed),
    ErrorsChanged = errors => ShowErrorsAsync(errors)
};
await editor.Validate();
```

MudBlazor의 UI enum은 `TableViewer.MewUI.Options`의 값으로 바뀌며, Razor `RenderFragment`는 MewUI `Element` 팩터리로 바뀝니다. 지원 CSS 선언과 기본 spacing 클래스는 네이티브 속성으로 변환합니다. 앱 고유의 DOM/CSS는 네이티브 `Configure`, `StyleName`, `StyleSheet`로 옮깁니다. 상세 차이와 원본에서 선언만 되어 있던 속성은 기능 문서에 구분합니다.

## 패키지 생성

```powershell
dotnet pack src/TableViewer.MewUI -c Release -o artifacts
```

임의 객체의 속성과 필드를 리플렉션으로 탐색하므로 trimming/NativeAOT를 사용할 경우 앱이 표시·편집할 모델 메타데이터를 보존해야 합니다. 현재 실행 검증 플랫폼은 Windows Direct2D입니다.
