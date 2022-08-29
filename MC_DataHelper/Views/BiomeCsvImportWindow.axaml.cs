using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MC_DataHelper.ViewModels;
using ReactiveUI;

namespace MC_DataHelper.Views;

public partial class BiomeCsvImportWindow : ReactiveWindow<BiomeCsvImportWindowViewModel>
{
    public BiomeCsvImportWindow()
    {
        InitializeComponent();
        this.WhenActivated(d =>
            d(ViewModel?.ShowOpenFileDialog.RegisterHandler(ShowOpenFileDialog) ??
              throw new InvalidOperationException($"Viewmodel for {nameof(ViewModel)} is null.")));
    }


    private async Task ShowOpenFileDialog(InteractionContext<OpenFileDialog, string?> interaction)
    {
        var dialog = interaction.Input;
        var fileNames = await dialog.ShowAsync(this);
        interaction.SetOutput(fileNames?.FirstOrDefault());
    }
}