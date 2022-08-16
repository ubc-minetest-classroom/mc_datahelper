using System.Collections.ObjectModel;
using System.Windows.Input;

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
        public ICommand NewProjectCommand { get; }
        public ICommand OpenProjectCommand { get; }
        public ICommand SaveProjectCommand { get; }
        public ICommand SaveProjectAsCommand { get; }
        public ICommand ExitCommand { get; }


        //Edit menu commands
        public ICommand UndoCommand { get; }
        public ICommand RedoCommand { get; }
        public ICommand CopyCommand { get; }
        public ICommand PasteCommand { get; }
        public ICommand BiomeCSVWindowCommand { get; }


      
    }
}