using System.ComponentModel.DataAnnotations;
using System.Reflection;
using YamlActionRunnerCli.Exceptions.GeneralExceptions;

namespace YamlActionRunnerCli.Utils.ObjectManagement;

public static class ObjectHandler
{
    public static object ToObjectWithProperties(this IDictionary<string, object> properties, Type objectType)
    {
        var instance = Activator.CreateInstance(objectType)!;
        
        instance.PlaceProperties(properties);
        instance.ValidateMembers();
        
        return instance;
    }

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