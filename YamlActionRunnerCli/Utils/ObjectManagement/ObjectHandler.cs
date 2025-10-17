using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace YamlActionRunnerCli.Utils.ObjectManagement;

public static class ObjectHandler
{
    public static object ToObjectWithProperties(this Dictionary<string, object> properties, Type objectType)
    {
        var instance = Activator.CreateInstance(objectType)!;
        
        instance.PlaceProperties(properties);
        instance.ValidateMembers();
        
        return instance;
    }

    public static void PlaceProperties(this object instance, Dictionary<string, object> properties)
    {
        var normalized = properties.ToDictionary(kv => kv.Key.ToLower(), kv => kv.Value);
        
        foreach (var prop in instance!.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (normalized.TryGetValue(prop.Name.ToLower(), out var value))
            {
                prop.SetValue(instance, value.ConvertValue(prop.PropertyType));
            }
        }
    }
    public static void ValidateMembers<TObject>(this TObject objectToValidate) where TObject : new()
    {
        var results = new List<ValidationResult>();
        var context = new ValidationContext(objectToValidate!);

        if (!Validator.TryValidateObject(objectToValidate!, context, results, validateAllProperties: true))
        {
            throw new ValidationException(
                $"Invalid members for {typeof(TObject)}: {string.Join("\n ", results.Select(r => r.ErrorMessage))}");
        }
    }
}