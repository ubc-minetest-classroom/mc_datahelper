using Newtonsoft.Json;

namespace MC_DataHelper.Models;

public interface IDataDefinition
{
    [JsonProperty(propertyName: "name")]
    string DataName { get; set; }

    [JsonProperty(propertyName: "_jsonType")]
    public string JsonType { get; }
}