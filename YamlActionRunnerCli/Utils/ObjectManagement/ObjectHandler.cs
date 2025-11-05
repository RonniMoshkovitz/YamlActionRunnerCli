using System.ComponentModel.DataAnnotations;
using System.Reflection;
using YamlActionRunnerCli.Exceptions.GeneralExceptions;

namespace YamlActionRunnerCli.Utils.ObjectManagement;

/// <summary>
/// Utility class that provides extension methods for dynamic object manipulation, validation, and property mapping.
/// </summary>
public static class ObjectHandler
{
    /// <summary>
    /// Creates an instance of a type and populates its properties based on a given dictionary.
    /// </summary>
    /// <param name="properties">The dictionary of properties mapped (property name to value).</param>
    /// <param name="objectType">The type of object to create.</param>
    /// <returns>A new object instance with populated and validated properties.</returns>
    public static object ToObjectWithProperties(this IDictionary<string, object> properties, Type objectType)
    {
        var instance = Activator.CreateInstance(objectType)!;
        
        instance.PlaceProperties(properties);
        instance.ValidateMembers();
        
        return instance;
    }

    /// <summary>
    /// Maps properties from a dictionary to an object instance, matching case-insensitively.
    /// </summary>
    /// <param name="instance">The object instance to populate.</param>
    /// <param name="properties">The dictionary of properties mapped (property name to value).</param>
    private static void PlaceProperties(this object instance, IDictionary<string, object> properties)
    {
        var normalized = properties.ToDictionary(kv => kv.Key.ToLower(), kv => kv.Value);
        
        foreach (var prop in instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (normalized.TryGetValue(prop.Name.ToLower(), out var value))
            {
                prop.SetValue(instance, value.ConvertValue(prop.PropertyType));
            }
        }
    }
    
    /// <summary>
    /// Validates an object's properties using DataAnnotations.
    /// </summary>
    /// <param name="objectToValidate">The object to validate.</param>
    /// <exception cref="InvalidConfigurationException">Thrown if validation fails.</exception>
    private static void ValidateMembers<TObject>(this TObject objectToValidate) where TObject : new()
    {
        List<ValidationResult> results = [];
        var context = new ValidationContext(objectToValidate!);

        if (!Validator.TryValidateObject(objectToValidate!, context, results, validateAllProperties: true))
        {
            throw new InvalidConfigurationException(objectToValidate!.GetType(), results.Select(result => result.ErrorMessage));
        }
    }
}