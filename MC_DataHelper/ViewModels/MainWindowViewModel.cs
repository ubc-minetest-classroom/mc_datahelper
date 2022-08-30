using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using MC_DataHelper.Models;
using MC_DataHelper.Models.DataDefinitions;
using MC_DataHelper.ViewModels.DataTreeView;
using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;

namespace MC_DataHelper.ViewModels;

public class MainWindowViewModel : ViewModelBase, IValidatableViewModel
{
    private readonly Dictionary<string, TreeViewFolderNode> _folders = new();
    private bool _editingExistingBiome;

    private string _footerText = "TIP: No project is open. Navigate to File -> New / Open to begin.";


    private bool _isProjectOpen;


    private ModPackage? _selectedPackage;

    private int _selectedTabIndex;
    private ITreeViewNode? _selectedTreeViewItem;

    // Initialize everything
    public MainWindowViewModel()
    {
        BiomeFormViewModel = new BiomeFormViewModel(this);

        ShowBiomeCsvDialog = new Interaction<ModPackage, BiomeCsvImportWindowViewModel?>();
        _isProjectOpen = false;
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

        //TODO: Implement these commands
        UndoCommand = ReactiveCommand.Create(() => { });
        RedoCommand = ReactiveCommand.Create(() => { });
        CopyCommand = ReactiveCommand.Create(() => { });
        PasteCommand = ReactiveCommand.Create(() => { });


        DeleteTreeItemCommand = ReactiveCommand.Create(DeleteTreeItem);
        RefreshTreeItemsCommand = ReactiveCommand.Create(CreateTree);
        EditTreeItemCommand = ReactiveCommand.Create(EditTreeItem);


        BiomeCsvWindowCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            if (Package != null) await ShowBiomeCsvDialog.Handle(Package);
            CreateTree();
        });

        CreateTree();
    }

    public BiomeFormViewModel BiomeFormViewModel { get; }

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
            this.RaisePropertyChanged(nameof(FooterText));
        }
    }

    public ObservableCollection<TreeViewFolderNode> TreeViewItems { get; } = new();

    public ITreeViewNode? SelectedTreeViewItem
    {
        get => _selectedTreeViewItem;
        set
        {
            if (value is TreeViewFolderNode folder)
            {
                this.RaiseAndSetIfChanged(ref _selectedTreeViewItem, null);
            }
            else if (value is TreeViewDataNode dataNode)
            {
                this.RaiseAndSetIfChanged(ref _selectedTreeViewItem, value);

                if (dataNode.DataDefinition is BiomeDataDefinition biomeDataDefinition)
                {
                    SelectedTabIndex = 1;
                }
                else
                {
                    SelectedTabIndex = 0;
                }
            }
        }
    }


    public bool EditingExistingBiome
    {
        get => _editingExistingBiome;
        set => this.RaiseAndSetIfChanged(ref _editingExistingBiome, value);
    }

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

    public ReactiveCommand<Unit, Unit> DeleteTreeItemCommand { get; }
    public ReactiveCommand<Unit, Unit> RefreshTreeItemsCommand { get; }
    public ReactiveCommand<Unit, Unit> EditTreeItemCommand { get; }

    public Interaction<ModPackage, BiomeCsvImportWindowViewModel?> ShowBiomeCsvDialog { get; }
    public Interaction<OpenFileDialog, string?> ShowOpenFileDialog { get; }
    public Interaction<OpenFolderDialog, string?> ShowOpenFolderDialog { get; }

    public bool IsProjectOpen
    {
        get => _isProjectOpen;
        set => this.RaiseAndSetIfChanged(ref _isProjectOpen, value);
    }

    public string FooterText
    {
        get => !_isProjectOpen ? "TIP: No project is open. Navigate to File -> New / Open to begin." : _footerText;
        set => this.RaiseAndSetIfChanged(ref _footerText, value);
    }

    public int SelectedTabIndex
    {
        get => _selectedTabIndex;
        set => this.RaiseAndSetIfChanged(ref _selectedTabIndex, value);
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

    public ValidationContext ValidationContext { get; } = new();

    private void EditTreeItem()
    {
        if (SelectedTreeViewItem is not TreeViewDataNode node) return;
        switch (node.DataDefinition)
        {
            case BiomeDataDefinition biomeDataDefinition:
                BiomeFormViewModel.UpdateDataSource(biomeDataDefinition, node);
                SelectedTabIndex = 1;
                break;
        }
    }

    public void AddDataDefinition(IDataDefinition dataDefinition)
    {
        if (Package == null) return;
        Package.DataDefinitions.Add(dataDefinition);
        AddTreeItem(dataDefinition);
    }

    private void CreateTree()
    {
        TreeViewItems.Clear();
        _folders.Clear();

        if (Package?.DataDefinitions == null) return;


        foreach (var dataDefinition in Package.DataDefinitions) AddTreeItem(dataDefinition);
    }

    private void AddTreeItem(IDataDefinition dataDefinition)
    {
        if (!_folders.TryGetValue(dataDefinition.JsonType, out var folder))
        {
            folder = new TreeViewFolderNode($"{dataDefinition.JsonType}s");
            _folders.Add(dataDefinition.JsonType, folder);
            TreeViewItems.Add(folder);
        }

        folder.Children.Add(new TreeViewDataNode(folder, dataDefinition));
    }

    private void DeleteTreeItem()
    {
        if (Package == null || SelectedTreeViewItem is not TreeViewDataNode node) return;
        Package.DataDefinitions.Remove(node.DataDefinition);
        node.ParentNode.Children.Remove(node);
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
            Directory = Environment.CurrentDirectory
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
            Directory = Environment.CurrentDirectory
        });
        if (directoryPath != null)
        {
            Environment.CurrentDirectory = directoryPath;
            Package = new ModPackage();

            FooterText = directoryPath;
            // Package.SavePackageToDisk(fileName);
        }

        CreateTree();
    }

    private async Task OpenProjectAsync()
    {
        var directoryPath = await ShowOpenFolderDialog.Handle(new OpenFolderDialog
        {
            Title = "Select a folder to open a project from",
            Directory = Environment.CurrentDirectory
        });
        if (directoryPath != null && File.Exists($"{directoryPath}/mod.conf"))
        {
            Environment.CurrentDirectory = directoryPath;
            Package = await ModPackage.LoadPackageFromDisk(directoryPath);
            FooterText = directoryPath;
        }
        else
        {
            Package = null;
        }

        CreateTree();
    }
}