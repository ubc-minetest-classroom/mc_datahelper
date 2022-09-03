using MC_DataHelper.Models.DataDefinitions;

namespace MC_DataHelper.ViewModels;

public class ItemDefinitionFormViewModel : DataDefinitionFormViewModelBase<CraftItemDataDefinition>
{
    public string? Description => Data.Description;

    public string? ShortDescription => Data.ShortDescription;

    public string? InventoryImage => Data.InventoryImage;

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

    protected override void UpdateProperties()
    {
        base.UpdateProperties();
    }

    public ItemDefinitionFormViewModel() : base()
    {
    }

    public ItemDefinitionFormViewModel(MainWindowViewModel mainWindowViewModel) : base(mainWindowViewModel)
    {
    }
}