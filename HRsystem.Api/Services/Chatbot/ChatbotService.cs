//using System.Text.Json;
//using HRsystem.Api.Services.Chatbot;
//using System.Net.Http;
//using System.Text;
//using Microsoft.Extensions.Configuration; // للتكوين

//namespace HRsystem.Api.Services.Chatbot
//{
//    // ... (بقية الكلاس) ...
//    public class ChatbotService
//    {
//        private readonly IntentExecutorService _intentExecutor;
//        private readonly Dictionary<string, IntentTemplate> _intents;
//        private readonly HttpClient _httpClient;
//        private readonly string _geminiApiKey;
//        // تم استخدام gemini-2.5-flash لضمان التوافق مع API v1
//        private const string GEMINI_MODEL = "gemini-2.5-flash";

//        // ... (Constructor) ...

//        public ChatbotService(IntentExecutorService intentExecutor)
//        {
//            _intentExecutor = intentExecutor;

//            // تحميل الـ intents file
//            var jsonPath = Path.Combine(AppContext.BaseDirectory, "Services", "Chatbot", "intent_templates.json");
//            if (!File.Exists(jsonPath))
//                throw new FileNotFoundException("intent_templates.json not found.", jsonPath);

//            var json = File.ReadAllText(jsonPath);
//            _intents = JsonSerializer.Deserialize<Dictionary<string, IntentTemplate>>(json)!;

//            _httpClient = new HttpClient();

//            // استخدام ConfigurationBuilder للقراءة من appsettings.json
//            var config = new ConfigurationBuilder()
//                .SetBasePath(Directory.GetCurrentDirectory())
//                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
//                .Build();

//            _geminiApiKey = config["Gemini:ApiKey"]
//                ?? throw new Exception("Gemini API key not found in appsettings.json or Environment Variables.");
//        }

//        public async Task<object> AskAsync(string userMessage)
//        {
//            // ⭐️⭐️ إعطاء Gemini تاريخ اليوم الحالي كمعلومات إضافية في الـ Prompt ⭐️⭐️
//            var currentDate = DateTime.Today.ToString("yyyy-MM-dd");

//            var prompt = $@"
//You are a professional HR chatbot connected to an internal HR database. 
//Today's date is: {currentDate}.
//Your task: Identify which intent matches the user's question.

//Available intents:
//{string.Join(", ", _intents.Keys)}

//If the user asks for 'today' or 'now', use the current date: {currentDate} for the 'date' parameter.

//Return ONLY the JSON object. Do not include any surrounding text or markdown formatting (like ```json or ```).

//Return JSON in this format:
//{{ 
//  ""intent"": ""intent_name"", 
//  ""parameters"": {{ ""param1"": ""value"", ""param2"": ""value"" }} 
//}}

//If the question doesn't match, return:
//{{ ""intent"": ""fallback_general"", ""parameters"": {{}} }}

//Question: {userMessage}
//";
//            // ... (بقية الكود الخاص باستدعاء Gemini) ...

//            var requestBody = new
//            {
//                contents = new[]
//                {
//                    new
//                    {
//                        role = "user",
//                        parts = new[] { new { text = prompt } }
//                    }
//                }
//            };

//            var geminiUrl = $"https://generativelanguage.googleapis.com/v1/models/gemini-2.5-flash:generateContent?key={_geminiApiKey}";
//            // لاحظ: قمنا بإزالة : قبل generateContent ووضعنا / مكانه


//            var requestJson = JsonSerializer.Serialize(requestBody);
//            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

//            var httpResponse = await _httpClient.PostAsync(geminiUrl, content);
//            httpResponse.EnsureSuccessStatusCode();

//            var responseString = await httpResponse.Content.ReadAsStringAsync();
//            var geminiResponse = JsonSerializer.Deserialize<JsonElement>(responseString);

//            var responseText = geminiResponse
//                .GetProperty("candidates")[0]
//                .GetProperty("content")
//                .GetProperty("parts")[0]
//                .GetProperty("text")
//                .GetString();

//            // ⭐️ خطوة التنظيف لمنع JsonException (تمت إضافتها سابقًا)
//            var cleanedJsonText = responseText!
//                .Replace("```json", "")
//                .Replace("```", "")
//                .Trim();

//            var parsed = JsonSerializer.Deserialize<JsonElement>(cleanedJsonText);

//            var intentName = parsed.GetProperty("intent").GetString() ?? "fallback_general";
//            var parameters = JsonSerializer.Deserialize<Dictionary<string, object>>(
//                parsed.GetProperty("parameters").ToString()
//            ) ?? new Dictionary<string, object>();

//            // ⭐️⭐️ الخطوة الحاسمة: تعويض التاريخ المفقود بـ تاريخ اليوم ⭐️⭐️
//            // هذا لضمان أن IntentExecutorService لا يرمي استثناء إذا كانت النية تتطلب 'date'
//            if (_intents.TryGetValue(intentName, out var intentTemplate))
//            {
//                if (intentTemplate.Parameters != null && intentTemplate.Parameters.Contains("date") && !parameters.ContainsKey("date"))
//                {
//                    // نضيف تاريخ اليوم فقط إذا كانت النية تتطلبه ولم يتم استخلاصه
//                    parameters["date"] = currentDate;
//                }
//            }

//            var result = await _intentExecutor.ExecuteAsync(intentName, parameters);

//            return new
//            {
//                Intent = intentName,
//                Answer = result
//            };
//        }
//    }
//}

//using System.Text;
//using System.Text.Json;
//using HRsystem.Api.Services.Chatbot;
//using Microsoft.Extensions.Configuration;

//namespace HRsystem.Api.Services.Chatbot
//{
//    public class ChatbotService
//    {
//        private readonly IntentExecutorService _intentExecutor;
//        private readonly Dictionary<string, IntentTemplate> _intents;
//        private readonly Dictionary<string, IntentGroupTemplate> _intentGroups;
//        private readonly HttpClient _httpClient;
//        private readonly string _geminiApiKey;
//        private const string GEMINI_MODEL = "gemini-2.5-flash";

//        public ChatbotService(IntentExecutorService intentExecutor)
//        {
//            _intentExecutor = intentExecutor;

//            // ✅ تحميل intents
//            var intentPath = Path.Combine(AppContext.BaseDirectory, "Services", "Chatbot", "intent_templates.json");
//            if (!File.Exists(intentPath))
//                throw new FileNotFoundException("intent_templates.json not found.", intentPath);
//            var intentJson = File.ReadAllText(intentPath);
//            _intents = JsonSerializer.Deserialize<Dictionary<string, IntentTemplate>>(intentJson)!;

//            // ✅ تحميل intent groups (اختياري لكن مفيد للـ LLM)
//            var groupPath = Path.Combine(AppContext.BaseDirectory, "Services", "Chatbot", "intent_groups.json");
//            if (File.Exists(groupPath))
//            {
//                var groupJson = File.ReadAllText(groupPath);
//                _intentGroups = JsonSerializer.Deserialize<Dictionary<string, IntentGroupTemplate>>(groupJson)!;
//            }
//            else
//            {
//                _intentGroups = new();
//            }

//            _httpClient = new HttpClient();

//            // ✅ قراءة مفتاح Gemini من appsettings.json
//            var config = new ConfigurationBuilder()
//                .SetBasePath(Directory.GetCurrentDirectory())
//                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
//                .Build();

//            _geminiApiKey = config["Gemini:ApiKey"]
//                ?? throw new Exception("Gemini API key not found in appsettings.json or Environment Variables.");
//        }

//        public async Task<object> AskAsync(string userMessage)
//        {
//            var currentDate = DateTime.Today.ToString("yyyy-MM-dd");

//            // ✅ بناء prompt ذكي يعتمد على الجروبات
//            var groupSummary = _intentGroups.Count > 0
//                ? string.Join("\n", _intentGroups.Select(g =>
//                    $"{g.Value.Title}: {g.Value.Description}\nContains: {string.Join(", ", g.Value.Intents)}"))
//                : string.Join(", ", _intents.Keys);

//            var prompt = $@"
//You are an internal HR chatbot connected to a company's HR database. 
//Today's date is: {currentDate}.

//Here are the available intent groups:
//{groupSummary}

//Match the user's question to one of the intents listed above.

//If the question includes 'today' or 'now', use the current date: {currentDate} for the 'date' parameter.

//Return ONLY a valid JSON object:
//{{
//  ""intent"": ""intent_name"",
//  ""parameters"": {{ ""param1"": ""value"", ""param2"": ""value"" }}
//}}

//If you can't find a match, return:
//{{ ""intent"": ""fallback_general"", ""parameters"": {{}} }}

//Question: {userMessage}
//";

//            var requestBody = new
//            {
//                contents = new[]
//                {
//                    new
//                    {
//                        role = "user",
//                        parts = new[] { new { text = prompt } }
//                    }
//                }
//            };

//            var geminiUrl =
//                $"https://generativelanguage.googleapis.com/v1/models/{GEMINI_MODEL}:generateContent?key={_geminiApiKey}";

//            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

//            var httpResponse = await _httpClient.PostAsync(geminiUrl, content);
//            httpResponse.EnsureSuccessStatusCode();

//            var responseString = await httpResponse.Content.ReadAsStringAsync();
//            var geminiResponse = JsonSerializer.Deserialize<JsonElement>(responseString);

//            var responseText = geminiResponse
//                .GetProperty("candidates")[0]
//                .GetProperty("content")
//                .GetProperty("parts")[0]
//                .GetProperty("text")
//                .GetString();

//            var cleanedJsonText = responseText!
//                .Replace("```json", "")
//                .Replace("```", "")
//                .Trim();

//            var parsed = JsonSerializer.Deserialize<JsonElement>(cleanedJsonText);

//            var intentName = parsed.GetProperty("intent").GetString() ?? "fallback_general";
//            var parameters = JsonSerializer.Deserialize<Dictionary<string, object>>(
//                parsed.GetProperty("parameters").ToString()
//            ) ?? new Dictionary<string, object>();

//            // ✅ تعويض التاريخ المفقود تلقائيًا
//            if (_intents.TryGetValue(intentName, out var intentTemplate))
//            {
//                if (intentTemplate.Parameters != null &&
//                    intentTemplate.Parameters.Contains("date") &&
//                    !parameters.ContainsKey("date"))
//                {
//                    parameters["date"] = currentDate;
//                }
//            }

//            var result = await _intentExecutor.ExecuteAsync(intentName, parameters);

//            return new
//            {
//                Intent = intentName,
//                Answer = result
//            };
//        }
//    }

//    // 📄 تعريف بسيط لهيكل الجروب
//    public class IntentGroupTemplate
//    {
//        public string Title { get; set; } = string.Empty;
//        public string Description { get; set; } = string.Empty;
//        public List<string> Intents { get; set; } = new();
//    }
//}

// ========================================
// 📄 ChatbotService.cs (Updated with Examples + Lookup)
// ========================================
using System.Text;
using System.Text.Json;
using HRsystem.Api.Database;
using HRsystem.Api.Services.Chatbot;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HRsystem.Api.Services.Chatbot
{
    public class ChatbotService
    {
        private readonly IntentExecutorService _intentExecutor;
        private readonly DBContextHRsystem _db;
        private readonly Dictionary<string, IntentTemplate> _intents;
        private readonly Dictionary<string, IntentGroupTemplate> _intentGroups;
        private readonly HttpClient _httpClient;
        private readonly string _geminiApiKey;
        private const string GEMINI_MODEL = "gemini-2.5-flash";

        public ChatbotService(IntentExecutorService intentExecutor, DBContextHRsystem db)
        {
            _intentExecutor = intentExecutor;
            _db = db;

            // ✅ تحميل intents
            var intentPath = Path.Combine(AppContext.BaseDirectory, "Services", "Chatbot", "intent_templates.json");
            if (!File.Exists(intentPath))
                throw new FileNotFoundException("intent_templates.json not found.", intentPath);

            var intentJson = File.ReadAllText(intentPath);
            _intents = JsonSerializer.Deserialize<Dictionary<string, IntentTemplate>>(intentJson)!;

            // ✅ تحميل intent groups
            var groupPath = Path.Combine(AppContext.BaseDirectory, "Services", "Chatbot", "intent_groups.json");
            if (File.Exists(groupPath))
            {
                var groupJson = File.ReadAllText(groupPath);
                _intentGroups = JsonSerializer.Deserialize<Dictionary<string, IntentGroupTemplate>>(groupJson)!;
            }
            else
            {
                _intentGroups = new();
            }

            _httpClient = new HttpClient();

            // ✅ قراءة مفتاح Gemini
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            _geminiApiKey = config["Gemini:ApiKey"]
                ?? throw new Exception("Gemini API key not found in appsettings.json");
        }

        public async Task<object> AskAsync(string userMessage)
        {
            var currentDate = DateTime.Today.ToString("yyyy-MM-dd");

            // ✅ جلب Lookup Data من Database (Departments, VacationTypes)
            var lookupData = await BuildLookupDataAsync();

            // ✅ بناء Prompt مع Examples
            var intentDetails = BuildIntentDetailsWithExamples();

            var prompt = $@"
You are an HR chatbot for a company. Today's date: {currentDate}

📋 Available Intents and Examples:
{intentDetails}

🗂️ Lookup Data (use this to map names to IDs):
{lookupData}

📌 Rules:
1. Match user question to the MOST relevant intent
2. Extract parameters accurately
3. For department/vacation names, return the EXACT ID from lookup data
4. If user says 'today' or 'now', use date: {currentDate}
5. If user says 'this month', extract: month={DateTime.Today.Month}, year={DateTime.Today.Year}

Return ONLY valid JSON (no markdown, no extra text):
{{
  ""intent"": ""intent_name"",
  ""parameters"": {{ ""param1"": ""value"" }}
}}

If no match: {{ ""intent"": ""fallback_general"", ""parameters"": {{}} }}

User Question: {userMessage}
";

            // ✅ استدعاء Gemini
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        role = "user",
                        parts = new[] { new { text = prompt } }
                    }
                }
            };

            var geminiUrl = $"https://generativelanguage.googleapis.com/v1/models/{GEMINI_MODEL}:generateContent?key={_geminiApiKey}";
            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var httpResponse = await _httpClient.PostAsync(geminiUrl, content);
            httpResponse.EnsureSuccessStatusCode();

            var responseString = await httpResponse.Content.ReadAsStringAsync();

            // ⬇️ DEBUG: طباعة Response من Gemini
            Console.WriteLine($"🤖 Gemini Raw Response:\n{responseString}\n");

            var geminiResponse = JsonSerializer.Deserialize<JsonElement>(responseString);

            var responseText = geminiResponse
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            // ⬇️ DEBUG
            Console.WriteLine($"📝 Extracted Text:\n{responseText}\n");

            var cleanedJsonText = responseText!
                .Replace("```json", "")
                .Replace("```", "")
                .Trim();

            // ⬇️ DEBUG
            Console.WriteLine($"✅ Cleaned JSON:\n{cleanedJsonText}\n");

            var parsed = JsonSerializer.Deserialize<JsonElement>(cleanedJsonText);

            var intentName = parsed.GetProperty("intent").GetString() ?? "fallback_general";
            var parameters = JsonSerializer.Deserialize<Dictionary<string, object>>(
                parsed.GetProperty("parameters").ToString()
            ) ?? new Dictionary<string, object>();

            // ⬇️ DEBUG
            Console.WriteLine($"🎯 Intent: {intentName}");
            Console.WriteLine($"📦 Parameters: {JsonSerializer.Serialize(parameters)}\n");

            // ✅ تعويض التاريخ المفقود
            if (_intents.TryGetValue(intentName, out var intentTemplate))
            {
                if (intentTemplate.Parameters != null &&
                    intentTemplate.Parameters.Contains("date") &&
                    !parameters.ContainsKey("date"))
                {
                    parameters["date"] = currentDate;
                }

                // ✅ تعويض month/year المفقودين
                if (intentTemplate.Parameters.Contains("month") && !parameters.ContainsKey("month"))
                {
                    parameters["month"] = DateTime.Today.Month;
                }
                if (intentTemplate.Parameters.Contains("year") && !parameters.ContainsKey("year"))
                {
                    parameters["year"] = DateTime.Today.Year;
                }
            }

            // ✅ Parameter Conversion: month/year → startDate/endDate
            //if (intentName == "WorkOnHoliday_ByDate" &&
            //    parameters.ContainsKey("month") &&
            //    parameters.ContainsKey("year"))
            //{
            //    try
            //    {
            //        var month = Convert.ToInt32(parameters["month"]);
            //        var year = Convert.ToInt32(parameters["year"]);

            //        parameters["startDate"] = new DateTime(year, month, 1).ToString("yyyy-MM-dd");
            //        parameters["endDate"] = new DateTime(year, month, 1).AddMonths(1).ToString("yyyy-MM-dd");

            //        parameters.Remove("month");
            //        parameters.Remove("year");

            //        Console.WriteLine($"🔄 Converted to: startDate={parameters["startDate"]}, endDate={parameters["endDate"]}");
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"⚠️ Conversion error: {ex.Message}");
            //    }
            //}
            // بعد جلب Parameters من Gemini
            //if (intentName == "WorkOnHoliday")
            //{
            //    // لو Gemini بعت month/year، حولهم لـ startDate/endDate
            //    if (parameters.ContainsKey("month") && parameters.ContainsKey("year"))
            //    {
            //        var month = Convert.ToInt32(parameters["month"]);
            //        var year = Convert.ToInt32(parameters["year"]);

            //        parameters["startDate"] = new DateTime(year, month, 1).ToString("yyyy-MM-dd");
            //        parameters["endDate"] = new DateTime(year, month, 1).AddMonths(1).ToString("yyyy-MM-dd");

            //        parameters.Remove("month");
            //        parameters.Remove("year");
            //    }
            //}
            if (intentName == "WorkOnHoliday")
            {
                if (parameters.ContainsKey("month") && parameters.ContainsKey("year"))
                {
                    int month = 0, year = 0;

                    try
                    {
                        var monthElem = (JsonElement)parameters["month"];
                        var yearElem = (JsonElement)parameters["year"];

                        // استخراج month
                        if (monthElem.ValueKind == JsonValueKind.Number)
                            month = monthElem.GetInt32();
                        else
                            int.TryParse(monthElem.ToString(), out month);

                        // استخراج year
                        if (yearElem.ValueKind == JsonValueKind.Number)
                            year = yearElem.GetInt32();
                        else
                            int.TryParse(yearElem.ToString(), out year);

                        parameters["startDate"] = new DateTime(year, month, 1).ToString("yyyy-MM-dd");
                        parameters["endDate"] = new DateTime(year, month, 1).AddMonths(1).ToString("yyyy-MM-dd");

                        parameters.Remove("month");
                        parameters.Remove("year");

                        Console.WriteLine($"🔄 Converted month/year to: startDate={parameters["startDate"]}, endDate={parameters["endDate"]}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"⚠️ Month/year conversion error: {ex.Message}");
                    }
                }
            }

            var result = await _intentExecutor.ExecuteAsync(intentName, parameters);

            return new
            {
                Intent = intentName,
                Parameters = parameters,
                Answer = result
            };
        }

        // ========================================
        // 🔧 بناء Intent Details مع Examples
        // ========================================
        private string BuildIntentDetailsWithExamples()
        {
            var sb = new StringBuilder();

            foreach (var intent in _intents.Where(i => i.Key != "fallback_general"))
            {
                sb.AppendLine($"\n🔹 {intent.Key}:");
                sb.AppendLine($"   Description: {intent.Value.Description}");

                if (intent.Value.Examples != null && intent.Value.Examples.Any())
                {
                    sb.AppendLine($"   Examples:");
                    foreach (var example in intent.Value.Examples.Take(3))
                    {
                        sb.AppendLine($"   - \"{example}\"");
                    }
                }
            }

            return sb.ToString();
        }

        // ========================================
        // 🗂️ جلب Lookup Data من Database
        // ========================================
        private async Task<string> BuildLookupDataAsync()
        {
            var sb = new StringBuilder();

            try
            {
                // ✅ جلب الأقسام
                var departments = await _db.TbDepartments
                    .Select(d => new
                    {
                        d.DepartmentId,
                        NameAr = d.DepartmentName.ar ?? d.DepartmentCode,
                        NameEn = d.DepartmentName.en ?? d.DepartmentCode
                    })
                    .ToListAsync();

                if (departments.Any())
                {
                    sb.AppendLine("\n📂 Departments:");
                    foreach (var dept in departments)
                    {
                        sb.AppendLine($"   - ID: {dept.DepartmentId} → \"{dept.NameAr}\" / \"{dept.NameEn}\"");
                    }
                }

                // ✅ جلب أنواع الإجازات
                var vacationTypes = await _db.TbVacationTypes
                    .Select(v => new
                    {
                        v.VacationTypeId,
                        NameAr = v.VacationName.ar ?? "",
                        NameEn = v.VacationName.en ?? ""
                    })
                    .ToListAsync();

                if (vacationTypes.Any())
                {
                    sb.AppendLine("\n🏖️ Vacation Types:");
                    foreach (var vac in vacationTypes)
                    {
                        sb.AppendLine($"   - ID: {vac.VacationTypeId} → \"{vac.NameAr}\" / \"{vac.NameEn}\"");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Warning: Could not fetch lookup data: {ex.Message}");
                sb.AppendLine("⚠️ Lookup data unavailable");
            }

            return sb.ToString();
        }
    }

    // 📄 تعريف IntentGroupTemplate
    public class IntentGroupTemplate
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Intents { get; set; } = new();
    }
}