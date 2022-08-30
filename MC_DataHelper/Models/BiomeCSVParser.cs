using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using MC_DataHelper.ViewModels;

namespace MC_DataHelper.Models;

public class BiomeCsvParser
{
    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

    private CsvConfiguration _config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        HasHeaderRecord = true,
        DetectDelimiter = true,
        TrimOptions = TrimOptions.Trim,
        PrepareHeaderForMatch = args => args.Header.ToUpper(CultureInfo.InvariantCulture).Trim()
    };

    public string[]? ReadHeaderRow(string filepath)
    {
        try
        {
            using var reader = new StreamReader(filepath);
            using var csv = new CsvReader(reader, _config);

            csv.Read();
            csv.ReadHeader();
            var headers = csv.HeaderRecord?.ToArray();

            return headers;
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }

        return null;
    }

    public List<BiomeDataDefinition> ReadCsvToBiomeData(string filePath, BiomeDataDefinitionMap map)
    {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, _config);

        csv.Context.RegisterClassMap(map);

        var records = csv.GetRecords<BiomeDataDefinition>();

        //Pull the entire file into memory and convert to a list of BiomeDataDefinition objects
        return records.ToList();
    }
}

public sealed class BiomeDataDefinitionMap : ClassMap<BiomeDataDefinition>
{
    public BiomeDataDefinitionMap(List<FieldHeaderPair> pairs)
    {
        foreach (var variablePair in pairs)
        {
            AutoMap(CultureInfo.InvariantCulture);

            if (!string.IsNullOrEmpty(variablePair.Header))
            {
                Map(m => m.GetType().GetProperty(variablePair.Field)).Name(variablePair.Header);
            }
        }
    }
}