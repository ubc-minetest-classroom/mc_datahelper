using MC_DataHelper.Models.DataDefinitions;

namespace MC_DataHelper.ViewModels;

public class ItemDefinitionFormViewModel : DataDefinitionFormViewModelBase<CraftItemDataDefinition>
{
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