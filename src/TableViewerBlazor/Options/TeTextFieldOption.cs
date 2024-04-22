namespace TableViewerBlazor.Options;

public static class TeTextFieldOptionExtensions
{
    public static bool TryGetTextFieldOption(this TeOptions options,
        MemberInfo? memberInfo, object? data,
        out ITeTextFieldOption? textFieldOption)
    {
        var textFieldAttribute = memberInfo?.GetCustomAttribute<TeTextFieldAttribute>();
        if (textFieldAttribute != null)
        {
            textFieldOption = options.TextFieldOptions?
                .FirstOrDefault(o => o.Id == textFieldAttribute.Id) ?? default;
            if (textFieldOption != null)
            {
                return true;
            }
        }

        textFieldOption = options.TextFieldOptions?
            .Where(option => string.IsNullOrEmpty(option.Id))
            .FirstOrDefault(o => o.Condition?.Invoke(data, 0, "path") ?? true) ?? default;
        if (textFieldOption != null)
        {
            return true;
        }

        textFieldOption = data switch
        {
            string => new TeTextFieldOption<string>(),
            _ => null,
        };
        return textFieldOption != null;
    }
}

public class TeTextFieldAttribute : Attribute
{
    public string Id { get; init; }
    public TeTextFieldAttribute(string id)
    {
        Id = id;
    }
}

public interface ITeTextFieldOption
{
    string? Id { get; }
    IEnumerable<ITeTextFieldValidation>? Validations { get; }
    public Func<object?, int, string, bool>? Condition { get; }
}

public class TeTextFieldOption<T> : ITeTextFieldOption
{
    public string? Id { get; set; }
    public IEnumerable<TeTextFieldValidation<T>>? Validations { get; set; }
    public Func<T?, int, string, bool>? Condition { get; set; }

    IEnumerable<ITeTextFieldValidation>? ITeTextFieldOption.Validations
        => Validations;

    Func<object?, int, string, bool>? ITeTextFieldOption.Condition =>
        (value, index, columnName) =>
        {
            if (value is T tValue)
            {
                return Condition?.Invoke(tValue, index, columnName) ?? true;
            }
            else
            {
                return false;
            }
        };

}

public interface ITeTextFieldValidation
{
    Func<object, bool> Func { get; }
    string Message { get; }
}

public class TeTextFieldValidation<T> : ITeTextFieldValidation
{
    public required Func<T, bool> Func { get; set; }
    public required string Message { get; set; }
    Func<object, bool> ITeTextFieldValidation.Func =>
        value =>
        {
            if (value is T tValue)
            {
                return Func?.Invoke(tValue) ?? false;
            }
            else
            {
                return false;
            }
        };
}