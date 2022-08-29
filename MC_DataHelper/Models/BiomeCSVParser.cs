using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace MC_DataHelper.Models;

public class BiomeCsvParser
{
    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();


    public string[]? ReadHeaderRow(string filepath)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            DetectDelimiter = true,
            TrimOptions = TrimOptions.Trim
        };

        try
        {
            using var reader = new StreamReader(filepath);
            using var csv = new CsvReader(reader, config);

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

    public List<BiomeDataDefinition> ReadCsvToBiomeData(string filePath)
    {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<BiomeDataDefinition>();

        //Pull the entire file into memory and convert to a list of BiomeDataDefinition objects
        return records.ToList();
    }
}