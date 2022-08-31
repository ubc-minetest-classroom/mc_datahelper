using Newtonsoft.Json;

namespace MC_DataHelper.Models.DataDefinitions;

public interface IDataDefinition
{
    [JsonProperty("name", NullValueHandling = NullValueHandling.Include)]
    string Name { get; set; }

    [JsonProperty("_jsonType", NullValueHandling = NullValueHandling.Include)]
    public string JsonType { get; }
}