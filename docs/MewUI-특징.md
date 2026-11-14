# MewUI의 특징

이 문서는 TableViewer 기능 목록과 별개로 **MewUI 자체의 개발 모델과 플랫폼 특성**을 정리한다. 예시와 API 기준은 공개 NuGet `Aprillz.MewUI.Core` 및 `Aprillz.MewUI.Windows` **0.22.1**, .NET 10이다. 설치된 패키지의 XML 문서와 공식 MewUI 스킬의 공개 API 사용 지침을 기준으로 작성했다.

## C#으로 UI 트리를 구성한다

MewUI는 C# 객체와 fluent 메서드로 UI를 선언한다. `.Text(...)`, `.Margin(...)` 같은 속성 설정, `.Children(...)` 같은 자식 구성, `.OnClick(...)` 같은 이벤트 연결, `.BindText(...)` 같은 바인딩을 조합한다. XAML, Razor, HTML DOM, 브라우저 JavaScript 런타임은 기본 UI 구성에 필요하지 않다.

```csharp
using Aprillz.MewUI;
using Aprillz.MewUI.Controls;

var name = new ObservableValue<string>("MewUI");

var content = new StackPanel().Vertical().Spacing(8).Children(
    new TextBox().BindText(name),
    new TextBlock().BindText(name, value => $"Hello, {value}"));
```

fluent 메서드가 모두 단순 속성 대입인 것은 아니다. `.OnClick`은 구독을 추가하고 `.Children`·`.Columns`는 컬렉션을 변경한다. 재빌드나 가상화된 셀 재바인딩에서 이를 반복하면 중복 이벤트나 열이 생길 수 있다. 각 overload의 실제 계약을 확인해야 한다.

## 앱, 창, 재사용 컨트롤의 수명이 분리된다

| 구성 | 역할 |
| --- | --- |
| `Application` | 플랫폼·렌더러·테마·종료 정책·메시지 루프 |
| `Window` | 독립 네이티브 창, 크기/위치, 소유 관계, 모달 |
| `UserControl` | 다른 창에 넣는 재사용 UI와 해당 상태 |
| `Element` 반환 메서드 | 상태 경계가 필요 없는 작은 조합 |
| typed template | 반복되는 행·셀의 생성과 재사용 |

`UserControl.OnBuild()`는 첫 레이아웃에서 지연 실행된다. `Window.OnBuild()`는 창 구성을 수행한다. Hot Reload와 Preview는 build 본문을 다시 실행할 수 있으므로, 지속할 상태·명령·서비스는 build 함수 밖의 필드나 앱 상태에 둔다. 구독처럼 한 번만 해야 하는 작업과 반복 가능한 UI 구성도 구분한다.

`ObservableValue<T>`를 `OnBuild` 안에서 새로 만들면 다시 빌드할 때 기존 입력 상태를 잃을 수 있다. 바인딩된 상태는 컨트롤보다 짧게 살지 않아야 한다. disposal 시 바인딩과 앱이 소유한 구독·리소스를 정리한다.

## 플랫폼과 렌더러를 명시적으로 선택한다

Windows 시작의 기본 형태는 다음과 같다.

```csharp
Application.Create()
    .UseWin32()
    .UseDirect2D()
    .UseTheme(ThemeVariant.System)
    .BuildMainWindow(() => new Window()
        .Title("MewUI")
        .Resizable(900, 600, minWidth: 640, minHeight: 420)
        .Content(content))
    .Run();
```

플랫폼 등록과 렌더링 backend 등록은 별개다. 하나의 startup 경로에는 호환되는 플랫폼/렌더러를 각각 하나만 등록한다.

| 대상 | 앱 패키지 | 등록 |
| --- | --- | --- |
| Windows Direct2D | `Aprillz.MewUI.Windows` | `UseWin32().UseDirect2D()` |
| Windows GDI | `Aprillz.MewUI.Windows` | `UseWin32().UseGdi()` |
| Windows MewVG | `Aprillz.MewUI.Windows` | `UseWin32().UseMewVGWin32()` |
| Linux X11 | `Aprillz.MewUI.Linux` | `UseX11().UseMewVGX11()` |
| macOS Metal | `Aprillz.MewUI.MacOS` | `UseMacOS().UseMewVGMetal()` |

컨트롤 라이브러리는 `Aprillz.MewUI.Core`만 참조하고 플랫폼 선택은 최종 앱이 맡을 수 있다. Windows용 demo를 빌드한 사실은 Linux/macOS에서의 실행 검증을 뜻하지 않는다. backend별 성능·그림 품질은 실제 데이터와 화면에서 측정해야 한다.

## 형식이 있는 상태와 바인딩을 사용한다

`ObservableValue<T>.Value`를 바꾸면 연결된 UI가 갱신된다. `Bind`는 `MewProperty<T>`와 상태 또는 다른 컨트롤의 property를 연결한다. `BindText`, `BindIsChecked` 등 편의 메서드도 있다. `INotifyPropertyChanged` 모델은 typed lambda로 연결하고, 중간 객체가 교체될 수 있는 경로는 `BindingPath`로 명시한다.

문자열 `DataContext` 경로나 reflection 기반 dotted-path binding이 필요하지 않다. 데이터 검사기에서 임의 객체의 멤버를 찾기 위해 reflection을 사용하는 것과, UI 바인딩 경로를 문자열로 구성하는 것은 별개다.

양방향 변환은 convert-back 함수를 지정한다. 입력 변환에서 예외가 나면 source 값을 유지하고 `Control.ValidationErrorsProperty`로 오류를 표현할 수 있다. 외부에서 `Text` 속성을 설정하는 것과 사용자가 편집·붙여넣기·undo로 문서를 변경하는 경로는 구분되므로, 입력 테스트는 실제 편집 경로도 확인해야 한다.

한 property를 다시 bind하면 이전 바인딩을 대체한다. `ClearBinding`은 현재 effective value를 유지한다. target 컨트롤이 dispose되면 해당 바인딩도 정리된다.

## 레이아웃은 패널과 측정 제약으로 표현한다

| 컨트롤/패널 | 용도 |
| --- | --- |
| `StackPanel`, `WrapPanel` | 한 방향 배치, 공간에 따른 줄바꿈 |
| `Grid` | `Auto`, 고정, `*` 비율의 행·열 |
| `DockPanel` | 상단 툴바·하단 상태와 남은 본문 |
| `Border` | 테두리·배경·padding을 가진 단일 자식 |
| `ScrollViewer` | 하나의 콘텐츠 트리에 대한 스크롤 |
| `Canvas` | 절대 위치가 필요한 그림/배치 |

크기는 DIP(device independent pixel) 단위다. `Margin`은 바깥 여백, `Padding`은 내부 여백이다. 창 크기는 일반 요소의 `Width/Height` 대신 `Resizable`, `Fixed`, fit-content API로 설정한다. 브라우저 CSS selector, flexbox class, `<table>` markup은 네이티브 패널로 자동 해석되지 않는다.

## 컬렉션은 typed view와 템플릿으로 표시한다

`ObservableCollection<T>`는 추가/삭제 알림에 사용하고 `ItemsView<T>`는 text/key selector를 연결한다. `ListBox`는 항목 선택, `ComboBox`는 압축된 선택 UI, `TreeView`는 계층, `GridView`는 열이 있는 표, `NavigationView`는 앱 탐색을 담당한다.

`GridViewColumn<T>`는 열 제목·폭·텍스트 또는 cell template·정렬 키를 행 타입과 함께 정의한다. 고정 `PixelWidth`, 비율 `StarWidth`, 내용 기반 `AutoWidth`를 선택할 수 있다.

```csharp
new GridView().ItemsSource(rows).Columns(
    new GridViewColumn<Person>().Header("Name")
        .StarWidth(minWidth: 140)
        .Text(person => person.Name)
        .SortBy(person => person.Name, StringComparer.OrdinalIgnoreCase));
```

반복 항목의 템플릿은 **build와 bind**로 나뉜다. build는 재사용할 시각 요소와 안정적인 이벤트 handler를 한 번 만들고, bind는 현재 행 값만 대입한다. 가상화로 같은 컨트롤이 다른 행에 연결될 수 있으므로 이전 행을 잘못 참조하거나 bind마다 이벤트를 더하면 안 된다. 편집 중 `ItemsSource` 전체를 교체하면 컨테이너·포커스가 바뀔 수 있으므로, 값을 바꿀 때마다 표 전체를 재생성하는 방식은 주의해야 한다.

고정 높이 가상화 행에 큰 중첩 표를 넣을 때에는 높이와 레이아웃을 따로 설계해야 한다. `GridView`를 사용했다는 사실만으로 HTML 중첩 표의 모든 배치가 자동 재현되지는 않는다.

## 입력은 네이티브 포커스·routed event·명령으로 처리한다

컨트롤은 key/mouse/focus 이벤트를 노출한다. `KeyDown`은 버블링되고 `Handled`로 후속 처리를 막을 수 있다. `Window.PreviewKeyDown`은 포커스 컨트롤보다 먼저 입력을 관찰한다. 키보드 단축키처럼 여러 UI가 공유하는 동작은 `Command`/`InputMap`으로 묶는 편이 적합하다.

`Focus()`는 컨트롤이 표시된 창에 연결된 뒤 호출한다. 브라우저 `KeyboardEventArgs`·`FocusEventArgs`를 그대로 재사용하는 대신 MewUI 이벤트 의미에 맞게 변환한다. 체크 상태, 선택, text 입력은 컨트롤의 바인딩/이벤트로 source와 연결해야 한다.

drag-and-drop은 source가 data와 allowed effect를 제공하고 target이 `AllowDrop`, 수락 effect를 설정하는 구조다. OS 파일 drop과 앱 내부 객체 이동은 같은 데이터 형식을 사용한다고 가정하지 않는다.

## 테마와 스타일을 UI 타입에 연결한다

초기 테마는 `UseTheme(ThemeVariant.System)`으로 지정하고 실행 중에는 `Application.Current.SetThemeMode(ThemeVariant.Light/Dark/System)`로 바꿀 수 있다. `WithTheme`에서 `theme.Palette`와 `theme.Metrics`를 사용하면 테마가 바뀌어도 배경·글자·테두리가 함께 갱신된다.

`StyleSheet`는 subtree의 타입별/이름별 스타일을 정의한다. `Style.DeriveFromDefault<T>`로 기본 chrome을 유지하며 typed `Setter`와 `StateTrigger`를 추가할 수 있다. `StateTrigger`는 hot/focused/pressed/checked/selected/invalid/read-only 같은 상태 표현에 사용한다. 모든 상태를 pointer handler로 직접 칠하는 방식은 피한다.

CSS 문자열, Material SVG 문자열, DOM user attributes는 MewUI 스타일 그 자체가 아니다. 네이티브 컨트롤의 property·색·크기·아이콘 요소·스타일 팩터리로 변환해야 한다. 브라우저 전용 의미가 없는 항목은 대체 API와 차이를 설명한다.

## 모달, 파일, 이미지와 비동기 작업을 앱이 소유한다

- 알림과 확인은 `MessageBox.NotifyAsync`, `ConfirmAsync`, `AskYesNoAsync`, `PromptAsync` 등의 관리형 API로 구성할 수 있다.
- 앱 전용 모달은 `Window.ShowDialogAsync(owner)`를 사용한다. owner 지정은 modality·포커스·창 쌓임 순서를 위해 필요하다.
- `FileDialog.OpenFile/SaveFile/SelectFolder`는 플랫폼 native dialog를 우선하거나 관리형 대화상자를 사용할 수 있다. 취소 결과 `null`을 파일 경로로 사용하지 않는다.
- `ImageSource.FromFile`, `FromResource`, `FromBytes`로 이미지 source를 만든다. 네트워크 이미지는 별도 다운로드·취소·fallback·수명 관리가 필요하다. HTML `<img>`의 URL 처리와 같다고 가정하지 않는다.
- async 이벤트 경계에서는 예외를 잡아 UI에 알리고, `.Wait()`/`.Result`로 UI thread를 막지 않는다. UI 상태 변경은 UI thread에서 수행한다.

앱 수명 기본은 마지막 창이 닫히면 종료하는 `OnLastWindowClose`다. tray처럼 창 없이 살아 있어야 하면 `OnExplicitShutdown`과 명시적인 종료 경로를 정한다. `OnMainWindowClose`는 실제 `MainWindow` 지정이 전제다.

## 배포와 선택 확장

일반 앱은 대상 플랫폼의 metapackage를 사용한다. SVG, Skia, docking, charts, WebView2 같은 확장은 필요할 때만 추가하고 같은 MewUI 버전으로 맞춘다. Core에 해당 기능이 포함되어 있다고 가정하지 않는다.

Windows 배포에서는 startup renderer와 `MewUIBackend` publish filter가 일치해야 한다.

```text
dotnet publish -c Release -r win-x64 --self-contained true -p:MewUIBackend=Direct2D
```

이 속성은 startup 등록을 바꾸지 않고 불필요한 backend 파일을 publish 결과에서 제외하는 역할이다. `PublishAot=true`·trimming을 사용할 경우 실제 publish와 결과 실행으로 검증한다. MewUI가 NativeAOT를 고려한 프레임워크라는 사실만으로, 임의 타입 reflection·serializer를 사용하는 응용 라이브러리가 자동으로 AOT 안전해지는 것은 아니다. 문화권·날짜·정렬이 중요한 앱에는 검증 없이 `InvariantGlobalization=true`를 적용하지 않는다.

## Blazor/MudBlazor와의 차이

| 항목 | Blazor/MudBlazor | MewUI |
| --- | --- | --- |
| UI 선언 | Razor component/RenderFragment | C# control/Element factory |
| 실행 표면 | 브라우저 DOM | 네이티브 Window와 자체 UI tree |
| 상태 통지 | component parameter/EventCallback/render | typed property/binding/event/explicit rebuild |
| 반복 셀 | Razor row fragment | typed build/bind template 또는 패널 조합 |
| 스타일 | CSS, MudBlazor theme/enum | Palette/Metrics, typed property, StyleSheet |
| 탐색 | Href/Target, browser route | 호스트 navigation 또는 OS URL 열기 |
| 시간대 | JavaScript Intl/브라우저 offset | .NET/OS 시간대 |
| 코드 표시 | Monaco/JS module | 선택 확장 `Aprillz.MewUI.MewvalonEdit`의 native `TextEditor`·highlighting |
| 서비스 | ASP.NET DI와 JS interop | 앱이 소유하는 일반 .NET service/platform API |
| 검증 | MudForm/field validation | native binding validation과 앱의 폼 정책 |

## 근거와 확인 방법

공식 프로젝트: [aprillz/MewUI](https://github.com/aprillz/MewUI). 패키지 nuspec에 기록된 0.22.1 source commit은 `77f453ec9d625de9810a0e252795b77c31109c53`이다.

설치한 버전의 계약은 다음으로 확인한다.

```text
dotnet list package
dotnet nuget locals global-packages --list
```

보고된 package cache의 `aprillz.mewui.core/0.22.1/lib/net10.0/Aprillz.MewUI.xml`과 동일 버전 assembly가 API 근거다. 최신 upstream 예제는 조합 방법을 이해하는 보조 자료이며, 설치 버전과 다른 API가 있을 때에는 package-only compile 결과를 따른다. 빌드, 실제 입력, 화면, publish, 타 플랫폼 실행은 서로 다른 검증 단계로 기록한다.
