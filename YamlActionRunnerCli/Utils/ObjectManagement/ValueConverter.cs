using YamlActionRunnerCli.Exceptions.GeneralExceptions;

namespace YamlActionRunnerCli.Utils.ObjectManagement;

/// <summary>
/// Utility class that provides utilities for converting object values to specific target types, handling enums, nullables, and simple type changes.
/// </summary>
public static class ValueConverter
{
    private static readonly Dictionary<Func<Type, bool>, Func<object, Type, object>> _typeMatchToConverters = new()
    {
        { type => type.IsEnum, ConvertEnum }
    };

    /// <summary>
    /// Converts a value to a specified target type.
    /// </summary>
    /// <param name="value">The input value to be converted to another type.</param>
    /// <param name="targetType">The desired target type for the given 'value'.</param>
    /// <returns>The converted object</returns>
    public static object? ConvertValue(this object? value, Type targetType)
    {
        if (value is null)
            return null;

        var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;
        var converter = _typeMatchToConverters.FirstOrDefault(typeMatch => typeMatch.Key(underlyingType)).Value;

        return converter is not null ? converter(value, underlyingType) : ConvertSimple(value, underlyingType);
    }

    /// <summary>
    /// Converts a value (usually string) to an enum type, case-insensitively.
    /// </summary>
    private static object ConvertEnum(object value, Type enumType)
    {
        try
        {
            return Enum.Parse(enumType, value.ToString()!, ignoreCase: true);
        }
        catch
        {
            throw new InvalidConfigurationException(enumType, [$"Cannot convert '{value}' to enum {enumType.Name}"]);
        }
    }
    
    /// <summary>
    /// Performs a simple type conversion.
    /// </summary>
    private static object ConvertSimple(object value, Type targetType)
    {
        try
        {
            return Convert.ChangeType(value, targetType);
        }
        catch (Exception exception) when (exception is FormatException or InvalidCastException or OverflowException)
        {
            throw new InvalidConfigurationException(targetType,
                [$"Cannot convert '{value}' to type {targetType.Name}"]);
        }
    }
}