using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using DynamicData;
using MC_DataHelper.Helpers;
using MC_DataHelper.Models;
using ReactiveUI;

namespace MC_DataHelper.ViewModels;

public class BiomeCsvImportWindowViewModel : ViewModelBase
{
    private readonly ModPackage _modPackage;
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

    public ObservableCollection<FieldHeaderPair> FieldMatchList { get; }

    public ObservableCollection<string> CsvFieldNames { get; }


    public BiomeCsvImportWindowViewModel(ModPackage modPackage)
    {
        _modPackage = modPackage;
        ShowOpenFileDialog = new Interaction<OpenFileDialog, string?>();
        BrowseCsvFileCommand = ReactiveCommand.CreateFromTask(FindCsvFile);
        ImportCsvCommand = ReactiveCommand.CreateFromTask(ImportCsvFile);
        LoadCsvFileCommand = ReactiveCommand.CreateFromTask(LoadCsvFile);

        FieldMatchList = new ObservableCollection<FieldHeaderPair>();
        CsvFieldNames = new ObservableCollection<string>();
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

        CsvFieldNames.Clear();
        CsvFieldNames.AddRange(parser.ReadHeaderRow(FilePath) ?? Array.Empty<string>());

        SetupFieldMatchList();
    }

    private void SetupFieldMatchList()
    {
        FieldMatchList.Clear();
        var biomeFields = typeof(BiomeDataDefinition).GetPropertyNames();

        foreach (var field in biomeFields)
        {
            var match = string.Empty;
            if (CsvFieldNames.Contains(field))
            {
                match = field;
            }

            FieldMatchList.Add(new FieldHeaderPair(field, match));
        }
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

public class FieldHeaderPair : ReactiveObject
{
    private string _field;
    private string _header;

    public string Field
    {
        get => _field;
        set => this.RaiseAndSetIfChanged(ref _field, value);
    }

    public string Header
    {
        get => _header;
        set => this.RaiseAndSetIfChanged(ref _header, value);
    }

    public FieldHeaderPair(string field, string header)
    {
        Field = field;
        Header = header;
    }
}