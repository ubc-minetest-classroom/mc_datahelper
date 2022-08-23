using System;
using System.Text.RegularExpressions;

namespace MC_DataHelper.Models;

public record ModConfig
{
    private string _name;


    public ModConfig(string name, string description, string dependencies, string optionalDependencies, string author,
        string title)
    {
        _name = name;
        Description = description;
        Dependencies = dependencies;
        OptionalDependencies = optionalDependencies;
        Author = author;
        Title = title;
    }

    public ModConfig() : this("", "", "", "", "", "")
    {
    }

    public string Name
    {
        get => _name;
        set
        {
            var rgx = new Regex("[^a-zA-Z]");
            _name = rgx.Replace(value, string.Empty);
        }
    }

    public string Title { get; set; }
    public string Description { get; set; }
    public string Author { get; set; }
    public string Dependencies { get; set; }
    public string OptionalDependencies { get; set; }
}