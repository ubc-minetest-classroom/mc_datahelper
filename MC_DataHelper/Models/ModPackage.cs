using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

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

    public async Task SavePackageToDisk(string path)
    {
        var modConfLines = new[]
        {
            "name = " + Config.Name,
            "title = " + Config.Title,
            "description = " + Config.Description,
            "depends = mc_json_importer," + Config.Dependencies,
        };

        await File.WriteAllLinesAsync(path + "/mod.conf", modConfLines);

        const string initString =
            "json_importer.loadDirectory(minetest.get_modpath(minetest.get_current_modname()) .. \"\\\\data\\\\\")";
        await File.WriteAllTextAsync(path + "/init.lua", initString);

        Directory.CreateDirectory(path + "/data");
    }

    public ModConfig Config { get; }
    public List<IDataDefinition> DataDefinitions { get; }
}