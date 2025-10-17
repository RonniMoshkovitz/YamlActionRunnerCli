namespace YamlActionRunnerCli.Utils.ObjectManagement;

public static class ValueConverter
{
    private static readonly Dictionary<Func<Type, bool>, Func<object, Type, object>> _typeMatchToConverters = new()
    {
        { t => t.IsEnum, ConvertEnum }
    };

    public static object? ConvertValue(this object? value, Type targetType)
    {
        if (value is null)
            return null;

        var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;
        var converter = _typeMatchToConverters.FirstOrDefault(typeMatch => typeMatch.Key(underlyingType)).Value;

        return converter is not null ? converter(value, underlyingType) : ConvertSimple(value, underlyingType);
    }

    private static object ConvertEnum(object value, Type enumType)
    {
        try
        {
            return Enum.Parse(enumType, value.ToString()!, ignoreCase: true);
        }
        catch
        {
            throw new FormatException($"Cannot convert '{value}' to enum {enumType.Name}");
        }
    }

    private static object ConvertSimple(object value, Type targetType)
    {
        try
        {
            return Convert.ChangeType(value, targetType);
        }
        catch (Exception ex) when (ex is FormatException or InvalidCastException or OverflowException)
        {
            throw new FormatException($"Cannot convert '{value}' to type {targetType.Name}", ex);
        }
    }
}