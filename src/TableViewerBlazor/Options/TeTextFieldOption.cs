﻿namespace TableViewerBlazor.Options;

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

public interface ITeTextFieldOption : ITeFieldOption
{
    IEnumerable<ITeValidation>? Validations { get; }
    ITeTextFieldProperty? Property { get; }
    ITeTextFieldEvent? Event { get; }
}

public class TeTextFieldOption<T> : ITeFieldOption<T>, ITeTextFieldOption
{
    public string? Id { get; set; }
    public Func<T?, int, string, bool>? Condition { get; set; }
    public IEnumerable<ITeValidation>? Validations { get; set; }
    public TeTextFieldProperty? Property { get; set; }
    public TeTextFieldEvent<T>? Event { get; set; }

    ITeTextFieldProperty? ITeTextFieldOption.Property => Property;
    ITeTextFieldEvent? ITeTextFieldOption.Event => Event;
}
