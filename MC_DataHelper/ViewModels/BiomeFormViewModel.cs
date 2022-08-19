using MC_DataHelper.Models;

namespace MC_DataHelper.ViewModels;

public class BiomeFormViewModel
{ 
    private readonly BiomeDataDefinition _data;

    public BiomeFormViewModel(BiomeDataDefinition data)
    {
        this._data = data;
    }
}