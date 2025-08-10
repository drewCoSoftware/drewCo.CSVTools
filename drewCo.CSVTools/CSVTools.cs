using CsvHelper;
using CsvHelper.Configuration;
using System.Formats.Asn1;
using System.Globalization;

namespace drewCo.CsvTools;

// ==============================================================================================================================
/// <summary>
/// Convenience functions for CsvHelper features.
/// </summary>
// TODO: We should make a silly little nuget package for these funtions, just so they don't have to be copied all over the place.
public static class CsvTools
{

    // ------------------------------------------------------------------------------------------------------------
    // TODO: SHARE:
    public static List<TData> ReadCSV<TData>(string usePath)
    {
        using (var fs = new StreamReader(usePath))
        using (var r = new CsvReader(fs, CultureInfo.InvariantCulture))
        {
            var allLines = r.GetRecords<TData>().ToList();
            return allLines;
        }
    }

    // ------------------------------------------------------------------------------------------------------------
    // TODO: SHARE:
    // This version should be used when you don't want to close the underlying stream.
    public static List<TData> ReadCSV<TData>(Stream s)
    {
        using (var fs = new StreamReader(s, leaveOpen: true))
        using (var r = new CsvReader(fs, CultureInfo.InvariantCulture))
        {
            var allLines = r.GetRecords<TData>().ToList();
            return allLines;
        }
    }

    // ------------------------------------------------------------------------------------------------------------
    // TODO: SHARE:
    public static void AppendCSVData<TData>(Stream s, TData toAppend)
    {
        AppendCSVData<TData>(s, new[] { toAppend });
    }

    // ------------------------------------------------------------------------------------------------------------
    // TODO: SHARE:
    public static void AppendCSVData<TData>(Stream s, IEnumerable<TData> toAppend)
    {
        bool isFirst = s.Length == 0;
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = isFirst,
        };

        using (var fs = new StreamWriter(s, leaveOpen: true))
        using (var w = new CsvWriter(fs, config, true))
        {
            // Will this work?
            if (!isFirst)
            {
                s.Seek(0, SeekOrigin.End);
            }
            w.WriteRecords(toAppend);
        }
    }
}