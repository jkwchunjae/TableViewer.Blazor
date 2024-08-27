using TableViewerBlazor.Internal.TeComponent;

namespace TableViewerBlazor.Options;

public class TeOptions
{
    public string? Title { get; set; }
    public TeStyleOption Style { get; set; } = new();
    public List<ITeTextFieldOption> TextFieldOptions { get; set; } = [];
    public List<ITeNumericFieldOption> NumericFieldOptions { get; set; } = [];
    public List<ITeSelectBoxOption> SelectBoxOptions { get; set; } = [];
    public List<ITeRadioOption> RadioOptions { get; set; } = [];
    public List<ITeCheckBoxOption> CheckBoxOptions { get; set; } = [];
    public List<ITeAutocompleteOption> AutocompleteOptions { get; set; } = [];
    public List<ITeSwitchOption> SwitchOptions { get; set; } = [];
    public TeCustomEditorOptionGroup CustomEditorOptions { get; set; } = new();
    public List<ITeObjectListEditorOption> ObjectListEditorOptions { get; set; } = [];
    public List<ITeListEditorOption> ListEditorOptions { get; set; } = [];
    public List<ITvButton> ToolbarButtons { get; set; } = [];
}

public static class TeOptionsExtension
{
    public static bool TryGetFieldOption(this TeOptions options,
        MemberInfo? memberInfo, TeEditorBase teBase,
        out ITeFieldOption? fieldOption)
    {
        var fieldOptions = Enumerable.Empty<ITeFieldOption>()
            .Concat(options.TextFieldOptions)
            .Concat(options.NumericFieldOptions)
            .Concat(options.SelectBoxOptions)
            .Concat(options.RadioOptions)
            .Concat(options.CheckBoxOptions)
            .Concat(options.AutocompleteOptions)
            .Concat(options.SwitchOptions)
            .Concat(options.CustomEditorOptions.CustomEditors)
            .Concat(options.ObjectListEditorOptions)
            .Concat(options.ListEditorOptions)
            .ToArray();

        // 1. ID가 양쪽에 정확히 일치 하는 경우
        var fieldAttribute = memberInfo?.GetCustomAttribute<TeFieldAttribute>();
        if (fieldAttribute != default)
        {
            var option = fieldOptions
                .Where(o => !string.IsNullOrEmpty(o.Id))
                .FirstOrDefault(o => o.Id == fieldAttribute.Id);
            if (option != default)
            {
                fieldOption = option;
                return true;
            }
        }

        // 2. data type으로 옵션을 추출
        if (teBase.Data != default)
        {
            ITeFieldOption? option;
            var dataTypeName = teBase.Data.GetType().FullName;
            option = fieldOptions
                .Where(o => o is ITeGenericTypeOption)
                .Select(o => o as ITeGenericTypeOption)
                .FirstOrDefault(o => o!.TypeName == dataTypeName);
            if (option != default)
            {
                fieldOption = option;
                return true;
            }

            option = teBase.Data switch
            {
                string => new TeTextFieldOption(),
                bool => new TeCheckBoxOption(),
                DateTime => new TeTextFieldOption<DateTime>() // TODO: TeDateTimeFied 를 만들어야 함
                {
                    Converter = new TeTextFieldConverter<DateTime>
                    {
                        FromString = s => DateTime.TryParse(s, out var value) ? value : default,
                        StringValue = value => value.ToString(),
                    },
                },
                TimeSpan => new TeTextFieldOption<TimeSpan>()
                {
                    Converter = new TeTextFieldConverter<TimeSpan>
                    {
                        FromString = s => TimeSpan.TryParse(s, out var value) ? value : default,
                        StringValue = value => value.ToString(),
                    },
                },

                // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/integral-numeric-types
                sbyte => new TeNumericFieldOption<sbyte>(),
                byte => new TeNumericFieldOption<byte>(),
                short => new TeNumericFieldOption<short>(),
                ushort => new TeNumericFieldOption<ushort>(),
                int => new TeNumericFieldOption<int>(),
                uint => new TeNumericFieldOption<uint>(),
                long => new TeNumericFieldOption<long>(),
                ulong => new TeNumericFieldOption<ulong>(),
                nint => new TeNumericFieldOption<nint>(),
                nuint => new TeNumericFieldOption<nuint>(),
                // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/floating-point-numeric-types
                float => new TeNumericFieldOption<float>(),
                double => new TeNumericFieldOption<double>(),
                decimal => new TeNumericFieldOption<decimal>(),

                IList<string> => TeListEditorOption<string>.Create(string.Empty),
                IList<bool> => TeListEditorOption<bool>.Create(default),
                // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/integral-numeric-types
                IList<sbyte> => TeListEditorOption<sbyte>.Create(default),
                IList<byte> => TeListEditorOption<byte>.Create(default),
                IList<short> => TeListEditorOption<short>.Create(default),
                IList<ushort> => TeListEditorOption<ushort>.Create(default),
                IList<int> => TeListEditorOption<int>.Create(default),
                IList<uint> => TeListEditorOption<uint>.Create(default),
                IList<long> => TeListEditorOption<long>.Create(default),
                IList<ulong> => TeListEditorOption<ulong>.Create(default),
                IList<nint> => TeListEditorOption<nint>.Create(default),
                IList<nuint> => TeListEditorOption<nuint>.Create(default),
                // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/floating-point-numeric-types
                IList<float> => TeListEditorOption<float>.Create(default),
                IList<double> => TeListEditorOption<double>.Create(default),
                IList<decimal> => TeListEditorOption<decimal>.Create(default),

                _ => default,
            };
            if (option != default)
            {
                fieldOption = option;
                return true;
            }
        }

        if (memberInfo != null)
        {
            var memberType = MemberType(memberInfo);
            ITeFieldOption? option =
                memberType == typeof(string) ? new TeTextFieldOption() :
                memberType == typeof(bool) ? new TeCheckBoxOption() :

                // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/integral-numeric-types
                memberType == typeof(sbyte) ? new TeNumericFieldOption<sbyte>() :
                memberType == typeof(byte) ? new TeNumericFieldOption<byte>() :
                memberType == typeof(short) ? new TeNumericFieldOption<short>() :
                memberType == typeof(ushort) ? new TeNumericFieldOption<ushort>() :
                memberType == typeof(int) ? new TeNumericFieldOption<int>() :
                memberType == typeof(uint) ? new TeNumericFieldOption<uint>() :
                memberType == typeof(long) ? new TeNumericFieldOption<long>() :
                memberType == typeof(ulong) ? new TeNumericFieldOption<ulong>() :
                memberType == typeof(nint) ? new TeNumericFieldOption<nint>() :
                memberType == typeof(nuint) ? new TeNumericFieldOption<nuint>() :
                // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/floating-point-numeric-types
                memberType == typeof(float) ? new TeNumericFieldOption<float>() :
                memberType == typeof(double) ? new TeNumericFieldOption<double>() :
                memberType == typeof(decimal) ? new TeNumericFieldOption<decimal>() :

                // IList로 하면 같다고 나오진 않는다.
                // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/integral-numeric-types
                memberType == typeof(List<sbyte>) ? TeListEditorOption<sbyte>.Create(default) :
                memberType == typeof(List<byte>) ? TeListEditorOption<byte>.Create(default) :
                memberType == typeof(List<short>) ? TeListEditorOption<short>.Create(default) :
                memberType == typeof(List<ushort>) ? TeListEditorOption<ushort>.Create(default) :
                memberType == typeof(List<int>) ? TeListEditorOption<int>.Create(default) :
                memberType == typeof(List<uint>) ? TeListEditorOption<uint>.Create(default) :
                memberType == typeof(List<long>) ? TeListEditorOption<long>.Create(default) :
                memberType == typeof(List<ulong>) ? TeListEditorOption<ulong>.Create(default) :
                memberType == typeof(List<nint>) ? TeListEditorOption<nint>.Create(default) :
                memberType == typeof(List<nuint>) ? TeListEditorOption<nuint>.Create(default) :
                // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/floating-point-numeric-types
                memberType == typeof(List<float>) ? TeListEditorOption<float>.Create(default) :
                memberType == typeof(List<double>) ? TeListEditorOption<double>.Create(default) :
                memberType == typeof(List<decimal>) ? TeListEditorOption<decimal>.Create(default) :

                default;

            if (option != default)
            {
                fieldOption = option;
                return true;
            }
        }

        // 3. Option의 generic type이 일치하는 경우 (primitive type이 아닌 경우)
        if (memberInfo != null)
        {
            var memberType = MemberType(memberInfo);
            fieldOption = fieldOptions
                .Where(option => option is ITeGenericTypeOption)
                .Select(option => option as ITeGenericTypeOption)
                .FirstOrDefault(option => option!.TypeName == memberType.FullName);
            if (fieldOption != default)
            {
                return true;
            }
        }

        fieldOption = default;
        return false;
    }

    private static Type MemberType(this MemberInfo memberInfo)
    {
        if (memberInfo is PropertyInfo property)
        {
            return property.PropertyType;
        }
        if (memberInfo is FieldInfo field)
        {
            return field.FieldType;
        }
        throw new Exception();
    }

}

