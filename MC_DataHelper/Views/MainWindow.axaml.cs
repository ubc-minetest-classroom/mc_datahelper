using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using MC_DataHelper.ViewModels;
using ReactiveUI;

namespace MC_DataHelper.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WhenActivated(d => d(ViewModel!.ShowBiomeCsvDialog.RegisterHandler(DoShowDialogAsync)));
        }

        private async Task DoShowDialogAsync(InteractionContext<Unit, BiomeCsvImportViewModel?> interaction)
        {
            var dialog = new BiomeCsvImportWindow();

            var result = await dialog.ShowDialog<BiomeCsvImportViewModel?>(this);
            interaction.SetOutput(result);
        }
    }
}