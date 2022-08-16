﻿using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using MC_DataHelper.Models;
using ReactiveUI;

namespace MC_DataHelper.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public bool IsProjectOpen { get; set; } = true;

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


        // Initialize everything
        public MainWindowViewModel()
        {
            NewProjectCommand = ReactiveCommand.Create(NewProject);
            OpenProjectCommand = ReactiveCommand.Create(() => { });
            SaveProjectCommand = ReactiveCommand.Create(() => { });
            SaveProjectAsCommand = ReactiveCommand.Create(() => { });
            ExitCommand = ReactiveCommand.Create(() => { });
            UndoCommand = ReactiveCommand.Create(() => { });
            RedoCommand = ReactiveCommand.Create(() => { });
            CopyCommand = ReactiveCommand.Create(() => { });
            PasteCommand = ReactiveCommand.Create(() => { });
            ShowBiomeCsvDialog = new Interaction<Unit, BiomeCsvImportViewModel?>();
            BiomeCsvWindowCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var result = await ShowBiomeCsvDialog.Handle(Unit.Default);
            });
        }

        private void NewProject()
        {
        }
    }
}