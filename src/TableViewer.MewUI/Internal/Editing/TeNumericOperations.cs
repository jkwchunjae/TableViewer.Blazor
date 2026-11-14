using System.Numerics;
using TableViewer.MewUI.Options.Property;

namespace TableViewer.MewUI.Internal.Editing;

public static class TeNumericOperations
{
    public static object Parse(string text, ITeNumericFieldOption option)
    {
        var type = option.DefaultValue.GetType();
        var culture = option.Property.Culture ?? CultureInfo.InvariantCulture;
        object value = type == typeof(nint) ? nint.Parse(text, culture)
            : type == typeof(nuint) ? nuint.Parse(text, culture)
            : Convert.ChangeType(text, type, culture);
        var minimum = Bound(option.Property, "Min");
        var maximum = Bound(option.Property, "Max");
        if (minimum is IComparable min && min.CompareTo(value) > 0) throw new ArgumentOutOfRangeException(nameof(text), $"Minimum is {minimum}.");
        if (maximum is IComparable max && max.CompareTo(value) < 0) throw new ArgumentOutOfRangeException(nameof(text), $"Maximum is {maximum}.");
        return value;
    }

    public static object Step(object? value, ITeNumericFieldOption option, int direction) =>
        typeof(TeNumericOperations).GetMethod(nameof(StepTyped), BindingFlags.Static | BindingFlags.NonPublic)!
            .MakeGenericMethod(option.DefaultValue.GetType()).Invoke(null, [value ?? option.DefaultValue, option.Property, direction])!;

    private static T StepTyped<T>(object value, TeNumericFieldProperty property, int direction) where T : INumber<T>, IMinMaxValue<T>
    {
        var min = Bound(property, "Min") is T minimum ? minimum : T.MinValue;
        var max = Bound(property, "Max") is T maximum ? maximum : T.MaxValue;
        var step = Bound(property, "Step") is T increment ? increment : T.One;
        try
        {
            var result = direction > 0 ? checked((T)value + step) : checked((T)value - step);
            return T.Clamp(result, min, max);
        }
        catch (OverflowException) { return direction > 0 ? max : min; }
    }

    private static object? Bound(TeNumericFieldProperty property, string name) => property.GetType()
        .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
        .FirstOrDefault(info => info.Name == name)?.GetValue(property) ?? property.GetType().BaseType?.GetProperty(name)?.GetValue(property);
}
