using System.Collections.Generic;

namespace MC_DataHelper.Models;

public class DataDefinition : IDataDefinition
{
    public DataDefinition(string name)
    {
        Name = name;
    }

    public string Name { get; }
}

