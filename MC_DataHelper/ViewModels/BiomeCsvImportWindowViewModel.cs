using System;
using System.Collections.Generic;
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

    private bool _csvLoaded = false;

    private string _footerText =
        "Step 1/3: Select the CSV file to import by clicking 'Browse' or by entering its path in the textbox above.";

    public bool CsvLoaded
    {
        get => _csvLoaded;
        set => this.RaiseAndSetIfChanged(ref _csvLoaded, value);
    }

    public string FooterText
    {
        get => _footerText;
        set => this.RaiseAndSetIfChanged(ref _footerText, value);
    }


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
            FooterText = "Step 2/3: CSV file selected. Click 'Load *.CSV' to load the file.";
        }
    }

    private async Task LoadCsvFile()
    {
        CsvLoaded = false;

        FooterText = "Step 2/3: CSV file selected. Click 'Load *.CSV' to load the file.";

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

        CsvLoaded = true;

        FooterText =
            "Step 3/3: Using the above fields, match each field in the biome definition to its corresponding header in the CSV file. Then, click 'Import *.CSV'";
    }

    private async Task ImportCsvFile()
    {
        if (!File.Exists(FilePath))
        {
            return;
        }

        var parser = new BiomeCsvParser();

        var map = new BiomeDataDefinitionMap(new List<FieldHeaderPair>(FieldMatchList));

        var biomes = parser.ReadCsvToBiomeData(FilePath, map);
        _modPackage.DataDefinitions.AddRange(biomes);
    }
}