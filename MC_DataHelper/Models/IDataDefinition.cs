using Newtonsoft.Json;

namespace MC_DataHelper.Models;

public interface IDataDefinition
{
    [JsonProperty("name")] string DataName { get; set; }

    [JsonProperty("_jsonType")] public string JsonType { get; }
}