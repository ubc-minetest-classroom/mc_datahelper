using System.Reactive;
using MC_DataHelper.Models.DataDefinitions;
using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;

namespace MC_DataHelper.ViewModels;

public class BiomesFormViewModel : ViewModelBase, IValidatableViewModel
{
    private readonly MainWindowViewModel _mainWindowViewModel;
    private BiomeDataDefinition _data = new();

    private TreeViewDataNode? _selectedNode;

    // Constructor for testing
    public BiomesFormViewModel()
    {
        _mainWindowViewModel = new MainWindowViewModel();
        SubmitFormCommand = ReactiveCommand.Create(SubmitForm);
        ClearFormCommand = ReactiveCommand.Create(ClearForm);
    }

    public BiomesFormViewModel(MainWindowViewModel mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;

        SubmitFormCommand = ReactiveCommand.Create(SubmitForm);
        ClearFormCommand = ReactiveCommand.Create(ClearForm);
    }

    private TreeViewDataNode? SelectedNode
    {
        get => _selectedNode;
        set
        {
            _selectedNode = value;
            _mainWindowViewModel.SelectedTreeViewItem = value;
            _mainWindowViewModel.EditingExistingBiome = value != null;
        }
    }

    public string? BiomeName
    {
        get => _data.Name;
        set
        {
            _data.Name = value ?? "";

            SelectedNode?.refreshLabel();
        }
    }

    public string? NodeDust
    {
        get => _data.NodeDust;
        set => _data.NodeDust = value;
    }

    public string? NodeTop
    {
        get => _data.NodeTop;
        set => _data.NodeTop = value;
    }

    public string? DepthTop
    {
        get => _data.DepthTop.ToString();
        set
        {
            if (int.TryParse(value, out var number)) _data.DepthTop = number;
        }
    }

    public string? NodeFiller
    {
        get => _data.NodeFiller;
        set => _data.NodeFiller = value;
    }

    public string? DepthFiller
    {
        get => _data.DepthFiller.ToString();
        set
        {
            if (int.TryParse(value, out var number)) _data.DepthFiller = number;
        }
    }

    public string? NodeStone
    {
        get => _data.NodeStone;
        set => _data.NodeStone = value;
    }

    public string? NodeWaterTop
    {
        get => _data.NodeWaterTop;
        set => _data.NodeWaterTop = value;
    }

    public string? DepthWaterTop
    {
        get => _data.DepthWaterTop.ToString();
        set
        {
            if (int.TryParse(value, out var number)) _data.DepthWaterTop = number;
        }
    }

    public string? NodeWater
    {
        get => _data.NodeWater;
        set => _data.NodeWater = value;
    }

    public string? NodeRiverbed
    {
        get => _data.NodeRiverbed;
        set => _data.NodeRiverbed = value;
    }

    public string? DepthRiverbed
    {
        get => _data.DepthRiverbed.ToString();
        set
        {
            if (int.TryParse(value, out var number)) _data.DepthRiverbed = number;
        }
    }

    public string? NodeCaveLiquid
    {
        get => _data.NodeCaveLiquid;
        set => _data.NodeCaveLiquid = value;
    }

    public string? YMin
    {
        get => _data.YMin.ToString();
        set
        {
            if (int.TryParse(value, out var number)) _data.YMin = number;
        }
    }

    public string? YMax
    {
        get => _data.YMax.ToString();
        set
        {
            if (int.TryParse(value, out var number)) _data.YMax = number;
        }
    }

    public string? VerticalBlend
    {
        get => _data.VerticalBlend.ToString();
        set
        {
            if (int.TryParse(value, out var number)) _data.VerticalBlend = number;
        }
    }

    public int HeatPoint
    {
        get => _data.HeatPoint ?? 0;
        set => _data.HeatPoint = value;
    }

    public int HumidityPoint
    {
        get => _data.HumidityPoint ?? 0;
        set => _data.HumidityPoint = value;
    }

    public ReactiveCommand<Unit, Unit> SubmitFormCommand { get; }

    public ReactiveCommand<Unit, Unit> ClearFormCommand { get; }

    public ValidationContext ValidationContext { get; } = new();

    private void SubmitForm()
    {
        _mainWindowViewModel.AddDataDefinition(_data);
        ClearForm();
    }

    private void ClearForm()
    {
        SelectedNode = null;
        _mainWindowViewModel.SelectedTreeViewItem = null;
        _data = new BiomeDataDefinition();
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

    public void UpdateDataSource(BiomeDataDefinition nodeDataDefinition, TreeViewDataNode treeViewDataNode)
    {
        _data = nodeDataDefinition;
        UpdateProperties();
    }
}