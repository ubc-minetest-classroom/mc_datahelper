using System;
using System.IO;

namespace MC_DataHelper.Helpers;

//Adapted from https://stackoverflow.com/a/1078898
public static class FileNameHelper
{
    private const string numberPattern = " ({0})";

    public static string NextAvailableFilename(string path)
    {
        // Short-cut if already available
        if (!File.Exists(path))
            return path;

        // If path has extension then insert the number pattern just before the extension and return next filename
        return Path.HasExtension(path)
            ? GetNextFilename(path.Insert(path.LastIndexOf(Path.GetExtension(path)), numberPattern))
            : GetNextFilename(path + numberPattern);

        // Otherwise just append the pattern to the path and return next filename
    }

    private static string GetNextFilename(string pattern)
    {
        var tmp = string.Format(pattern, 1);
        if (tmp == pattern)
            throw new ArgumentException("The pattern must include an index place-holder", nameof(pattern));

        if (!File.Exists(tmp))
            return tmp; // short-circuit if no matches

        int min = 1, max = 2; // min is inclusive, max is exclusive/untested

        while (File.Exists(string.Format(pattern, max)))
        {
            min = max;
            max *= 2;
        }

        while (max != min + 1)
        {
            var pivot = (max + min) / 2;
            if (File.Exists(string.Format(pattern, pivot)))
                min = pivot;
            else
                max = pivot;
        }

        return string.Format(pattern, max);
    }
}