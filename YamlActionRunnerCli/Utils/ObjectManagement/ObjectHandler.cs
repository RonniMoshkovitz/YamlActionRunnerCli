using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace YamlActionRunnerCli.Utils.ObjectManagement;

public static class ObjectHandler
{
    public static object ToObjectWithProperties(this Dictionary<string, object> properties, Type objectType)
    {
        var instance = Activator.CreateInstance(objectType)!;
        instance.PlaceProperties(properties);

        ValidateRequiredProperties(instance);
        return instance;
    }

    public static void PlaceProperties(this object instance, Dictionary<string, object> properties)
    {
        var normalized = properties.ToDictionary(kv => kv.Key.ToLower(), kv => kv.Value);
        
        foreach (var prop in instance!.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (normalized.TryGetValue(prop.Name.ToLower(), out var value))
            {
                prop.SetValue(instance, ConvertValue(value, prop.PropertyType));
            }
        }
    }
    private static void ValidateRequiredProperties<TObject>(TObject objectToValidate) where TObject : new()
    {
        var results = new List<ValidationResult>();
        var context = new ValidationContext(objectToValidate!);

        if (!Validator.TryValidateObject(objectToValidate!, context, results, validateAllProperties: true))
        {
            throw new ValidationException(
                $"Invalid members: {string.Join("\n ", results.Select(r => r.ErrorMessage))}");
        }
    }

    private static object? ConvertValue(object? value, Type targetType)
    {
        if (value is null)
            return null;

        var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

        if (underlyingType.IsEnum)
            return Enum.Parse(underlyingType, value.ToString()!, ignoreCase: true);

        if (underlyingType == typeof(Guid))
            return Guid.Parse(value.ToString()!);

        return Convert.ChangeType(value, underlyingType);
    }
}