namespace MC_DataHelper.Models;

public record ModConfig
{
    string ModName { get; set; }
    private string Description { get; set; }
    private string Author { get; set; }
    private string Dependencies { get; set; }
    private string OptionalDependencies { get; set; }
}