using MC_DataHelper.Models;
using ReactiveUI;

namespace MC_DataHelper.ViewModels;

public class ModConfigViewModel : ReactiveObject
{
    private ModPackage? _selectedPackage;

    public ModPackage? Package
    {
        get => _selectedPackage;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedPackage, value);

            this.RaisePropertyChanged(nameof(ConfigName));
            this.RaisePropertyChanged(nameof(ConfigDescription));
            this.RaisePropertyChanged(nameof(ConfigDependencies));
            this.RaisePropertyChanged(nameof(ConfigOptionalDependencies));
            this.RaisePropertyChanged(nameof(ConfigAuthor));
            this.RaisePropertyChanged(nameof(ConfigTitle));
        }
    }

    public string ConfigName
    {
        get => Package?.Config.Name ?? string.Empty;
        set
        {
            if (Package != null) Package.Config.Name = value;
        }
    }

    public string ConfigDescription
    {
        get => Package?.Config.Description ?? string.Empty;
        set
        {
            if (Package != null) Package.Config.Description = value;
        }
    }

    public string ConfigDependencies
    {
        get => Package?.Config.Dependencies ?? string.Empty;
        set
        {
            if (Package != null) Package.Config.Dependencies = value;
        }
    }

    public string ConfigOptionalDependencies
    {
        get => Package?.Config.OptionalDependencies ?? string.Empty;
        set
        {
            if (Package != null) Package.Config.OptionalDependencies = value;
        }
    }

    public string ConfigAuthor
    {
        get => Package?.Config.Author ?? string.Empty;
        set
        {
            if (Package != null) Package.Config.Author = value;
        }
    }

    public string ConfigTitle
    {
        get => Package?.Config.Title ?? string.Empty;
        set
        {
            if (Package != null) Package.Config.Title = value;
        }
    }
}