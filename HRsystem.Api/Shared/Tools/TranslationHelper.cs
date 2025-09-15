using HRsystem.Api.Shared.DTO;
using System.Text.Json;

namespace HRsystem.Api.Shared.Tools
{
    public static class TranslationHelper
    {
        //public static string GetTranslation(this string json, string language, string fallback = "en")
        //{
        //    if (string.IsNullOrWhiteSpace(json))
        //        return string.Empty;

        //    try
        //    {
        //        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        //        if (dict == null || dict.Count == 0)
        //            return string.Empty;

        //        return dict.ContainsKey(language) ? dict[language]
        //             : dict.ContainsKey(fallback) ? dict[fallback]
        //             : dict.Values.FirstOrDefault() ?? string.Empty;
        //    }
        //    catch
        //    {
        //        return json; // fallback: return raw string if not valid JSON
        //    }
        //}


        public static string GetTranslation(this LocalizedData data, string language, string fallback = "en")
        {
            if (data == null) return string.Empty;

            return language.ToLower() switch
            {
                "en" when !string.IsNullOrWhiteSpace(data.En) => data.En,
                "ar" when !string.IsNullOrWhiteSpace(data.Ar) => data.Ar,
                _ when fallback == "ar" && !string.IsNullOrWhiteSpace(data.Ar) => data.Ar,
                _ when fallback == "en" && !string.IsNullOrWhiteSpace(data.En) => data.En,
                _ => data.En ?? data.Ar ?? string.Empty
            };
        }

    }

}
