using System;
using System.Text.RegularExpressions;

namespace MC_DataHelper.Models;

public record ModConfig
{
    private string _name;

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
    private string Author { get; set; }
    public string Dependencies { get; set; }
    private string OptionalDependencies { get; set; }
}