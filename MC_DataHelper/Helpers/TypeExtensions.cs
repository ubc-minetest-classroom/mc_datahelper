using System;

namespace MC_DataHelper.Helpers;

public static class TypeExtensions
{
    public static string[] GetPropertyNames(this Type type)
    {
        var properties = type.GetProperties();
        var propertyNames = new string[properties.Length];

        for (var index = 0; index < properties.Length; index++)
        {
            var property = properties[index];
            propertyNames[index] = property.Name;
        }

        return propertyNames;
    }
}