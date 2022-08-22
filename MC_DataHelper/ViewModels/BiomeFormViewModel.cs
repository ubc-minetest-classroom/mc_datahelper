using System.Reactive;
using MC_DataHelper.Models;
using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Extensions;

namespace MC_DataHelper.ViewModels;

public class BiomeFormViewModel : ViewModelBase, IValidatableViewModel
{
    private BiomeDataDefinition _data = new();
    private ModPackage _parentPackage;

    MainWindowViewModel _mainWindowViewModel;

    public string BiomeName => _data.DataName;

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
        set
        {
            if (int.TryParse(value, out var number))
            {
                _data.DepthFiller = number;
            }
        }
    }

    public string NodeStone => _data.NodeStone;

    public string NodeWaterTop => _data.NodeWaterTop;

    public string DepthWaterTop
    {
        get => _data.DepthWaterTop.ToString();
        set
        {
            if (int.TryParse(value, out var number))
            {
                _data.DepthWaterTop = number;
            }
        }
    }

    public string NodeWater => _data.NodeWater;

    public string NodeRiverbed => _data.NodeRiverbed;

    public string DepthRiverbed
    {
        get => _data.DepthRiverbed.ToString();
        set
        {
            if (int.TryParse(value, out var number))
            {
                _data.DepthRiverbed = number;
            }
        }
    }

    public string NodeCaveLiquid => _data.NodeCaveLiquid;

    public string YMin
    {
        get => _data.YMin.ToString();
        set
        {
            if (int.TryParse(value, out var number))
            {
                _data.YMin = number;
            }
        }
    }

    public string YMax
    {
        get => _data.YMax.ToString();
        set
        {
            if (int.TryParse(value, out var number))
            {
                _data.YMax = number;
            }
        }
    }

    public string VerticalBlend
    {
        get => _data.VerticalBlend.ToString();
        set
        {
            if (int.TryParse(value, out var number))
            {
                _data.VerticalBlend = number;
            }
        }
    }

    public string HeatPoint
    {
        get => _data.HeatPoint.ToString();
        set
        {
            if (int.TryParse(value, out var number))
            {
                _data.HeatPoint = number;
            }
        }
    }

    public string HumidityPoint
    {
        get => _data.HumidityPoint.ToString();
        set
        {
            if (int.TryParse(value, out var number))
            {
                _data.HumidityPoint = number;
            }
        }
    }

    public ReactiveCommand<Unit, Unit> SubmitFormCommand { get; }

    public ReactiveCommand<Unit, Unit> ClearFormCommand { get; }

    public BiomeFormViewModel(MainWindowViewModel mainWindowViewModel, ModPackage parentPackage)
    {
        _mainWindowViewModel = mainWindowViewModel;
        
        _parentPackage = parentPackage;
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
        _parentPackage.DataDefinitions.Add(_data);
        ClearForm();
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

    public void UpdatePackage(ModPackage selectedPackage)
    {
        _parentPackage = selectedPackage;
        UpdateProperties();
    }
}