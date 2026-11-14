using System.Text.RegularExpressions;

namespace TableViewer.MewUI.Options.Property;

public enum Margin { None, Dense, Normal }
public enum Origin { TopLeft, TopCenter, TopRight, CenterLeft, CenterCenter, CenterRight, BottomLeft, BottomCenter, BottomRight }
public enum Adornment { None, Start, End }
public enum InputMode { none, text, tel, url, email, numeric, @decimal, search }
public enum InputType { Text, Password, Email, Number, Telephone, Url, Search, Date, DateTimeLocal, Month, Time, Week, Color, Hidden }
public enum Typo { h1, h2, h3, h4, h5, h6, subtitle1, subtitle2, body1, body2, button, caption, overline, inherit }

/// <summary>Native text-mask contract. A mask must reject invalid text without changing the source value.</summary>
public interface ITeTextMask
{
    string Apply(string text);
}

public sealed class TeRegexMask(string pattern) : ITeTextMask
{
    public string Apply(string text) => Regex.IsMatch(text, pattern, RegexOptions.None, TimeSpan.FromSeconds(1)) ? text : throw new FormatException("Input does not match the mask.");
}

/// <summary>A mask that can also format an incomplete input while the user types.</summary>
public interface ITeInputMask : ITeTextMask
{
    string FormatInput(string text);
}

/// <summary>Formats fixed patterns: 0 accepts digits, a accepts letters, * accepts letters or digits; backslash escapes a literal.</summary>
public sealed class TePatternMask(string pattern) : ITeInputMask
{
    public string Pattern { get; } = pattern ?? throw new ArgumentNullException(nameof(pattern));
    public bool RequireComplete { get; set; } = true;
    public bool AllowPartial { get; set; }
    public string FormatInput(string text) => Format(text, false);
    public string Apply(string text) => Format(text, RequireComplete && !AllowPartial);

    private string Format(string text, bool requireComplete)
    {
        var result = new System.Text.StringBuilder();
        var position = 0;
        for (var index = 0; index < Pattern.Length; index++)
        {
            var token = Pattern[index];
            var literal = false;
            if (token == '\\' && index + 1 < Pattern.Length) { token = Pattern[++index]; literal = true; }
            var placeholder = !literal && token is '0' or 'a' or '*';
            if (!placeholder)
            {
                if (position < text.Length)
                {
                    result.Append(token);
                    if (text[position] == token) position++;
                }
                continue;
            }
            if (position >= text.Length)
            {
                if (requireComplete && text.Length > 0) throw new FormatException("The masked input is incomplete.");
                break;
            }
            var character = text[position++];
            var valid = token switch { '0' => char.IsDigit(character), 'a' => char.IsLetter(character), _ => char.IsLetterOrDigit(character) };
            if (!valid) throw new FormatException($"'{character}' is not valid at position {result.Length + 1}.");
            result.Append(character);
        }
        if (position < text.Length) throw new FormatException("The masked input is too long.");
        return result.ToString();
    }
}

/// <summary>Selects a mask from typed predicates, such as local and international phone formats.</summary>
public sealed class TeMultiMask : ITeInputMask
{
    public required IReadOnlyList<TeMaskChoice> Choices { get; init; }
    public ITeTextMask? Fallback { get; init; }
    private ITeTextMask Select(string text) => Choices.FirstOrDefault(choice => choice.Condition(text))?.Mask
        ?? Fallback ?? throw new FormatException("No mask accepts this input.");
    public string Apply(string text) => Select(text).Apply(text);
    public string FormatInput(string text) => Select(text) is ITeInputMask mask ? mask.FormatInput(text) : text;
}

public sealed record TeMaskChoice(Func<string, bool> Condition, ITeTextMask Mask);

/// <summary>Combines independently masked fixed-width blocks separated by a literal separator.</summary>
public sealed class TeBlockMask : ITeInputMask
{
    public required IReadOnlyList<TeMaskBlock> Blocks { get; init; }
    public string Separator { get; init; } = "-";
    public string Apply(string text) => Format(text, true);
    public string FormatInput(string text) => Format(text, false);
    private string Format(string text, bool commit)
    {
        var remainder = text;
        var formatted = new List<string>();
        foreach (var block in Blocks)
        {
            if (block.Length < 1) throw new InvalidOperationException("Mask-block lengths must be positive.");
            if (remainder.Length == 0) break;
            var length = Math.Min(block.Length, remainder.Length);
            var part = remainder[..length];
            formatted.Add(commit ? block.Mask.Apply(part) : block.Mask is ITeInputMask mask ? mask.FormatInput(part) : part);
            remainder = remainder[length..];
            if (Separator.Length > 0 && remainder.StartsWith(Separator, StringComparison.Ordinal)) remainder = remainder[Separator.Length..];
        }
        if (remainder.Length > 0) throw new FormatException("The masked input has too many blocks.");
        if (commit && text.Length > 0 && formatted.Count < Blocks.Count) throw new FormatException("The masked input is incomplete.");
        return string.Join(Separator, formatted);
    }
}

public sealed record TeMaskBlock(int Length, ITeTextMask Mask);
