using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
namespace LjudButikenBackEnd.Services;


// Quick walk-through
//  För-definerad för att kunna generera URL-slugs från input-strängar.
// Generate-metoden tar en sträng som input och returnerar en "slugified" version av den.

public static class Slugify
{
    public static string Generate(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;

        string normalized = input.ToLowerInvariant().Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();
        foreach (var c in normalized)
        {
            var uc = CharUnicodeInfo.GetUnicodeCategory(c);
            if (uc != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(c);
            }
        }

        string deaccented = sb.ToString().Normalize(NormalizationForm.FormC);
        //replace digits & special characters with - (hyphen)
        string hyphenated = Regex.Replace(deaccented, @"[^a-z0-9]+", "-").Trim('-');
        // collapse multiple hyphens
        hyphenated = Regex.Replace(hyphenated, "-+", "-");
        return hyphenated;
    }
}