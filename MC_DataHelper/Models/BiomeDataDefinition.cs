using System.Collections.Generic;
using Newtonsoft.Json;

namespace MC_DataHelper.Models;

public class BiomeDataDefinition : IDataDefinition
{
    public BiomeDataDefinition()
    {
    }

    [JsonProperty("name")] public string Name { get; set; } = "New Biome";
    [JsonProperty("_jsonType")] public string JsonType => "biome";
    [JsonProperty("depth_filler")] public int DepthFiller { get; set; } = 3;
    [JsonProperty("depth_riverbed")] public int DepthRiverbed { get; set; } = 2;
    [JsonProperty("depth_top")] public int DepthTop { get; set; } = 1;
    [JsonProperty("depth_water_top")] public int DepthWaterTop { get; set; } = 10;
    [JsonProperty("heat_point")] public int HeatPoint { get; set; } = 50;
    [JsonProperty("humidity_point")] public int HumidityPoint { get; set; } = 50;
    [JsonProperty("y_min")] public int YMin { get; set; } = -31000;
    [JsonProperty("y_max")] public int YMax { get; set; } = 31000;
    [JsonProperty("vertical_blend")] public int VerticalBlend { get; set; } = 8;
    [JsonProperty("node_cave_liquid")] public string NodeCaveLiquid { get; set; } = "default:water_source";
    [JsonProperty("node_filler")] public string NodeFiller { get; set; } = "default:dirt";
    [JsonProperty("node_riverbed")] public string NodeRiverbed { get; set; } = "default:water_source";
    [JsonProperty("node_stone")] public string NodeStone { get; set; } = "default:stone";
    [JsonProperty("node_top")] public string NodeTop { get; set; } = "default:dirt_with_grass";
    [JsonProperty("node_water")] public string NodeWater { get; set; } = "default:water_source";
    [JsonProperty("node_water_top")] public string NodeWaterTop { get; set; } = "default:water_source";
    [JsonProperty("node_dust")] public string? NodeDust { get; set; } = null;
}