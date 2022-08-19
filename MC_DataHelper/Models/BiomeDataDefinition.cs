using System.Collections.Generic;

namespace MC_DataHelper.Models;

public abstract class BiomeDataDefinition : IDataDefinition
{
    protected BiomeDataDefinition(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
    public string JsonType => "biome";

    public int DepthFiller { get; set; } = 3;
    public int DepthRiverbed { get; set; } = 2;
    public int DepthTop { get; set; } = 1;
    public int DepthWaterTop { get; set; } = 10;
    public int HeatPoint { get; set; } = 50;
    public int HumidityPoint { get; set; } = 50;
    public int YMin { get; set; } = -31000;
    public int YMax { get; set; } = 31000;
    public int VerticalBlend { get; set; } = 8;
    public string NodeCaveLiquid { get; set; } = "default:water_source";
    public string NodeFiller { get; set; } = "default:dirt";
    public string NodeRiverbed { get; set; } = "default:water_source";
    public string NodeStone { get; set; } = "default:stone";
    public string NodeTop { get; set; } = "default:dirt_with_grass";
    public string NodeWater { get; set; } = "default:water_source";
    public string NodeWaterTop { get; set; } = "default:water_source";
}