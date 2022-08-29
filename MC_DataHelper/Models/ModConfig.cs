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

    public virtual bool Equals(ModConfig? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _name == other._name && Title == other.Title && Description == other.Description &&
               Author == other.Author && Dependencies == other.Dependencies &&
               OptionalDependencies == other.OptionalDependencies;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_name, Title, Description, Author, Dependencies, OptionalDependencies);
    }
}