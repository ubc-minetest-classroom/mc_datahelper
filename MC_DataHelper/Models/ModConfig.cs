namespace MC_DataHelper.Models;

public record ModConfig
{
    public ModConfig(string modName, string description, string author, string dependencies, string optionalDependencies)
    {
        ModName = modName;
        Description = description;
        Author = author;
        Dependencies = dependencies;
        OptionalDependencies = optionalDependencies;
    }

    string ModName { get; set; }
    private string Description { get; set; }
    private string Author { get; set; }
    private string Dependencies { get; set; }
    private string OptionalDependencies { get; set; }
}