using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MC_DataHelper.Helpers;

namespace MC_DataHelper.Models;

public class ModPackage
{
    public ModPackage() : this(new ModConfig(), new List<IDataDefinition>(0))
    {
    }

    public ModPackage(ModConfig config) : this(config, new List<IDataDefinition>(0))
    {
    }


    public ModPackage(ModConfig config, List<IDataDefinition> dataDefinitions)
    {
        Config = config;
        DataDefinitions = dataDefinitions;
    }

    public static async Task<ModPackage> LoadPackageFromDisk(string path)
    {
        var confFileLines = await File.ReadAllLinesAsync(path + "mod.conf");
        var confDictionary = new Dictionary<string, string>(4);

        foreach (var line in confFileLines)
        {
            var subs = line.Split("=");
            confDictionary.Add(subs[0].Trim().ToLower(), subs[1].Trim());
        }

        confDictionary.TryGetValue("name", out var confName);
        confDictionary.TryGetValue("description", out var confDescription);
        confDictionary.TryGetValue("depends", out var confDependencies);
        confDictionary.TryGetValue("optional_depends", out var confOptionalDependencies);
        confDictionary.TryGetValue("author", out var confAuthor);
        confDictionary.TryGetValue("title", out var confTitle);

        confDependencies = confDependencies?.Replace("mc_json_importer,", string.Empty);

        var config = new ModConfig(confName ?? string.Empty, confDescription ?? string.Empty,
            confDependencies ?? string.Empty, confOptionalDependencies ?? string.Empty,
            confAuthor ?? string.Empty, confTitle ?? string.Empty);


        return new ModPackage(config);
    }

    public async Task SavePackageToDisk(string path)
    {
        var modConfLines = new[]
        {
            "name = " + Config.Name,
            "description = " + Config.Description,
            "depends = mc_json_importer," + Config.Dependencies,
            "optional_depends =" + Config.OptionalDependencies,
            "author = " + Config.Author,
            "title = " + Config.Title,
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