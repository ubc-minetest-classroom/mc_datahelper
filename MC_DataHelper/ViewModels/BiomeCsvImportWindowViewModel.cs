using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using MC_DataHelper.Models;
using ReactiveUI;

namespace MC_DataHelper.ViewModels;

public class BiomeCsvImportWindowViewModel : ViewModelBase
{
    public Interaction<OpenFileDialog, string?> ShowOpenFileDialog { get; }

    public ReactiveCommand<Unit, Unit> BrowseCsvFileCommand { get; }


    private string _filePath = "";

    public string FilePath
    {
        get => _filePath;
        set => _filePath = this.RaiseAndSetIfChanged(ref _filePath, value);
    }


    public BiomeCsvImportWindowViewModel()
    {
        ShowOpenFileDialog = new Interaction<OpenFileDialog, string?>();


        BrowseCsvFileCommand = ReactiveCommand.CreateFromTask(FindCsvFile);
    }


    private async Task FindCsvFile()
    {
        var filePath = await ShowOpenFileDialog.Handle(new OpenFileDialog
        {
            Title = "Select your biome CSV file",
            Directory = Environment.CurrentDirectory,
            Filters =
            {
                new FileDialogFilter { Name = "CSV Files", Extensions = { "csv" } },
                new FileDialogFilter { Name = "Biome CSV Files", Extensions = { "bcsv" } },
                new FileDialogFilter { Name = "All Files", Extensions = { "*" } }
            },
            AllowMultiple = false
        });

        if (filePath != null)
        {
            FilePath = filePath;
        }
    }
}