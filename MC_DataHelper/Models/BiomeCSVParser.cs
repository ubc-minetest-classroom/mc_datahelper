using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;

namespace MC_DataHelper.Models;

public class BiomeCSVParser
{
    public List<BiomeDataDefinition> ReadCsv(string filePath)
    {
        var list = new List<BiomeDataDefinition>();

        
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<BiomeDataDefinition>();
        }
        
        
        return list;
    }
}