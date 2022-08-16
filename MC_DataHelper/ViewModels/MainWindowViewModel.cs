using System.Collections.ObjectModel;
using System.Reactive;
using System.Windows.Input;
using ReactiveUI;

namespace MC_DataHelper.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
   

        public bool IsProjectOpen
        {
            get { return true; }
        }
        
        public ObservableCollection<string> TreeViewItems { get; }


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


      
    }
}