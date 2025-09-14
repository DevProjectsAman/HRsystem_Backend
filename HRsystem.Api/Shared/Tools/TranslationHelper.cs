using System.Text.Json;

namespace HRsystem.Api.Shared.Tools
{
    public static class TranslationHelper
    {
        public static string GetTranslation(this string json, string language, string fallback = "en")
        {
            if (string.IsNullOrWhiteSpace(json))
                return string.Empty;

            try
            {
                var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                if (dict == null || dict.Count == 0)
                    return string.Empty;

                return dict.ContainsKey(language) ? dict[language]
                     : dict.ContainsKey(fallback) ? dict[fallback]
                     : dict.Values.FirstOrDefault() ?? string.Empty;
            }
            catch
            {
                return json; // fallback: return raw string if not valid JSON
            }
        }
    }

}
