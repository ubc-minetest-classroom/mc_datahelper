using MC_DataHelper.Models.DataDefinitions;
using ReactiveUI;

namespace MC_DataHelper.ViewModels;

public class ItemDefinitionFormViewModel : DataDefinitionFormViewModelBase<CraftItemDataDefinition>
{
    public string? Description => Data.Description;

    public string? ShortDescription => Data.ShortDescription;

    public string? InventoryImage => Data.InventoryImage;
    public string? InventoryOverlay => Data.InventoryOverlay;

    public string? WieldImage => Data.WieldImage;
    public string? WieldOverlay => Data.WieldOverlay;
    public string? Palette => Data.Palette;
    public string? Color => Data.Color;
    public string? WieldScale => Data.WieldScale;

    public string? StackMax
    {
        get => Data.StackMax.ToString();
        set
        {
            if (int.TryParse(value, out var number)) Data.StackMax = number;
        }
    }

    public string? Range
    {
        get => Data.Range.ToString();
        set
        {
            if (int.TryParse(value, out var number)) Data.Range = number;
        }
    }

    public bool LiquidsPointable
    {
        get => Data.LiquidsPointable.HasValue && Data.LiquidsPointable.Value;
        set => Data.LiquidsPointable = value;
    }

    public string? LightSource
    {
        get => Data.LightSource.ToString();
        set
        {
            if (int.TryParse(value, out var number)) Data.LightSource = number;
        }
    }

    public string? NodePlacementPrediction => Data.NodePlacementPrediction;
    public string? NodeDigPrediction => Data.NodeDigPrediction;


    protected override void UpdateProperties()
    {
        base.UpdateProperties();
        this.RaisePropertyChanged(nameof(Description));
        this.RaisePropertyChanged(nameof(ShortDescription));
        this.RaisePropertyChanged(nameof(InventoryImage));
        this.RaisePropertyChanged(nameof(WieldImage));
        this.RaisePropertyChanged(nameof(WieldOverlay));
        this.RaisePropertyChanged(nameof(Palette));
        this.RaisePropertyChanged(nameof(Color));
        this.RaisePropertyChanged(nameof(WieldScale));
        this.RaisePropertyChanged(nameof(StackMax));
        this.RaisePropertyChanged(nameof(Range));
        this.RaisePropertyChanged(nameof(LiquidsPointable));
        this.RaisePropertyChanged(nameof(LightSource));
        this.RaisePropertyChanged(nameof(NodePlacementPrediction));
        this.RaisePropertyChanged(nameof(NodeDigPrediction));
    }

    public ItemDefinitionFormViewModel()
    {
    }

    public ItemDefinitionFormViewModel(MainWindowViewModel mainWindowViewModel) : base(mainWindowViewModel)
    {
    }
}