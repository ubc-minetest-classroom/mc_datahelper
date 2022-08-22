using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MC_DataHelper.Models;

public class ModPackage
{
    public ModPackage() : this(new ModConfig(), new List<IDataDefinition?>(0))
    {
    }

    public ModPackage(ModConfig config) : this(config, new List<IDataDefinition?>(0))
    {
    }


    public ModPackage(ModConfig config, List<IDataDefinition?> dataDefinitions)
    {
        Config = config;
        DataDefinitions = dataDefinitions;
    }

    public static async Task<ModPackage> LoadPackageFromDisk(string path)
    {
        var confFileLines = await File.ReadAllLinesAsync(path + "/mod.conf");
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

        var config = new ModConfig
        {
            Name = confName ?? string.Empty,
            Description = confDescription ?? string.Empty,
            Dependencies = confDependencies ?? string.Empty,
            OptionalDependencies = confOptionalDependencies ?? string.Empty,
            Author = confAuthor ?? string.Empty,
            Title = confTitle ?? string.Empty
        };

        var dataDefinitions = new List<IDataDefinition?>();

        var files = Directory.GetFiles(path, "*.json", SearchOption.AllDirectories);
        foreach (var filePath in files)
        {
            IDataDefinition? dataDefinition = null;
            using StreamReader sr = new(filePath);

            var jsonObject = (JObject)await JToken.ReadFromAsync(new JsonTextReader(sr));
            var jsonType = jsonObject["_jsonType"].Value<string>();
            if (jsonType == null)
            {
                continue;
            }

            switch (jsonType.ToLower())
            {
                case "biome":
                    dataDefinition = jsonObject.ToObject<BiomeDataDefinition>();
                    break;
                default:
                    continue;
            }


            if (dataDefinition != null)
            {
                if (dataDefinition.DataName.Contains(':'))
                {
                    var splitString = dataDefinition.DataName.Split(':');
                    dataDefinition.DataName = splitString[splitString.GetUpperBound(0)];
                }

                dataDefinitions.Add(dataDefinition);
            }
        }


        return new ModPackage(config, dataDefinitions);
    }

    public async Task SavePackageToDisk(string path)
    {
        if (Directory.Exists($"{path}/data"))
        {
            Directory.Delete($"{path}/data", true);
        }

        var modConfLines = new[]
        {
            $"name = {Config.Name}",
            $"description = {Config.Description}",
            $"depends = mc_json_importer,{Config.Dependencies}",
            $"optional_depends ={Config.OptionalDependencies}",
            $"author = {Config.Author}",
            $"title = {Config.Title}",
        };

        await File.WriteAllLinesAsync($"{path}/mod.conf", modConfLines);

        const string initString =
            "json_importer.loadDirectory(minetest.get_modpath(minetest.get_current_modname()) .. \"\\\\data\\\\\")";
        await File.WriteAllTextAsync($"{path}/init.lua", initString);

        foreach (var dataDefinition in DataDefinitions)
        {
            var folder = dataDefinition.JsonType.ToLower();
            var filePath = $"{path}/data/{folder}s";
            var Filename = $"{filePath}/{dataDefinition.DataName}.json";
            Directory.CreateDirectory(filePath);

            dataDefinition.DataName = $"{Config.Name}:{dataDefinition.DataName}";

            var output = JsonConvert.SerializeObject(dataDefinition, Formatting.Indented);
            await File.WriteAllTextAsync(Filename, output);
        }
    }

    public ModConfig Config { get; }
    public List<IDataDefinition> DataDefinitions { get; }
}