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

    private string _footerText = "TIP: No project is open. Navigate to File -> New / Open to begin.";


    private bool _isProjectOpen;


    private ModPackage? _selectedPackage;

    private int _selectedTabIndex;
    private ITreeViewNode? _selectedTreeViewItem;

    // Initialize everything
    public MainWindowViewModel()
    {
        BiomesDefinitionFormViewModel = new BiomesDefinitionFormViewModel(this);
        ItemsDefinitionFormViewModel = new ItemDefinitionFormViewModel(this);
        ModConfigViewModel = new ModConfigViewModel();

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

    public BiomesDefinitionFormViewModel BiomesDefinitionFormViewModel { get; }
    public ItemDefinitionFormViewModel ItemsDefinitionFormViewModel { get; }
    public ModConfigViewModel ModConfigViewModel { get; }

    private ModPackage? Package
    {
        get => _selectedPackage;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedPackage, value);
            this.RaisePropertyChanged(nameof(FooterText));

            ModConfigViewModel.Package = value;
            IsProjectOpen = value != null;
        }
    }

    public ObservableCollection<TreeViewFolderNode> TreeViewItems { get; } = new();

    public ITreeViewNode? SelectedTreeViewItem
    {
        get => _selectedTreeViewItem;
        set
        {
            switch (value)
            {
                case TreeViewFolderNode:
                    this.RaiseAndSetIfChanged(ref _selectedTreeViewItem, null);
                    SelectedTabIndex = 0;
                    break;
                case TreeViewDataNode dataNode:
                {
                    this.RaiseAndSetIfChanged(ref _selectedTreeViewItem, value);

                    SelectedTabIndex = dataNode.DataDefinition switch
                    {
                        BiomeDataDefinition => 1,
                        CraftItemDataDefinition => 2,
                        _ => 0
                    };

                    break;
                }
            }
        }
    }

    private TreeViewDataNode? _selectedNode;

    public TreeViewDataNode? SelectedNode
    {
        get => _selectedNode;
        set
        {
            _selectedNode = value;
            SelectedTreeViewItem = value;
        }
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


    public ValidationContext ValidationContext { get; } = new();

    private void EditTreeItem()
    {
        if (SelectedTreeViewItem is not TreeViewDataNode node)
        {
            return;
        }

        switch (node.DataDefinition)
        {
            case BiomeDataDefinition biomeDataDefinition:
                BiomesDefinitionFormViewModel.UpdateDataSource(biomeDataDefinition, node);
                SelectedTabIndex = 1;
                break;
            case CraftItemDataDefinition craftItemDataDefinition:
                ItemsDefinitionFormViewModel.UpdateDataSource(craftItemDataDefinition, node);
                SelectedTabIndex = 2;
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
        if (node.ParentNode.Children.Count == 0)
        {
            TreeViewItems.Remove((TreeViewFolderNode)node.ParentNode);
        }

        SelectedTreeViewItem = null;
        SelectedTabIndex = 0;
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