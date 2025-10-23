using System.Text.Json;
using System.Text.Json.Serialization;

namespace HRsystem.Api.Shared.DTO
{
    public class LocalizedData
    {
        [JsonPropertyName("en")]
        public string en { get; set; } = string.Empty;

        [JsonPropertyName("ar")]
        public string ar { get; set; } = string.Empty;

        /// <summary>
        /// Gets a localized value safely, with automatic fallback.
        /// </summary>
        /// <param name="lang">Language code (e.g. "en", "ar", "EN", "AR")</param>
        /// <returns>Localized value or fallback</returns>
        public string GetValue(string lang)
        {
            if (string.IsNullOrWhiteSpace(lang))
                return en ?? ar ?? string.Empty;

            lang = lang.Trim().ToLowerInvariant();

            return lang switch
            {
                "ar" => !string.IsNullOrWhiteSpace(ar) ? ar : en ?? string.Empty,
                "en" => !string.IsNullOrWhiteSpace(en) ? en : ar ?? string.Empty,
                _ => en ?? ar ?? string.Empty
            };
        }

        /// <summary>
        /// Tries to detect the current culture from the thread and get localized value.
        /// </summary>
        public string GetCurrentCultureValue()
        {
            var current = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            return GetValue(current);
        }

        public override string ToString()
        {
            // Default to English if available
            return !string.IsNullOrWhiteSpace(en) ? en : ar;
        }
    }
}
