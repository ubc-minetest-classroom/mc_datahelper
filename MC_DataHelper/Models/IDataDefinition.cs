using Newtonsoft.Json;

namespace MC_DataHelper.Models;

public interface IDataDefinition
{
    [JsonProperty(propertyName: "name")]
    string Name { get; }
    [JsonProperty(propertyName: "_jsonType")]
    public string JsonType { get; }
}