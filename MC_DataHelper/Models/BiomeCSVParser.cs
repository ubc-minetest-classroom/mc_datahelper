using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using CsvHelper;
using CsvHelper.Configuration;
using MC_DataHelper.ViewModels;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NLog.Fluent;

namespace MC_DataHelper.Models;

public class BiomeCsvParser
{
    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

    private CsvConfiguration _config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        HasHeaderRecord = true,
        TrimOptions = TrimOptions.Trim,
        PrepareHeaderForMatch = args => args.Header.ToUpper(CultureInfo.InvariantCulture).Trim(),
        MissingFieldFound = MissingFieldFound,
        ReadingExceptionOccurred = ReadingExceptionOccurred
    };

    private static void MissingFieldFound(MissingFieldFoundArgs args)
    {
        Logger.Error($"Missing field {args.HeaderNames} at line {args.Index}");
    }

    private static bool ReadingExceptionOccurred(ReadingExceptionOccurredArgs args)
    {
        Logger.Error(args.Exception, "Error reading CSV file");
        return false;
    }

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
        try
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, _config);

            csv.Context.RegisterClassMap(map);

            var records = csv.GetRecords<BiomeDataDefinition>();

            //Pull the entire file into memory and convert to a list of BiomeDataDefinition objects
            return new List<BiomeDataDefinition>(records);
        }
        catch (Exception e)
        {
            Logger.Error(e);
            return new List<BiomeDataDefinition>(0);
        }
    }
}

public sealed class BiomeDataDefinitionMap : ClassMap<BiomeDataDefinition>
{
    public BiomeDataDefinitionMap(IEnumerable<FieldHeaderPair> pairs)
    {
        AutoMap(CultureInfo.InvariantCulture);
        Map(m => m.JsonType).Ignore(true);


        foreach (var variablePair in pairs)
        {
            if (string.IsNullOrEmpty(variablePair.Header))
            {
                Map(typeof(BiomeDataDefinition), typeof(BiomeDataDefinition).GetMember(variablePair.Field).First())
                    .Ignore(true);
            }
            else
            {
                Map(typeof(BiomeDataDefinition), typeof(BiomeDataDefinition).GetMember(variablePair.Field).First())
                    .Name(variablePair.Header);
            }
        }
    }
}