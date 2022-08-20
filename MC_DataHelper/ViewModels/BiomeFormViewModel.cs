using System.Reactive;
using Avalonia.Data;
using MC_DataHelper.Models;
using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Extensions;

namespace MC_DataHelper.ViewModels;

public class BiomeFormViewModel : ViewModelBase, IValidatableViewModel
{
    private BiomeDataDefinition _data = new();


    public string BiomeName => _data.Name;

    public string? NodeDust => _data.NodeDust;

    public string NodeTop => _data.NodeTop;

    public string DepthTop
    {
        get => _data.DepthTop.ToString();
        set
        {
            if (int.TryParse(value, out var number))
            {
                _data.DepthTop = number;
            }
        }
    }

    public string NodeFiller => _data.NodeFiller;

    public string DepthFiller
    {
        get => _data.DepthFiller.ToString();
        set => _data.DepthFiller = int.Parse(value);
    }

    public string NodeStone => _data.NodeStone;

    public string NodeWaterTop => _data.NodeWaterTop;

    public string DepthWaterTop
    {
        get => _data.DepthWaterTop.ToString();
        set => _data.DepthWaterTop = int.Parse(value);
    }

    public string NodeWater => _data.NodeWater;

    public string NodeRiverbed => _data.NodeRiverbed;

    public string DepthRiverbed
    {
        get => _data.DepthRiverbed.ToString();
        set => _data.DepthRiverbed = int.Parse(value);
    }

    public string NodeCaveLiquid => _data.NodeCaveLiquid;

    public string YMin
    {
        get => _data.YMin.ToString();
        set => _data.YMin = int.Parse(value);
    }

    public string YMax
    {
        get => _data.YMax.ToString();
        set => _data.YMax = int.Parse(value);
    }

    public string VerticalBlend
    {
        get => _data.VerticalBlend.ToString();
        set => _data.VerticalBlend = int.Parse(value);
    }

    public string HeatPoint
    {
        get => _data.HeatPoint.ToString();
        set => _data.HeatPoint = int.Parse(value);
    }

    public string HumidityPoint
    {
        get => _data.HumidityPoint.ToString();
        set => _data.HumidityPoint = int.Parse(value);
    }

    public ReactiveCommand<Unit, Unit> SubmitFormCommand { get; }

    public ReactiveCommand<Unit, Unit> ClearFormCommand { get; }

    public BiomeFormViewModel()
    {
        SubmitFormCommand = ReactiveCommand.Create(SubmitForm);
        ClearFormCommand = ReactiveCommand.Create(ClearForm);
        
        this.ValidationRule(
            viewModel => viewModel.DepthTop,
            name => !int.TryParse(name, out var value),
            "You must specify a valid integer");
    }
    
    public ValidationContext ValidationContext { get; } = new ValidationContext();


    private void SubmitForm()
    {
    }

    private void ClearForm()
    {
        _data = new();
        UpdateProperties();
    }

    private void UpdateProperties()
    {
        this.RaisePropertyChanged(nameof(BiomeName));
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

}