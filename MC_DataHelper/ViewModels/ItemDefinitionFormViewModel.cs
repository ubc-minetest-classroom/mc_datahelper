using Avalonia.Media;
using MC_DataHelper.Models.DataDefinitions;
using ReactiveUI;

namespace MC_DataHelper.ViewModels;

public class ItemDefinitionFormViewModel : DataDefinitionFormViewModelBase<CraftItemDataDefinition>
{
    public string? Description
    {
        get => Data.Description;
        set => Data.Description = value;
    }

    public string? ShortDescription
    {
        get => Data.ShortDescription;
        set => Data.ShortDescription = value;
    }

    public string? InventoryImage
    {
        get => Data.InventoryImage;
        set => Data.InventoryImage = value;
    }

    public string? InventoryOverlay
    {
        get => Data.InventoryOverlay;
        set => Data.InventoryOverlay = value;
    }

    public string? WieldImage
    {
        get => Data.WieldImage;
        set => Data.WieldImage = value;
    }

    public string? WieldOverlay
    {
        get => Data.WieldOverlay;
        set => Data.WieldOverlay = value;
    }

    public string? Palette
    {
        get => Data.Palette;
        set => Data.Palette = value;
    }

    public Color Color
    {
        get => Color.TryParse(Data.Color, out var color) ? color : Colors.White;
        set => Data.Color = value.ToString();
    }

    public string? WieldScale
    {
        get => Data.WieldScale;
        set => Data.WieldScale = value;
    }

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
        get => Data.LiquidsPointable;
        set => Data.LiquidsPointable = value;
    }

    public int? LightSource
    {
        get => Data.LightSource;
        set => Data.LightSource = value;
    }

    public string? NodePlacementPrediction
    {
        get => Data.NodePlacementPrediction;
        set => Data.NodePlacementPrediction = value;
    }

    public string? NodeDigPrediction
    {
        get => Data.NodeDigPrediction;
        set => Data.NodeDigPrediction = value;
    }


    protected override void UpdateProperties()
    {
        base.UpdateProperties();
        this.RaisePropertyChanged(nameof(Description));
        this.RaisePropertyChanged(nameof(ShortDescription));
        this.RaisePropertyChanged(nameof(InventoryImage));
        this.RaisePropertyChanged(nameof(InventoryOverlay));
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