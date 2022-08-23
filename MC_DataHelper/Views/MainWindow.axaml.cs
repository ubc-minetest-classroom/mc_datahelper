using System;
using System.Linq;
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
            this.WhenActivated(d => d(ViewModel.ShowOpenFileDialog.RegisterHandler(ShowOpenFileDialog)));
            this.WhenActivated(d => d(ViewModel.ShowOpenFolderDialog.RegisterHandler(ShowOpenFolderDialog)));
        }

        private async Task DoShowDialogAsync(InteractionContext<Unit, BiomeCsvImportViewModel?> interaction)
        {
            var dialog = new BiomeCsvImportWindow();
            var result = await dialog.ShowDialog<BiomeCsvImportViewModel?>(this);
            interaction.SetOutput(result);
        }

        private async Task ShowOpenFileDialog(InteractionContext<OpenFileDialog, string?> interaction)
        {
            var dialog = interaction.Input;
            var fileNames = await dialog.ShowAsync(this);
            interaction.SetOutput(fileNames?.FirstOrDefault());
        }

        private async Task ShowOpenFolderDialog(InteractionContext<OpenFolderDialog, string?> interaction)
        {
            var dialog = interaction.Input;
            var directoryName = await dialog.ShowAsync(this);
            interaction.SetOutput(directoryName);
        }
        
    }
}