﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using MC_DataHelper.Models;
using MC_DataHelper.ViewModels.DataTreeView;
using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;

namespace MC_DataHelper.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IValidatableViewModel
    {
        public BiomeFormViewModel BiomeFormViewModel { get; set; }


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


        private readonly Dictionary<string, TreeViewFolderNode> _folders = new Dictionary<string, TreeViewFolderNode>();

        public ObservableCollection<TreeViewFolderNode> TreeViewItems { get; } = new();

        private ITreeViewNode? _selectedTreeViewItem;

        public ITreeViewNode? SelectedTreeViewItem
        {
            get => _selectedTreeViewItem;
            set
            {
                _selectedTreeViewItem = value;
                if (value != null)
                {
                    ChangeOpenEditorView(value);
                }
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

        public Interaction<Unit, BiomeCsvImportViewModel?> ShowBiomeCsvDialog { get; }
        public Interaction<OpenFileDialog, string?> ShowOpenFileDialog { get; }
        public Interaction<OpenFolderDialog, string?> ShowOpenFolderDialog { get; }


        private bool _isProjectOpen;

        public bool IsProjectOpen
        {
            get => _isProjectOpen;
            set => this.RaiseAndSetIfChanged(ref _isProjectOpen, value);
        }

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
        public MainWindowViewModel()
        {
            BiomeFormViewModel = new BiomeFormViewModel(this);

            ShowBiomeCsvDialog = new Interaction<Unit, BiomeCsvImportViewModel?>();
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
            UndoCommand = ReactiveCommand.Create(() => { });
            RedoCommand = ReactiveCommand.Create(() => { });
            CopyCommand = ReactiveCommand.Create(() => { });
            PasteCommand = ReactiveCommand.Create(() => { });


            DeleteTreeItemCommand = ReactiveCommand.Create(DeleteTreeItem);
            RefreshTreeItemsCommand = ReactiveCommand.Create(CreateTree);


            BiomeCsvWindowCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                await ShowBiomeCsvDialog.Handle(Unit.Default);
            });

            CreateTree();
        }

        public void AddDataDefinition(IDataDefinition dataDefinition)
        {
            if (Package == null) return;
            Package.DataDefinitions.Add(dataDefinition);
            AddTreeItem(dataDefinition);
        }

        public void CreateTree()
        {
            TreeViewItems.Clear();
            _folders.Clear();

            if (Package?.DataDefinitions == null) return;


            foreach (var dataDefinition in Package.DataDefinitions)
            {
                AddTreeItem(dataDefinition);
            }
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

            UpdateViewModels();
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

            UpdateViewModels();
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

            UpdateViewModels();
            CreateTree();
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

            UpdateViewModels();
            CreateTree();
        }

        private void UpdateViewModels()
        {
        }

        private void ChangeOpenEditorView(ITreeViewNode iTreeViewNode)
        {
            if (iTreeViewNode is not TreeViewDataNode node) return;
            if (node.DataDefinition is BiomeDataDefinition biomeDataDefinition)
            {
                BiomeFormViewModel.UpdateDataSource(biomeDataDefinition);
            }
        }

        public ValidationContext ValidationContext { get; } = new ValidationContext();
    }
}