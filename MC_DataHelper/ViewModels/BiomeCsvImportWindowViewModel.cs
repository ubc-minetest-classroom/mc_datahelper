using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using MC_DataHelper.Helpers;
using MC_DataHelper.Models;
using ReactiveUI;

namespace MC_DataHelper.ViewModels;

public class BiomeCsvImportWindowViewModel : ViewModelBase
{
    private ModPackage _modPackage;
    public Interaction<OpenFileDialog, string?> ShowOpenFileDialog { get; }
    public ReactiveCommand<Unit, Unit> BrowseCsvFileCommand { get; }
    public ReactiveCommand<Unit, Unit> ImportCsvCommand { get; }

    public ReactiveCommand<Unit, Unit> LoadCsvFileCommand { get; }


    private string _filePath = @"C:\Users\Lukas Olson\Desktop\Minetest BEC Zones Biomes.csv";

    public string FilePath
    {
        get => _filePath;
        set => _filePath = this.RaiseAndSetIfChanged(ref _filePath, value);
    }

    public Dictionary<string, string> FieldMatchDictionary { get; }

    public List<string> CSVFieldNames { get; }


    public BiomeCsvImportWindowViewModel(ModPackage modPackage)
    {
        _modPackage = modPackage;
        ShowOpenFileDialog = new Interaction<OpenFileDialog, string?>();
        BrowseCsvFileCommand = ReactiveCommand.CreateFromTask(FindCsvFile);
        ImportCsvCommand = ReactiveCommand.CreateFromTask(ImportCsvFile);
        LoadCsvFileCommand = ReactiveCommand.CreateFromTask(LoadCsvFile);

        FieldMatchDictionary = new Dictionary<string, string>();
        CSVFieldNames = new List<string>();
    }

    public BiomeCsvImportWindowViewModel() : this(new ModPackage())
    {
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

    private async Task LoadCsvFile()
    {
        if (!File.Exists(FilePath))
        {
            return;
        }

        var parser = new BiomeCsvParser();
        var headerRow = parser.ReadHeaderRow(FilePath);

        SetupFieldMatchDictionary();
    }

    private void SetupFieldMatchDictionary()
    {
        FieldMatchDictionary.Clear();
        var recordProperties = typeof(BiomeDataDefinition).GetPropertyNames();
    }

    private async Task ImportCsvFile()
    {
        if (!File.Exists(FilePath))
        {
            return;
        }

        var parser = new BiomeCsvParser();
        var biomes = parser.ReadCsvToBiomeData(FilePath);
        _modPackage.DataDefinitions.AddRange(biomes);
    }
}