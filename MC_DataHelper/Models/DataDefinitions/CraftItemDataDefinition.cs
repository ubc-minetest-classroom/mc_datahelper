using System.Collections.Generic;
using Newtonsoft.Json;

namespace MC_DataHelper.Models.DataDefinitions;

//TODO: Add Sound table
//TODO: Add Tool_Capabilities table
public record CraftItemDataDefinition : IDataDefinition
{
    [JsonProperty("description")] public string? Description { get; set; }

    [JsonProperty("short_description")] public string? ShortDescription { get; set; }

    [JsonProperty("groups")] public List<string> Groups { get; set; } = new List<string>();

    [JsonProperty("inventory_image")] public string? InventoryImage { get; set; }

    [JsonProperty("wield_image")] public string? WieldImage { get; set; }

    [JsonProperty("wield_overlay")] public string? WieldOverlay { get; set; }

    [JsonProperty("palette")] public string? Palette { get; set; }

    [JsonProperty("color")] public string? Color { get; set; }

    [JsonProperty("wield_scale")] public string? WieldScale { get; set; }

    [JsonProperty("stack_max")] public int? StackMax { get; set; }

    [JsonProperty("range")] public int? Range { get; set; }

    [JsonProperty("liquids_pointable")] public bool? LiquidsPointable { get; set; }

    [JsonProperty("light_source")] public int? LightSource { get; set; }

    [JsonProperty("node_placement_prediction")]
    public string? NodePlacementPrediction { get; set; }

    [JsonProperty("node_dig_prediction")] public string? NodeDigPrediction { get; set; } = "air";


    [JsonProperty("name", NullValueHandling = NullValueHandling.Include)]
    public string Name { get; set; } = "New Item";

    [JsonProperty("_jsonType", NullValueHandling = NullValueHandling.Include)]
    public string JsonType => "craftitem";
}