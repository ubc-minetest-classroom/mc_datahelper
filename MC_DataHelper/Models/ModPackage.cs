using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MC_DataHelper.Helpers;
using MC_DataHelper.Models.DataDefinitions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MC_DataHelper.Models;

public class ModPackage
{
    public ModPackage() : this(new ModConfig(), new List<IDataDefinition>(0))
    {
    }

    public ModPackage(ModConfig config) : this(config, new List<IDataDefinition>(0))
    {
    }


    private ModPackage(ModConfig config, List<IDataDefinition> dataDefinitions)
    {
        Config = config;
        DataDefinitions = dataDefinitions;
    }

    public ModConfig Config { get; }
    public List<IDataDefinition> DataDefinitions { get; }

    private readonly JsonSerializerSettings _jsonSettings = new()
    {
        TypeNameHandling = TypeNameHandling.None,
        Formatting = Formatting.Indented,
        NullValueHandling = NullValueHandling.Ignore,
        ContractResolver = new NoNullWhiteSpaceResolver(),
    };

    public static async Task<ModPackage> LoadPackageFromDisk(string path)
    {
        var confFileLines = await File.ReadAllLinesAsync($"{path}/mod.conf");
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

        var dataDefinitions = new List<IDataDefinition>();

        var files = Directory.GetFiles(path, "*.json", SearchOption.AllDirectories);
        foreach (var filePath in files)
        {
            IDataDefinition? dataDefinition;
            using StreamReader sr = new(filePath);

            var jsonObject = (JObject)await JToken.ReadFromAsync(new JsonTextReader(sr));
            var jsonType = jsonObject["_jsonType"]?.Value<string>();
            if (jsonType == null) continue;

            //TODO: Auto Register Data Definitions
            switch (jsonType.ToLowerInvariant())
            {
                case "biome":
                    dataDefinition = jsonObject.ToObject<BiomeDataDefinition>();
                    break;
                case "craftitem":
                    dataDefinition = jsonObject.ToObject<CraftItemDataDefinition>();
                    break;
                default:
                    dataDefinition = new UnknownDataDefinition(jsonObject.ToObject<dynamic>());
                    break;
            }


            if (dataDefinition == null) continue;
            if (dataDefinition.Name.Contains(':'))
            {
                var splitString = dataDefinition.Name.Split(':');
                dataDefinition.Name = splitString[splitString.GetUpperBound(0)];
            }

            dataDefinitions.Add(dataDefinition);
        }


        return new ModPackage(config, dataDefinitions);
    }

    public async Task SavePackageToDisk(string path)
    {
        if (Directory.Exists($"{path}/data")) Directory.Delete($"{path}/data", true);

        var modConfLines = new[]
        {
            $"name = {Config.Name}",
            $"description = {Config.Description}",
            $"depends = mc_json_importer,{Config.Dependencies}",
            $"optional_depends ={Config.OptionalDependencies}",
            $"author = {Config.Author}",
            $"title = {Config.Title}"
        };

        await File.WriteAllLinesAsync($"{path}/mod.conf", modConfLines);

        const string initString =
            "json_importer.loadDirectory(minetest.get_modpath(minetest.get_current_modname()) .. \"\\\\data\\\\\")";
        await File.WriteAllTextAsync($"{path}/init.lua", initString);

        Directory.CreateDirectory($"{path}/data");

        foreach (var dataDefinition in DataDefinitions)
        {
            var oldName = dataDefinition.Name;

            var folderName = dataDefinition.JsonType.ToLower();
            var filePath = $"{path}/data/{folderName}s";
            var filename = FileNameHelper.NextAvailableFilename($"{filePath}/{oldName}.json");

            dataDefinition.Name = $"{Config.Name}:{dataDefinition.Name}";

            Directory.CreateDirectory(filePath);


            object serializedObject;
            if (dataDefinition is UnknownDataDefinition definition)
                serializedObject = definition.Data;
            else
                serializedObject = dataDefinition;

            var output = JsonConvert.SerializeObject(serializedObject, _jsonSettings);
            await File.WriteAllTextAsync(filename, output);

            dataDefinition.Name = oldName;
        }
    }


    private bool Equals(ModPackage other)
    {
        return Config.Equals(other.Config) && DataDefinitions.Equals(other.DataDefinitions);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((ModPackage)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Config, DataDefinitions);
    }
}