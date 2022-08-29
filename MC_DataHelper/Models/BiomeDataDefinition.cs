﻿using System;
using Newtonsoft.Json;

namespace MC_DataHelper.Models;

public class BiomeDataDefinition : IDataDefinition
{
    [JsonProperty("depth_filler", NullValueHandling = NullValueHandling.Ignore)]
    public int DepthFiller { get; set; } = 3;

    [JsonProperty("depth_riverbed", NullValueHandling = NullValueHandling.Ignore)]
    public int DepthRiverbed { get; set; } = 2;

    [JsonProperty("depth_top", NullValueHandling = NullValueHandling.Ignore)]
    public int DepthTop { get; set; } = 1;

    [JsonProperty("depth_water_top", NullValueHandling = NullValueHandling.Ignore)]
    public int DepthWaterTop { get; set; } = 10;

    [JsonProperty("heat_point", NullValueHandling = NullValueHandling.Ignore)]
    public int HeatPoint { get; set; } = 50;

    [JsonProperty("humidity_point", NullValueHandling = NullValueHandling.Ignore)]
    public int HumidityPoint { get; set; } = 50;

    [JsonProperty("y_min", NullValueHandling = NullValueHandling.Ignore)]
    public int YMin { get; set; } = -31000;

    [JsonProperty("y_max", NullValueHandling = NullValueHandling.Ignore)]
    public int YMax { get; set; } = 31000;

    [JsonProperty("vertical_blend", NullValueHandling = NullValueHandling.Ignore)]
    public int VerticalBlend { get; set; } = 8;

    [JsonProperty("node_cave_liquid", NullValueHandling = NullValueHandling.Ignore)]
    public string NodeCaveLiquid { get; set; } = "default:water_source";

    [JsonProperty("node_filler", NullValueHandling = NullValueHandling.Ignore)]
    public string NodeFiller { get; set; } = "default:dirt";

    [JsonProperty("node_riverbed", NullValueHandling = NullValueHandling.Ignore)]
    public string NodeRiverbed { get; set; } = "default:water_source";

    [JsonProperty("node_stone", NullValueHandling = NullValueHandling.Ignore)]
    public string NodeStone { get; set; } = "default:stone";

    [JsonProperty("node_top", NullValueHandling = NullValueHandling.Ignore)]
    public string NodeTop { get; set; } = "default:dirt_with_grass";

    [JsonProperty("node_water", NullValueHandling = NullValueHandling.Ignore)]
    public string NodeWater { get; set; } = "default:water_source";

    [JsonProperty("node_water_top", NullValueHandling = NullValueHandling.Ignore)]
    public string NodeWaterTop { get; set; } = "default:water_source";

    [JsonProperty("node_dust", NullValueHandling = NullValueHandling.Ignore)]
    public string? NodeDust { get; set; }

    [JsonProperty("name", NullValueHandling = NullValueHandling.Include)]
    public string DataName { get; set; } = "New Biome";

    [JsonProperty("_jsonType", NullValueHandling = NullValueHandling.Include)]
    public string JsonType => "biome";

    protected bool Equals(BiomeDataDefinition other)
    {
        return DataName == other.DataName && DepthFiller == other.DepthFiller && DepthRiverbed == other.DepthRiverbed &&
               DepthTop == other.DepthTop && DepthWaterTop == other.DepthWaterTop && HeatPoint == other.HeatPoint &&
               HumidityPoint == other.HumidityPoint && YMin == other.YMin && YMax == other.YMax &&
               VerticalBlend == other.VerticalBlend && NodeCaveLiquid == other.NodeCaveLiquid &&
               NodeFiller == other.NodeFiller && NodeRiverbed == other.NodeRiverbed && NodeStone == other.NodeStone &&
               NodeTop == other.NodeTop && NodeWater == other.NodeWater && NodeWaterTop == other.NodeWaterTop &&
               NodeDust == other.NodeDust;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((BiomeDataDefinition)obj);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(DataName);
        hashCode.Add(DepthFiller);
        hashCode.Add(DepthRiverbed);
        hashCode.Add(DepthTop);
        hashCode.Add(DepthWaterTop);
        hashCode.Add(HeatPoint);
        hashCode.Add(HumidityPoint);
        hashCode.Add(YMin);
        hashCode.Add(YMax);
        hashCode.Add(VerticalBlend);
        hashCode.Add(NodeCaveLiquid);
        hashCode.Add(NodeFiller);
        hashCode.Add(NodeRiverbed);
        hashCode.Add(NodeStone);
        hashCode.Add(NodeTop);
        hashCode.Add(NodeWater);
        hashCode.Add(NodeWaterTop);
        hashCode.Add(NodeDust);
        return hashCode.ToHashCode();
    }
}