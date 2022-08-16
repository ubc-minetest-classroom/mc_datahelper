using System.Collections.Generic;

namespace MC_DataHelper.Models;

public class ModModel
{
    public ModConfig? Config { get; set; }
    public List<IDataDefinition> DataDefinitions { get; set; }
}