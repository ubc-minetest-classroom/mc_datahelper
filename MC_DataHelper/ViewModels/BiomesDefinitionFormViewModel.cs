using MC_DataHelper.Models.DataDefinitions;
using ReactiveUI;

namespace MC_DataHelper.ViewModels;

public class BiomesDefinitionFormViewModel : DataDefinitionFormViewModelBase<BiomeDataDefinition>
{
    public string? NodeDust
    {
        get => Data.NodeDust;
        set => Data.NodeDust = value;
    }

    public string? NodeTop
    {
        get => Data.NodeTop;
        set => Data.NodeTop = value;
    }

    public string? DepthTop
    {
        get => Data.DepthTop.ToString();
        set
        {
            if (int.TryParse(value, out var number)) Data.DepthTop = number;
        }
    }

    public string? NodeFiller
    {
        get => Data.NodeFiller;
        set => Data.NodeFiller = value;
    }

    public string? DepthFiller
    {
        get => Data.DepthFiller.ToString();
        set
        {
            if (int.TryParse(value, out var number)) Data.DepthFiller = number;
        }
    }

    public string? NodeStone
    {
        get => Data.NodeStone;
        set => Data.NodeStone = value;
    }

    public string? NodeWaterTop
    {
        get => Data.NodeWaterTop;
        set => Data.NodeWaterTop = value;
    }

    public string? DepthWaterTop
    {
        get => Data.DepthWaterTop.ToString();
        set
        {
            if (int.TryParse(value, out var number)) Data.DepthWaterTop = number;
        }
    }

    public string? NodeWater
    {
        get => Data.NodeWater;
        set => Data.NodeWater = value;
    }

    public string? NodeRiverbed
    {
        get => Data.NodeRiverbed;
        set => Data.NodeRiverbed = value;
    }

    public string? DepthRiverbed
    {
        get => Data.DepthRiverbed.ToString();
        set
        {
            if (int.TryParse(value, out var number)) Data.DepthRiverbed = number;
        }
    }

    public string? NodeCaveLiquid
    {
        get => Data.NodeCaveLiquid;
        set => Data.NodeCaveLiquid = value;
    }

    public string? YMin
    {
        get => Data.YMin.ToString();
        set
        {
            if (int.TryParse(value, out var number)) Data.YMin = number;
        }
    }

    public string? YMax
    {
        get => Data.YMax.ToString();
        set
        {
            if (int.TryParse(value, out var number)) Data.YMax = number;
        }
    }

    public string? VerticalBlend
    {
        get => Data.VerticalBlend.ToString();
        set
        {
            if (int.TryParse(value, out var number)) Data.VerticalBlend = number;
        }
    }

    public int HeatPoint
    {
        get => Data.HeatPoint ?? 0;
        set => Data.HeatPoint = value;
    }

    public int HumidityPoint
    {
        get => Data.HumidityPoint ?? 0;
        set => Data.HumidityPoint = value;
    }

    protected override void UpdateProperties()
    {
        base.UpdateProperties();
        this.RaisePropertyChanged(nameof(NodeDust));
        this.RaisePropertyChanged(nameof(NodeTop));
        this.RaisePropertyChanged(nameof(DepthTop));
        this.RaisePropertyChanged(nameof(NodeFiller));
        this.RaisePropertyChanged(nameof(DepthFiller));
        this.RaisePropertyChanged(nameof(NodeStone));
        this.RaisePropertyChanged(nameof(NodeWaterTop));
        this.RaisePropertyChanged(nameof(DepthWaterTop));
        this.RaisePropertyChanged(nameof(NodeWater));
        this.RaisePropertyChanged(nameof(NodeRiverbed));
        this.RaisePropertyChanged(nameof(DepthRiverbed));
        this.RaisePropertyChanged(nameof(NodeCaveLiquid));
        this.RaisePropertyChanged(nameof(YMin));
        this.RaisePropertyChanged(nameof(YMax));
        this.RaisePropertyChanged(nameof(VerticalBlend));
        this.RaisePropertyChanged(nameof(HeatPoint));
        this.RaisePropertyChanged(nameof(HumidityPoint));
    }

    public BiomesDefinitionFormViewModel() : base()
    {
    }

    public BiomesDefinitionFormViewModel(MainWindowViewModel mainWindowViewModel) : base(mainWindowViewModel)
    {
    }
}