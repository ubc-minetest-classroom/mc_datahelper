using System;
using System.Collections.Generic;

namespace MC_DataHelper.Models;

public class ModPackage
{
    public ModPackage()
    {
        Config = new ModConfig();
        DataDefinitions = new List<IDataDefinition>(0);
    }

    public static ModPackage LoadPackageFromDisk(string path)
    {
        //TODO: Load "package" from disk
        throw new NotImplementedException();
    }

    public void SavePackageToDisk(string path)
    {
        //TODO: Save mod "package" to disk
        throw new NotImplementedException();
    }

    public ModConfig Config { get; }
    public List<IDataDefinition> DataDefinitions { get; }
}