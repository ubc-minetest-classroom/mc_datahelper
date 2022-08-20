using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using MC_DataHelper.Models;
using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;

namespace MC_DataHelper.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IValidatableViewModel
    {
        public BiomeFormViewModel BiomeFormViewModel { get; }


        private ModPackage? _selectedPackage;

        private ModPackage? Package
        {
            get => _selectedPackage;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedPackage, value);
                IsProjectOpen = value != null;

                this.RaisePropertyChanged(nameof(ConfigName));
                this.RaisePropertyChanged(nameof(ConfigDescription));
                this.RaisePropertyChanged(nameof(ConfigDependencies));
                this.RaisePropertyChanged(nameof(ConfigOptionalDependencies));
                this.RaisePropertyChanged(nameof(ConfigAuthor));
                this.RaisePropertyChanged(nameof(ConfigTitle));
            }
        }

        // In debug mode, we'll default to the project being set as open so that we can test the UI.
#if DEBUG
        private bool _isProjectOpen = true;
#else
        private bool _isProjectOpen = false;
#endif


        public bool IsProjectOpen
        {
            get => _isProjectOpen;
            set => this.RaiseAndSetIfChanged(ref _isProjectOpen, value);
        }

        public ObservableCollection<BiomeDataDefinition> TreeViewItems { get; } = new();


        //File menu commands
        public ReactiveCommand<Unit, Unit> NewProjectCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenProjectCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveProjectCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveProjectAsCommand { get; }
        public ReactiveCommand<Unit, Unit> ExitCommand { get; }


        //Edit menu commands
        public ReactiveCommand<Unit, Unit> UndoCommand { get; }
        public ReactiveCommand<Unit, Unit> RedoCommand { get; }
        public ReactiveCommand<Unit, Unit> CopyCommand { get; }
        public ReactiveCommand<Unit, Unit> PasteCommand { get; }

        public ReactiveCommand<Unit, Unit> BiomeCsvWindowCommand { get; }

        public Interaction<Unit, BiomeCsvImportViewModel?> ShowBiomeCsvDialog { get; }
        public Interaction<OpenFileDialog, string?> ShowOpenFileDialog { get; }
        public Interaction<OpenFolderDialog, string?> ShowOpenFolderDialog { get; }

        private string _footerText = "TIP: No project is open. Navigate to File -> New / Open to begin.";

        public string FooterText
        {
            get => !_isProjectOpen ? "TIP: No project is open. Navigate to File -> New / Open to begin." : _footerText;
            set => this.RaiseAndSetIfChanged(ref _footerText, value);
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

        // Initialize everything
        public MainWindowViewModel(BiomeFormViewModel biomeFormViewModel)
        {
            BiomeFormViewModel = biomeFormViewModel;

            ShowBiomeCsvDialog = new Interaction<Unit, BiomeCsvImportViewModel?>();
            ShowOpenFileDialog = new Interaction<OpenFileDialog, string?>();
            ShowOpenFolderDialog = new Interaction<OpenFolderDialog, string?>();

            NewProjectCommand = ReactiveCommand.CreateFromTask(NewProjectAsync);
            OpenProjectCommand = ReactiveCommand.CreateFromTask(OpenProjectAsync);
            SaveProjectCommand = ReactiveCommand.CreateFromTask(SaveProjectAsync);
            SaveProjectAsCommand = ReactiveCommand.CreateFromTask(SaveProjectAsAsync);
            ExitCommand = ReactiveCommand.Create(() =>
            {
                if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                    desktop.Shutdown();
            });
            UndoCommand = ReactiveCommand.Create(() => { });
            RedoCommand = ReactiveCommand.Create(() => { });
            CopyCommand = ReactiveCommand.Create(() => { });
            PasteCommand = ReactiveCommand.Create(() => { });


            BiomeCsvWindowCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var result = await ShowBiomeCsvDialog.Handle(Unit.Default);
            });
        }

        private async Task SaveProjectAsync()
        {
            if (Package != null) await Package.SavePackageToDisk(Environment.CurrentDirectory);
        }

        private async Task SaveProjectAsAsync()
        {
            var directoryPath = await ShowOpenFolderDialog.Handle(new OpenFolderDialog
            {
                Title = "Select a folder to save the project to.",
                Directory = Environment.CurrentDirectory,
            });
            if (directoryPath != null)
            {
                Environment.CurrentDirectory = directoryPath;
                if (Package != null) await Package.SavePackageToDisk(directoryPath);
            }
        }


        private async Task NewProjectAsync()
        {
            var directoryPath = await ShowOpenFolderDialog.Handle(new OpenFolderDialog
            {
                Title = "Select a folder to create a new project in",
                Directory = Environment.CurrentDirectory,
            });
            if (directoryPath != null)
            {
                Environment.CurrentDirectory = directoryPath;
                Package = new ModPackage();

                FooterText = directoryPath;
                // Package.SavePackageToDisk(fileName);
            }
        }

        private async Task OpenProjectAsync()
        {
            var directoryPath = await ShowOpenFolderDialog.Handle(new OpenFolderDialog
            {
                Title = "Select a folder to open a project from",
                Directory = Environment.CurrentDirectory,
            });
            if (directoryPath != null)
            {
                Environment.CurrentDirectory = directoryPath;
                Package = await ModPackage.LoadPackageFromDisk(directoryPath);
                FooterText = directoryPath;
            }
        }

        public ValidationContext ValidationContext { get; } = new ValidationContext();
    }
}