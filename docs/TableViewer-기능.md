# TableViewer 기능과 MewUI 포팅 기준

이 문서는 `src/TableViewerBlazor/TableViewerBlazor.csproj`가 배포하는 **TableViewer.MudBlazor 0.3.6**을 기준으로 한다. 프로젝트 폴더·네임스페이스는 `TableViewerBlazor`이지만 NuGet 패키지 이름은 `TableViewer.MudBlazor`다. MewUI 버전의 목표는 이 패키지의 데이터 표시, 편집, 옵션, 이벤트, 사용자 지정 UI를 같은 용도로 사용할 수 있게 옮기는 것이다.

단순 객체 탐색기, JSON 파일 도구, 트리 목록을 따로 만드는 것이 포팅의 완료 조건이 아니다. 원본처럼 객체의 값은 키/값 표로, 객체 컬렉션은 속성을 열로 하는 표로 보여주고, **셀 안에서 중첩 데이터와 편집기를 재귀적으로 구성**해야 한다.

## 문서를 읽는 기준

- **원본 계약**은 현재 저장소의 공개 타입과 실제 Razor/C# 구현에서 확인한 동작이다.
- **대응 방식**은 같은 기능을 MewUI 컨트롤, 이벤트, 스타일로 표현하는 방법이다. 브라우저 전용 타입을 그대로 참조하는 것은 목표가 아니다.
- 아래 기능 ID는 구현·테스트·수동 검증을 연결하는 기준이다. 기능을 나열한 사실만으로 검증 완료를 뜻하지 않는다. 마지막 검증표의 확인란은 실행 근거가 있는 경우에만 채운다.
- 원본에 선언만 있거나 연결되지 않은 옵션은 별도 절에서 구분한다. 원본 버그를 의도된 계약처럼 복제하지 않는다.

## 공개 진입점

| 진입점 | 원본 계약 | MewUI 대응 |
| --- | --- | --- |
| `TableViewer` | `Data: object?`, `Loading: bool`, `Options: TvOptions`. 루트 깊이는 1, 최초 펼침 깊이는 `GlobalOpenDepth` | 같은 입력을 받는 재사용 `UserControl` |
| `TableEditor<T>` | `Data`, `DataChanged`, `Options: TeOptions`, `IsValidChanged`, `ErrorsChanged`, `Validate()` | 네이티브 편집기와 비동기 변경·검증 알림 |
| `TableEditor1` … `TableEditor5` | 1~5개의 `TParent, TItem` 형식 쌍과 형식이 지정된 사용자 편집 UI | `Func<CustomEditorArgument<TParent,TItem>, Element>` 형태의 팩터리 |
| `TvOptions`, `TeOptions` | 보기·편집 정책을 데이터와 분리 | 같은 옵션 구조를 유지하며 MudBlazor UI 타입을 중립/네이티브 타입으로 대체 |
| `AddTableViewer()` | Blazor DI에 날짜/시간 서비스를 등록 | 앱이 날짜 정책·플랫폼 서비스를 직접 소유. Blazor DI가 필수 의존성이 되지 않음 |
| `DateTimeOptionProvider` | JS에서 브라우저 시간대 정보를 읽어 날짜 서비스에 저장 | OS/.NET 시간대 사용. 브라우저 provider가 필요 없는 이유를 날짜 정책에 명시 |

## 데이터 표시

| ID | 기능 | 원본의 구체적 동작 | 포팅 시 확인할 사항 |
| --- | --- | --- | --- |
| V01 | `null` | 녹색 이탤릭 `null`로 표시 | 빈 객체·빈 문자열과 구분 |
| V02 | 문자열 | `string.Empty`는 흐린 이탤릭 `Empty`, 그 외는 원문 | 줄바꿈·유니코드·선택/복사 가능성 |
| V03 | 숫자 | `sbyte`, `byte`, `short`, `ushort`, `int`, `uint`, `long`, `ulong`, `nint`, `nuint`, `float`, `double`, `decimal` | 값의 정밀도를 줄이지 않고 표시 |
| V04 | 논리값 | `TRUE`는 굵게, `FALSE`는 흐리게 | 값뿐 아니라 시각적 구분 |
| V05 | 열거형 | 열거형 문자열 | 이름과 조합 플래그 |
| V06 | 날짜/시간 | `DateTime`, 지정 형식 또는 상대 시간, 시간대 보정 | 형식·과거/미래 상대값·현지 시간대 |
| V07 | 객체 | 읽을 수 있는 public 속성과 public 필드의 키/값 표. `Type` 형 멤버 제외 | 속성/필드 모두, 중첩 셀 재귀 표시 |
| V08 | 객체 컬렉션 | 배열 및 `IEnumerable<T>`의 객체 원소를 표의 행으로, 첫 non-null 원소의 멤버를 열로 사용 | class·record·struct, null 행, 중첩 값, 행 액션 |
| V09 | 일반 컬렉션 | 인덱스/값 표. 처음 `max(2, ArrayVisibleDepth)`개 표시, `more`마다 표시 수 두 배 | 단순 숫자·문자열·논리값·혼합 컬렉션, more 종료 |
| V10 | 사전 | `IDictionary`의 키·값을 각각 재귀 표시 | 문자열 이외의 키, null 값, `DisableKeys` |
| V11 | 문자열 취급 특성 | `TvStringAttribute`가 멤버, 멤버의 선언 타입 또는 실제 값 타입에 붙으면 `ToString()`으로 표시 | 세 적용 위치, 배열 원소 분류에도 반영 |
| V12 | 로딩/빈 데이터 | `Loading`, `TvContents.LoadingContent`, `NoRecordsContent` | 빈 표와 로딩 표의 콘텐츠 구분 |

타입별 렌더러 선택 순서는 `null` → 사용자 지정 보기 → `TvStringAttribute` → 객체 배열 → 숫자 → enum → bool → DateTime → string → IDictionary → IEnumerable → 객체다. 사용자 지정 보기에는 아래의 C01~C03이 해당한다.

## 보기 옵션과 펼침 정책

| 옵션 | 기본값 | 기능 |
| --- | --- | --- |
| `Title` | `null` | 루트 제목 |
| `GlobalOpenDepth` | `1` | `Depth <= OpenDepth`인 중첩 구조의 초기 펼침 |
| `ArrayVisibleDepth` | `1` | 일반 컬렉션의 최초 표시 개수 하한. 원본 내부 시작값은 2 |
| `VisibleItemCount` | `true` | key/item 개수 표시. 개수 버튼을 눌러 접기 |
| `Buttons` | 빈 목록 | 컬렉션 각 원소/객체 행의 조건부 버튼 |
| `TitleButtons` | 빈 목록 | 루트 제목 옆 버튼. 객체 배열에서는 변환 전 `OriginalData`를 전달 |
| `ColumnVisible` | 빈 목록 | `Before`의 모든 키가 있으면 첫 일치 정책의 `After`에 있는 키만 표시 |
| `DisableKeys` | 빈 목록 | 객체 멤버와 사전의 문자열 키 제외 |
| `Style` | `new TvStyleOption()` | 아래 스타일 정책 |
| `EditorOptions`, `StringLinkOptions`, `ImageViewerOptions` | 빈 목록 | 조건부 사용자 지정 표시 |
| `OpenDepth` | 빈 목록 | 특정 데이터·깊이·경로 조건의 하위 펼침 깊이 |
| `DateTime` | `null` | 날짜 출력 형식과 상대 시간 |
| `Contents` | `Loading...`, 빈 데이터 문구 `null` | 로딩/빈 데이터 문구 |

기능 ID **O01**은 전체/조건부 펼침, **O02**는 컬럼 선택과 숨김, **O03**은 제목·개수·내부 툴바, **O04**는 객체 루트의 비표 레이아웃이다. `Style.UseTable=false`일 때 루트 객체만 표 대신 멤버를 독립 섹션으로 보여주며 하위 객체는 계속 중첩 표를 사용한다.

`TvColumnVisibleOption`의 `Matched`는 `Before.All(...)` 조건이다. 원본 객체/객체 배열 경로에서는 `After`가 선택 필터로 작동하며 원본 멤버 순서를 유지한다. `After` 순서대로 재정렬된다고 가정하지 않는다. 사전에는 `ColumnVisible`이 적용되지 않으며 `DisableKeys`만 적용된다.

### 스타일

**S01** `TvStyleOption`은 `UseTable=true`, `Dense=true`, `SuperDense=true`, `Hover=false`, `Striped=false`, `Bordered=true`, `VisibleInnerToolbar=true`가 기본이다. `RootClass`, `LoadingProgressColor`, `TitleStyle`, `ThStyle`을 별도로 제공한다. 제목 스타일 기본은 17px·굵게, 머리글 스타일은 굵게·회색 배경이다.

**S02** `TvButtonStyle`은 `Variant`, `Size`, `Color`, `Style`, `StartIcon`, `EndIcon`, `IconSize`, `IconColor`, `Ripple`, `DropShadow`, `Dense`, `SuperDense`를 가진다. 두 번째 클릭에는 `TvSecondButtonStyle`의 별도 variant·색·아이콘 색을 사용한다. 한 번 실행 버튼의 진행 표시도 색·크기를 별도로 지정한다.

**S03** `TvTooltipOption`은 텍스트와 `TvTooltipProperty`를 갖는다. 후자는 비활성화·표시 상태/알림·화살표·색·지연/지속 시간·배치·클릭/포커스/호버 표시를 지정한다.

CSS class/style 문자열과 Material SVG 아이콘은 네이티브 화면에서 자동으로 적용되지 않는다. MewUI 포트는 지원하는 네이티브 속성과 스타일 팩터리로 의미를 옮기고, 지원하지 않는 브라우저 전용 항목을 문서화해야 한다. 이름만 남기고 동작을 무시한 상태를 스타일 동등 지원으로 표시하지 않는다.

현재 MewUI 공개 편집 속성에는 `Configure: Action<Control>`가 추가되어 네이티브 크기·색·스타일·동작을 직접 구성할 수 있다. 남겨진 `Class`, CSS `Style`, HTML `UserAttributes`, ARIA 식별자는 이 호환 데이터만으로 DOM을 만들지 않는다. `StyleName`/`StyleSheet` 또는 `Configure`로 옮겨야 한다. 애니메이션, popup origin, Material icon의 세부 외형은 네이티브 컨트롤 방식으로 대응하며 웹 화면과 pixel 단위로 같은 외형을 보장한다는 의미가 아니다.

## 조건부 사용자 지정 표시

| ID | API | 기능과 조건 |
| --- | --- | --- |
| C01 | `TvEditorOption<T>`, `TvJsonEditorOption<T>`, `TvYamlEditorOption<T>` | 조건에 맞는 값을 직렬화하여 읽기 전용 코드 보기로 표시. `Language`, `Serializer`, `LayoutMaxSize: EditorSize(Height,Width)` |
| C02 | `TvStringLinkOption<TParent,TValue>` | 부모·값·깊이·경로 조건에 따라 문자열 표시를 링크로 바꿈. `Href(parent,value)` |
| C03 | `TvImageViewerOption<TParent,TValue>` | 같은 조건으로 이미지 표시. 주 URL 실패 시 fallback URL, Alt, Fluid, Width/Height, Elevation, ObjectFit/ObjectPosition |
| C04 | `TvStringJsonConverter<T>`, `TvStringYamlConverter` | 직렬화 시 대상 객체를 `ToString()` 문자열로 기록. YAML은 `TvStringAttribute` 타입을 자동 인식 |

`TryGetCustomOption`은 **EditorOptions → StringLinkOptions → ImageViewerOptions** 순서로 모든 정책을 이어 붙여 처음 일치한 하나를 선택한다. 일반 `TvEditorOption<T>`는 조건이 없으면 타입이 맞는 모든 값에 적용한다. JSON 옵션 기본 serializer는 들여쓴 JSON이다. YAML 옵션 기본 serializer는 `TvStringYamlConverter`를 등록한 YamlDotNet serializer다.

원본 코드 뷰어는 Monaco를 읽기 전용으로 사용하고 줄 번호·미니맵·glyph margin·folding을 끈다. 콘텐츠의 줄 수·최대 줄 길이로 크기를 계산한 뒤 `LayoutMaxSize`로 제한한다. 따라서 단순 한 줄 `ToString()` 표시는 C01의 대체가 아니다. MewUI에서는 네이티브 코드/텍스트 표시로 직렬화 결과, 읽기 전용 동작, 크기 제한을 대응시킨다. 구문 강조나 Monaco 전용 편집 명령의 차이는 별도로 표기한다.

## 행·제목·툴바 액션

모든 버튼은 `ITvButton`의 `Condition(object?,depth)`, `Label`, `Style`, `TooltipOption`을 공유한다. 제네릭 구현은 대상 값이 `T`인 경우에만 사용자 조건을 평가한다.

| ID | API | 상태와 동작 |
| --- | --- | --- |
| A01 | `TvAction<T>` | `Func<T,Task>` 실행. 기본 label `ACTION`, 완료 label `DONE` |
| A02 | `TvOneClickAction<T>` | 기본 → 진행 → 완료. 실행 중 중복 입력 방지, `Reusable=false`이면 완료 상태, true이면 기본 상태 복귀 |
| A03 | `TvDoubleClickAction<T>` | 첫 클릭은 확인 상태로 전환, 제한 시간 안 두 번째 클릭에서 실행. `ResetDelay` 기본 2초, 시간이 지나면 첫 상태로 복귀 |
| A04 | `TvPopupAction<T>` | 동적 제목·내용 확인 모달. 닫기와 확인 label/style 지정. 확인에서만 실행하고 닫음 |
| A05 | `TvLink<T>` | 동적 Href와 Target. 네이티브에서는 호스트 탐색 콜백/OS URL 열기로 대체 |
| A06 | 버튼 조건·툴팁 | 값·깊이별 노출과 해당 버튼 스타일/툴팁 |

`TvAction`의 실제 렌더러는 일반 액션, 한 번 실행, 두 단계 확인, 팝업을 구분한다. `TvDoubleClickAction`은 OS의 빠른 더블클릭 이벤트와 다르다. 첫 클릭 뒤 확인 상태가 유지되는 두 단계 버튼이다.

`InnerButtonsOptions`는 `CloseLabel="Close"`, `ConfirmLabel="Confirm"`과 두 버튼 스타일을 제공한다. 원본 popup 기본은 full width, Escape 닫기 허용, header 표시다. 네이티브 모달은 owner·포커스·Escape 동작을 함께 검증한다.

## 자동 편집

### 필드 선택과 변경 전파

**E01** 객체 편집은 public 읽기/쓰기 속성과 public 필드를 편집한다. `Type` 멤버와 `[TeIgnore]` 멤버는 제외한다. 멤버 변경을 객체에 반영하고 루트 `DataChanged`까지 전달한다. 중첩 경로와 부모 값을 사용자 편집기에 전달한다.

**E02** 필드 옵션의 우선순위는 다음과 같다.

1. 멤버의 `TeFieldAttribute.Id`와 옵션의 nonempty `Id`가 정확히 일치한 첫 옵션.
2. 실제 데이터 타입 또는 바로 위 base type의 `FullName`과 `ITeGenericTypeOption.TypeName`이 일치하는 첫 옵션.
3. 기본 string/bool/number/DateTime/TimeSpan/primitive list 편집기.
4. 값이 null인 경우 선언된 멤버 타입을 이용한 기본 편집기 또는 generic 옵션.
5. 일치하지 않으면 객체 재귀 편집.

옵션 검색 목록은 `TextFieldOptions` → `NumericFieldOptions` → `SelectBoxOptions` → `RadioOptions` → `CheckBoxOptions` → `AutocompleteOptions` → `SwitchOptions` → `CustomEditorOptions.CustomEditors` → `ObjectListEditorOptions` → `ListEditorOptions` 순서다.

| ID | 필드 API | 계약 |
| --- | --- | --- |
| E03 | `TeTextFieldOption`, `<TValue>` / `[TeTextField]` | string 또는 `TeTextFieldConverter<TValue>`로 문자열과 양방향 변환. 유효성·입력 이벤트·속성 |
| E04 | `TeNumericFieldOption<TNumber>`, `<TValue,TNumber>` / `[TeNumericField]` | 모든 지원 숫자 타입, 기본값, `TeNumericFieldConverter`, min/max/step·spin button·형식/문화권 |
| E05 | `TeSelectBoxOption<TValue>` / `[TeSelectBox]` | 값/텍스트/default 항목 목록, 선택·clear·표시 정책 |
| E06 | `TeRadioOption<TValue>` / `[TeRadio]` | 값/텍스트/default 항목, 개별 항목 속성과 그룹 검증 |
| E07 | `TeCheckBoxOption`, `<TValue>` / `[TeCheckBox]` | bool 또는 `TeBoolConverter<TValue>`로 변환한 값. 라벨·배치·상태 |
| E08 | `TeSwitchOption`, `<TValue>` / `[TeSwitch]` | bool 또는 converter 값. 체크/비체크 상태의 색·thumb 표현 |
| E09 | `TeAutocompleteOption`, `<TValue>` / `[TeAutocomplete]` | 후보 항목, 문자열 변환, 대소문자를 무시하는 기본 contains 또는 `CustomSearchFilter`. 검색어가 비면 전체 후보 |
| E10 | `TeListEditorOption<TListItem>`, `<TValue,TListField,TListItem>` / `[TeListEditor]` | primitive/사용자 타입 목록을 셀 편집기로 표시. converter 왕복, 번호, 추가/삭제 정책 |
| E11 | `TeObjectListEditorOption<TListItem>` / `[TeObjectList]` | 객체 목록의 각 멤버를 편집 가능한 열로 표시. 중첩 편집·제목·툴바·행 추가/삭제 |

DateTime과 TimeSpan의 기본 편집기는 전용 날짜 컨트롤이 아니라 `TryParse` 기반 텍스트 converter다. 목록 옵션 `Create(defaultValue)`는 지정 기본값을 추가하는 `AddItemAction`을 만든다. 추가/삭제 버튼은 해당 action이 null이 아닌 경우에만 활성화된다. `ShowNumber`는 1부터 시작하는 번호 열이다.

### 변환·검증·이벤트

**E12** `ITeConverter<TValue,TField>`는 사용자 값에서 필드 값으로 `ToField`, 필드 값에서 사용자 값으로 `FromField`를 제공한다. `TeConverter`, `TeTextFieldConverter`, `TeNumericFieldConverter`, `TeBoolConverter`, `TeListEditorConverter`는 서로 다른 저장 타입을 같은 편집 UI로 다루기 위한 계약이다. 형 변환 실패를 조용히 잘못된 값으로 덮어쓰지 않는지 검증한다.

**E13** `TeValidation<T>`와 `TeAsyncValidation<T>`는 각각 동기/비동기 판정과 오류 메시지를 제공한다. 텍스트·숫자는 `FromField`로 복원한 사용자 데이터에 검증을 적용한다. 검증은 등록 순서대로 await하며 실패 메시지 또는 예외 메시지를 모은다. 폼은 `Validate()`, `IsValidChanged`, `ErrorsChanged`를 제공한다. 필수 입력은 `Required`, `RequiredError`, 명시 오류는 `Error`, `ErrorText`로 지정한다.

**E14** `TeTextFieldEvent<T>`는 `ValueChanged`, `OnClearButtonClick`, `OnDebounceIntervalElapsed`, `OnAdornmentClick`, `OnBlur`, `OnInternalInputChanged`, `OnKeyDown`, `OnKeyPress`, `OnKeyUp`을 제공한다. native key/focus/pointer 이벤트 타입으로 옮겨도 이벤트 발생 시점과 변경 데이터의 의미는 유지해야 한다.

### 사용자 지정 편집기

**E15** `TeCustomEditorOptionGroup`은 `CustomEditor1`~`CustomEditor5`를 제공한다. `TeCustomEditorOption`은 `Id`, `Condition(MemberInfo?,Type?)`, 렌더 팩터리를 갖는다. 멤버의 `[TeCustomEditor(id)]`가 우선하며, 없으면 조건이 맞는 첫 옵션을 사용한다.

`CustomEditorArgument<TParent,TItem>`은 `Parent`, `Value`, 비동기 `DataChanged`, `ParentChanged` 이벤트를 제공한다. 객체 목록의 같은 행에서 다른 필드가 바뀌면 해당 부모를 사용하는 사용자 편집기에도 통지한다. 1~5 슬롯 래퍼는 typed argument와 공통 `ICustomEditorArgument`를 연결하고 구독을 해제해야 한다. 단순 `Func<object,string>` formatter는 사용자 편집기의 대체가 아니다.

### 편집기 표시·동작 옵션

다음 표는 공개 property wrapper가 드러내는 기능 범위다. 실제 원본 Razor에서 전달하지 않는 속성은 아래 원본 제약 절도 확인한다.

| 그룹 | 관련 타입 | 기능 |
| --- | --- | --- |
| 기본 컴포넌트 | `TeComponentBaseProperty` | Class, Style, Tag, UserAttributes |
| 폼 | `TeFormComponentProperty` | Required/RequiredError, Error/ErrorText/ErrorId, Culture |
| 입력 공통 | `TeBaseInputProperty` | Disabled, ReadOnly, FullWidth, Immediate, Underline, helper text/focus, adornment text/icon/click, dirty-only 검증, placeholder, counter, MaxLength, Label, AutoFocus, Lines, InputMode, Pattern, ShrinkLabel, Format, Variant/Margin/Typo |
| debounce | `TeDebouncedInputProperty` | DebounceInterval |
| 텍스트 | `TeTextFieldProperty` | InputType, Clearable/ClearIcon, AutoGrow/MaxLines, Mask |
| 숫자 | `TeNumericFieldProperty`, `<T>` | Clearable, InvertMouseWheel, Min/Max/Step, HideSpinButtons, 숫자 InputMode/Pattern |
| bool 공통 | `TeBooleanInputProperty` | Disabled, ReadOnly |
| checkbox | `TeCheckBoxProperty` | Label/배치, KeyboardEnabled, Dense/Size, checked·unchecked·indeterminate icon, TriState, 색, Underline |
| switch | `TeSwitchProperty` | Label/배치, Size, checked·unchecked thumb icon/색, Underline |
| radio | `TeRadioGroupProperty`, `TeRadioItemProperty` | 그룹 및 개별 항목 disabled/read-only, 항목 배치·밀도·크기·색 |
| select | `TeSelectBoxProperty`, `<T>` | 메뉴/목록 스타일, open/close/clear icon, SelectAll/SelectAllText, 선택 문자열 함수·delimiter, MaxHeight, anchor/transform origin, Strict, Clearable, LockScroll, 선택 상태 icon |
| autocomplete | `TeAutocompleteProperty` | before/after/no/more item template, MaxItems, MaxHeight, progress template, MinCharacters, DebounceInterval, Strict, Clearable, ResetValueOnEmptyText, CoerceText/Value, TextUpdateSuppression, origin·아이콘·목록 스타일 |
| 상태별 라벨 | `ITeLabelOptions`, `TeLabelOptions<T>` | 부모 조건, 선택/비선택 라벨 함수, 라벨 배치 |
| 편집 표 | `TeStyleOption` | Dense, SuperDense, Hover, Striped, Bordered, RootClass |

## 원본 제약과 포팅 시 명시할 정정

이 목록은 기능을 생략하기 위한 예외가 아니라 **현재 소스의 실제 상태와 의도된 API를 혼동하지 않기 위한 기록**이다.

- View의 사용자 조건에 전달되는 path는 현재 대부분 문자열 리터럴 `"path"`다. MewUI에서 실제 경로를 전달하면 정정으로 기록하고 경로 조건 테스트를 추가한다.
- `TvObjectView.ChildrenOpen()`은 조건부 펼침 깊이를 계산하지만 결과 `ThisOpenDepth`가 Razor의 자식 `OpenDepth`에 반영되지 않는다. MewUI에서는 공개 옵션의 의도대로 적용했는지 검증한다.
- `TvYamlEditorOption<T>.Language` 기본값이 소스에서 `"json"`이다. 실제 serializer는 YAML이다. 네이티브 코드 보기의 언어는 YAML로 정정 가능하다.
- 날짜 provider의 JS offset 갱신은 코드에 타이밍 이슈 주석이 있으며 offset 분을 정수 시간으로 나누는 구현이다. OS 시간대의 분 단위 offset과 DST를 사용하는 구현과 차이를 기록한다.
- JSON/YAML string converter의 역직렬화 메서드는 `NotImplementedException`이다. 직렬화 전용 계약이다.
- 원본 `TvColumnVisibleOption.NewKeys`는 문자열 overload와 멤버 overload의 순서 의미가 다르다. 실제 Viewer 경로는 멤버 원래 순서를 유지한다.
- `TeSelectBoxProperty`에는 복수 선택 표현 관련 속성이 있지만 원본 `TeSelectBox.razor`는 `T="object"`, 단일 `Value`/`ValueChanged`를 사용하고 `MultiSelection`을 켜지 않는다. `SelectAll`, 선택 문자열 함수·delimiter 선언만으로 실제 복수 선택 편집을 제공한다고 간주하지 않는다.
- `TeLabelOptions<T>`는 공개 타입이나 현재 편집기에서 소비하는 연결이 없다.
- 원본 목록 삭제는 remove 옵션의 액션 함수를 호출하는 대신 목록에서 삭제하고 label/style을 사용한다. 네이티브에서 콜백 실행을 추가하면 변경점으로 명시한다.
- 원본 객체 목록의 툴바 버튼은 목록 자체 대신 `(index,obj)`의 enumerable을 전달한다. 네이티브 API가 원본 목록을 전달한다면 그 차이를 명시한다.
- `TeTextFieldEvent`의 `OnKeyDown`·`OnKeyPress`·`OnKeyUp`은 원본 Razor에 연결되지 않는다. `OnClearButtonClick`, debounce, adornment, blur, internal input은 연결되어 있다. MewUI에서 키 이벤트를 연결하면 공개 계약의 의도를 보완한 정정이다.
- 원본 select/radio 항목의 `Default` 플래그는 공개 항목에 존재하지만 Razor에서 소비하지 않는다. 최초 선택은 입력 `Data`에 의해 결정된다.
- `TeFormComponentProperty.Error`와 `ErrorText`는 wrapper에 있지만 원본 텍스트 Razor에서 직접 전달하지 않는다. 폼 검증은 `Validations`, Required 등을 통해 수행한다.

## 포팅 검증 체크리스트

다음은 원본과 같은 입력을 사용해 비교할 시나리오다. 단위 테스트는 정책·변환·변경 전파를, 실제 Windows 화면 검증은 입력·포커스·모달·표현을 담당한다.

| 범위 | 필요한 시나리오 | 자동 검증 | 화면 검증 |
| --- | --- | --- | --- |
| V01~V06 | null/Empty/숫자 13종/bool/enum/date format·relative | [ ] | [ ] |
| V07~V10 | 객체 public field/property, 객체 배열/record/struct/null행, primitive more, 사전 key/value 중첩 | [ ] | [ ] |
| V11~V12 | 멤버·타입 특성, 로딩·빈 표 문구 | [ ] | [ ] |
| O01~O04 | 전역/조건부 펼침, more, column 정책/DisableKeys, 제목/개수/툴바, UseTable=false | [ ] | [ ] |
| C01~C04 | JSON/YAML/custom serializer·크기 제한, string link, 이미지/fallback/fit, converter | [ ] | [ ] |
| A01~A06 | 조건부 노출, async 진행/완료/재사용, 2단계 제한시간, popup 취소/확인, link, tooltip | [ ] | [ ] |
| S01~S03 | 표 밀도·줄무늬·테두리·호버, 제목/머리글/버튼 스타일, tooltip 동작 | [ ] | [ ] |
| E01~E02 | 객체·중첩·필드, Ignore, attribute/type 우선순위, null 멤버 | [ ] | [ ] |
| E03~E09 | 텍스트·숫자·select·radio·checkbox·switch·autocomplete 실제 입력과 source 변경 | [ ] | [ ] |
| E10~E11 | primitive·객체 목록 추가/삭제, 번호, 중첩 셀, 리스트 converter, toolbar | [ ] | [ ] |
| E12~E14 | converter 왕복, 순차 async 검증·예외·필수, 폼 알림, debounce/clear/focus/key 이벤트 | [ ] | [ ] |
| E15 | custom 1~5 슬롯, typed 부모/값, 같은 행 ParentChanged, 구독 해제 | [ ] | [ ] |

현재 확인란은 검증 결과를 기록하기 전 상태다. 빌드 통과는 기능별 화면 검증을 대신하지 않는다. 플랫폼 전환 차이와 아직 실행하지 않은 시나리오는 별도 기록 없이 완료로 처리하지 않는다.

## 원본 소스 안내

- 공개 진입점: [`TableViewer`](../src/TableViewerBlazor/TableViewer.razor.cs), [`TableEditorBase`](../src/TableViewerBlazor/TableEditorBase.cs), `TableEditor1`~`TableEditor5`.
- 보기 정책: [`TvOptions`](../src/TableViewerBlazor/Options/TvOptions.cs), [`TvElementView`](../src/TableViewerBlazor/Internal/TvComponent/TvElementView.razor), [`TvObjectArrayView`](../src/TableViewerBlazor/Internal/TvComponent/TvObjectArrayView.razor.cs).
- 편집 정책: [`TeOptions`](../src/TableViewerBlazor/Options/TeOptions.cs), [`TeElementEditor`](../src/TableViewerBlazor/Internal/TeComponent/TeElementEditor.razor), [`TeObjectListEditor`](../src/TableViewerBlazor/Internal/TeComponent/TeObjectListEditor.razor.cs).
- 실제 사용 예: [`Home`](../test/TableViewerTest/Components/Pages/Home.razor.cs), [`EditorTest`](../test/TableViewerTest/Components/Pages/EditorTest.razor.cs), [`EditorConverter`](../test/TableViewerTest/Components/Pages/EditorConverter.razor.cs), [`EditorObjectArray`](../test/TableViewerTest/Components/Pages/EditorObjectArray.razor.cs), [`ImageViewTest`](../test/TableViewerTest/Components/Pages/Viewer/ImageViewTest.razor).
