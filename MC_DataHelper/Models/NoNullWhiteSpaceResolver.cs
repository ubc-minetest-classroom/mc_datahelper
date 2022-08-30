//COPYRIGHT NOTICE:
//CODE ADOPTED FROM:
//https://stackoverflow.com/questions/50840347/prevent-serialization-if-value-is-null-or-whitespace-in-json-net

using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace MC_DataHelper.Models;

public class NoNullWhiteSpaceResolver : DefaultContractResolver
{
    public static readonly NoNullWhiteSpaceResolver Instance = new NoNullWhiteSpaceResolver();

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);

        if (property.PropertyType == typeof(string))
        {
            property.ShouldSerialize =
                instance =>
                {
                    try
                    {
                        var rawValue = property.ValueProvider?.GetValue(instance);
                        if (rawValue == null)
                        {
                            return false;
                        }

                        var stringValue = property.ValueProvider?.GetValue(instance)?.ToString();
                        return !string.IsNullOrWhiteSpace(stringValue);
                    }
                    catch
                    {
                        return true;
                    }
                };
        }

        return property;
    }
}