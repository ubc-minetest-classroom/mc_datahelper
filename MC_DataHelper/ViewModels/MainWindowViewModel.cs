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

namespace MC_DataHelper.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ModPackage? _selectedPackage;

        private ModPackage? Package
        {
            get => _selectedPackage;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedPackage, value);
                IsProjectOpen = value != null;
            }
        }

        private bool _isProjectOpen;

        public bool IsProjectOpen
        {
            get => _isProjectOpen;
            set => this.RaiseAndSetIfChanged(ref _isProjectOpen, value);
        }

        public ObservableCollection<DataDefinition> TreeViewItems { get; } = new();


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

        // Initialize everything
        public MainWindowViewModel()
        {
            ShowBiomeCsvDialog = new Interaction<Unit, BiomeCsvImportViewModel?>();
            ShowOpenFileDialog = new Interaction<OpenFileDialog, string?>();
            ShowOpenFolderDialog = new Interaction<OpenFolderDialog, string?>();

            NewProjectCommand = ReactiveCommand.CreateFromTask(NewProjectAsync);
            OpenProjectCommand = ReactiveCommand.CreateFromTask(OpenProjectAsync);
            SaveProjectCommand = ReactiveCommand.Create(() => { Package?.SavePackageToDisk(Environment.CurrentDirectory);});
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
                Package?.SavePackageToDisk(directoryPath);
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
                Package = ModPackage.LoadPackageFromDisk(directoryPath);
                FooterText = directoryPath;
            }
        }
    }
}