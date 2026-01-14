////using HRsystem.Api.Database;
////using Microsoft.EntityFrameworkCore;
////using System.Text;
////using System.Text.Json;

////namespace HRsystem.Api.Services.Chatbot
////{
////    public class IntentExecutorService
////    {
////        private readonly DBContextHRsystem _db;
////        private readonly Dictionary<string, IntentTemplate> _intents;

////        public IntentExecutorService(DBContextHRsystem db)
////        {
////            _db = db;

////            var jsonPath = Path.Combine(AppContext.BaseDirectory, "Services", "Chatbot", "intent_templates.json");
////            if (!File.Exists(jsonPath))
////                throw new FileNotFoundException("intent_templates.json not found.", jsonPath);

////            var json = File.ReadAllText(jsonPath);
////            _intents = JsonSerializer.Deserialize<Dictionary<string, IntentTemplate>>(json)!;
////        }

////        public async Task<string> ExecuteAsync(string intentName, Dictionary<string, object> parameters)
////        {
////            // التعامل مع Fallback
////            if (intentName == "fallback_general")
////            {
////                return "❓ معذرة، لم أفهم سؤالك بشكل واضح.\n\n" +
////                       "يمكنك السؤال عن:\n" +
////                       "• الحضور والغياب اليوم\n" +
////                       "• المتأخرين هذا الشهر\n" +
////                       "• الإجازات المعتمدة\n" +
////                       "• المهمات والأعذار\n" +
////                       "• ترتيب الأقسام";
////            }

////            if (!_intents.ContainsKey(intentName))
////                return $"❌ لا يوجد استعلام معرف للـ intent '{intentName}'.";

////            var intent = _intents[intentName];

////            // التحقق من المعاملات المطلوبة
////            foreach (var param in intent.Parameters ?? new List<string>())
////            {
////                if (!parameters.ContainsKey(param))
////                    return $"⚠️ معلومات ناقصة: يجب توفير '{param}' لتنفيذ هذا الاستعلام.";
////            }

////            try
////            {
////                var resultList = new List<Dictionary<string, object>>();

////                using var connection = _db.Database.GetDbConnection();
////                await connection.OpenAsync();

////                using var command = connection.CreateCommand();
////                command.CommandText = intent.Sql;

////                // إضافة المعاملات
////                foreach (var param in intent.Parameters ?? new List<string>())
////                {
////                    var dbParameter = command.CreateParameter();
////                    dbParameter.ParameterName = $"@{param}";
////                    dbParameter.Value = parameters[param] ?? DBNull.Value;
////                    command.Parameters.Add(dbParameter);
////                }

////                // تنفيذ الاستعلام
////                using (var reader = await command.ExecuteReaderAsync())
////                {
////                    while (await reader.ReadAsync())
////                    {
////                        var row = new Dictionary<string, object>();
////                        for (int i = 0; i < reader.FieldCount; i++)
////                        {
////                            var value = reader.IsDBNull(i) ? null : reader.GetValue(i);
////                            row[reader.GetName(i)] = value;
////                        }
////                        resultList.Add(row);
////                    }
////                }

////                // تنسيق النتائج
////                return FormatResults(intentName, resultList);
////            }
////            catch (Exception ex)
////            {
////                return $"❌ حدث خطأ أثناء تنفيذ الاستعلام: {ex.Message}";
////            }
////        }

////        private string FormatResults(string intentName, List<Dictionary<string, object>> results)
////        {
////            if (results.Count == 0)
////            {
////                return intentName switch
////                {
////                    "absent_today_all" or "absent_today_by_department" =>
////                        "✅ لا يوجد موظفين غائبين اليوم. ممتاز!",

////                    "late_today_all" =>
////                        "✅ لا يوجد موظفين متأخرين اليوم. رائع!",

////                    "vacation_pending_count" =>
////                        "✅ لا توجد طلبات إجازة معلقة.",

////                    _ => "⚠️ لا توجد نتائج مطابقة للاستعلام."
////                };
////            }

////            // تنسيق خاص حسب نوع Intent
////            return intentName switch
////            {
////                "absent_today_all" => FormatAbsentEmployees(results),
////                "late_today_all" => FormatLateEmployees(results),
////                "most_late_employees_month" => FormatMostLateEmployees(results),
////                "vacation_approved_count_month" => FormatVacationCount(results),
////                "vacation_by_type_month" => FormatVacationsByType(results),
////                "missions_count_month" => FormatMissionsCount(results),
////                "excuses_stats_month" => FormatExcusesStats(results),
////                "department_attendance_rank_month" => FormatDepartmentRanking(results),
////                _ => FormatGenericResults(results)
////            };
////        }

////        private string FormatAbsentEmployees(List<Dictionary<string, object>> results)
////        {
////            var sb = new StringBuilder();
////            sb.AppendLine($"👥 الموظفون الغائبون اليوم ({results.Count} موظف):\n");
////            sb.AppendLine("═══════════════════════════════════");

////            foreach (var row in results)
////            {
////                var name = row.GetValueOrDefault("ArabicFullName")?.ToString()
////                        ?? row.GetValueOrDefault("EnglishFullName")?.ToString()
////                        ?? "غير معروف";
////                var dept = row.GetValueOrDefault("DepartmentName")?.ToString() ?? "غير محدد";

////                sb.AppendLine($"\n• {name}");
////                sb.AppendLine($"  📍 القسم: {dept}");
////            }

////            return sb.ToString();
////        }

////        private string FormatLateEmployees(List<Dictionary<string, object>> results)
////        {
////            var sb = new StringBuilder();
////            sb.AppendLine($"⏰ الموظفون المتأخرون اليوم ({results.Count} موظف):\n");
////            sb.AppendLine("═══════════════════════════════════");

////            foreach (var row in results)
////            {
////                var name = row.GetValueOrDefault("ArabicFullName")?.ToString() ?? "غير معروف";
////                var dept = row.GetValueOrDefault("DepartmentName")?.ToString() ?? "غير محدد";
////                var checkIn = row.GetValueOrDefault("CheckInTime")?.ToString() ?? "N/A";

////                sb.AppendLine($"\n• {name}");
////                sb.AppendLine($"  📍 القسم: {dept}");
////                sb.AppendLine($"  🕐 وقت الحضور: {checkIn}");
////            }

////            return sb.ToString();
////        }

////        private string FormatMostLateEmployees(List<Dictionary<string, object>> results)
////        {
////            var sb = new StringBuilder();
////            sb.AppendLine($"📊 أكثر الموظفين تأخيراً:\n");
////            sb.AppendLine("═══════════════════════════════════");

////            int rank = 1;
////            foreach (var row in results)
////            {
////                var name = row.GetValueOrDefault("ArabicFullName")?.ToString() ?? "غير معروف";
////                var dept = row.GetValueOrDefault("DepartmentName")?.ToString() ?? "غير محدد";
////                var count = row.GetValueOrDefault("LateCount")?.ToString() ?? "0";

////                var medal = rank switch { 1 => "🥇", 2 => "🥈", 3 => "🥉", _ => $"{rank}." };

////                sb.AppendLine($"\n{medal} {name}");
////                sb.AppendLine($"   📍 القسم: {dept}");
////                sb.AppendLine($"   🔢 عدد التأخيرات: {count} مرة");
////                rank++;
////            }

////            return sb.ToString();
////        }

////        private string FormatVacationCount(List<Dictionary<string, object>> results)
////        {
////            var count = results[0].GetValueOrDefault("ApprovedCount")?.ToString() ?? "0";
////            return $"🏖️ عدد الإجازات المعتمدة: {count} إجازة";
////        }

////        private string FormatVacationsByType(List<Dictionary<string, object>> results)
////        {
////            var sb = new StringBuilder();
////            sb.AppendLine("📋 الإجازات حسب النوع:\n");
////            sb.AppendLine("═══════════════════════════════════");

////            foreach (var row in results)
////            {
////                var type = row.GetValueOrDefault("VacationType")?.ToString() ?? "غير محدد";
////                var count = row.GetValueOrDefault("VacationCount")?.ToString() ?? "0";
////                var days = row.GetValueOrDefault("TotalDays")?.ToString() ?? "0";

////                sb.AppendLine($"\n• {type}");
////                sb.AppendLine($"  العدد: {count} إجازة");
////                sb.AppendLine($"  الأيام: {days} يوم");
////            }

////            return sb.ToString();
////        }

////        private string FormatMissionsCount(List<Dictionary<string, object>> results)
////        {
////            var total = results[0].GetValueOrDefault("MissionCount")?.ToString() ?? "0";
////            var approved = results[0].GetValueOrDefault("ApprovedCount")?.ToString() ?? "0";
////            var pending = results[0].GetValueOrDefault("PendingCount")?.ToString() ?? "0";

////            return $"🚀 إحصائيات المهمات:\n" +
////                   $"═══════════════════════════════════\n" +
////                   $"• الإجمالي: {total} مهمة\n" +
////                   $"• المعتمدة: {approved} مهمة\n" +
////                   $"• قيد المراجعة: {pending} مهمة";
////        }

////        private string FormatExcusesStats(List<Dictionary<string, object>> results)
////        {
////            var total = results[0].GetValueOrDefault("TotalExcuses")?.ToString() ?? "0";
////            var accepted = results[0].GetValueOrDefault("AcceptedCount")?.ToString() ?? "0";
////            var rate = results[0].GetValueOrDefault("AcceptanceRate")?.ToString() ?? "0";

////            return $"📝 إحصائيات الأعذار:\n" +
////                   $"═══════════════════════════════════\n" +
////                   $"• الإجمالي: {total} عذر\n" +
////                   $"• المقبول: {accepted} عذر\n" +
////                   $"• نسبة القبول: {rate}%";
////        }

////        private string FormatDepartmentRanking(List<Dictionary<string, object>> results)
////        {
////            var sb = new StringBuilder();
////            sb.AppendLine("🏆 ترتيب الأقسام حسب الالتزام:\n");
////            sb.AppendLine("═══════════════════════════════════");

////            int rank = 1;
////            foreach (var row in results)
////            {
////                var dept = row.GetValueOrDefault("DepartmentName")?.ToString() ?? "غير محدد";
////                var empCount = row.GetValueOrDefault("EmployeeCount")?.ToString() ?? "0";
////                var onTimePercent = row.GetValueOrDefault("OnTimePercentage")?.ToString() ?? "0";
////                var lateCount = row.GetValueOrDefault("LateCount")?.ToString() ?? "0";

////                var medal = rank switch { 1 => "🥇", 2 => "🥈", 3 => "🥉", _ => $"{rank}." };

////                sb.AppendLine($"\n{medal} {dept}");
////                sb.AppendLine($"   👥 عدد الموظفين: {empCount}");
////                sb.AppendLine($"   ✅ نسبة الالتزام: {onTimePercent}%");
////                sb.AppendLine($"   ⏰ التأخيرات: {lateCount}");
////                rank++;
////            }

////            return sb.ToString();
////        }

////        private string FormatGenericResults(List<Dictionary<string, object>> results)
////        {
////            // نتائج واحدة وقيمة واحدة = رقم بسيط
////            if (results.Count == 1 && results[0].Count == 1)
////            {
////                var value = results[0].Values.First()?.ToString() ?? "N/A";
////                return $"📊 النتيجة: {value}";
////            }

////            // عدة نتائج = جدول
////            var sb = new StringBuilder();
////            sb.AppendLine($"📊 النتائج ({results.Count} سجل):\n");
////            sb.AppendLine("═══════════════════════════════════");

////            foreach (var row in results.Take(10)) // أول 10 نتائج
////            {
////                sb.AppendLine();
////                foreach (var kvp in row)
////                {
////                    sb.AppendLine($"  {kvp.Key}: {kvp.Value ?? "N/A"}");
////                }
////            }

////            if (results.Count > 10)
////                sb.AppendLine($"\n... و {results.Count - 10} نتيجة أخرى");

////            return sb.ToString();
////        }
////    }
////}

//using HRsystem.Api.Database;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Data.SqlClient;
//using System.Data;
//using System.Text;
//using System.Text.Json;

//namespace HRsystem.Api.Services.Chatbot
//{
//    public class IntentExecutorService
//    {
//        private readonly DBContextHRsystem _db;
//        private readonly Dictionary<string, IntentTemplate> _intents;

//        public IntentExecutorService(DBContextHRsystem db)
//        {
//            _db = db;

//            var jsonPath = Path.Combine(AppContext.BaseDirectory, "Services", "Chatbot", "intent_templates.json");
//            if (!File.Exists(jsonPath))
//                throw new FileNotFoundException("intent_templates.json not found.", jsonPath);

//            var json = File.ReadAllText(jsonPath);
//            _intents = JsonSerializer.Deserialize<Dictionary<string, IntentTemplate>>(json)!;
//        }

//        public async Task<string> ExecuteAsync(string intentName, Dictionary<string, object> parameters)
//        {
//            // ✅ التعامل مع fallback intent
//            if (intentName == "fallback_general")
//            {
//                return "❓ لم أتمكن من فهم سؤالك بدقة.\n\n" +
//                       "جرب أسئلة مثل:\n" +
//                       "• الغياب اليوم أو هذا الشهر\n" +
//                       "• التأخير أو الحضور في الأقسام\n" +
//                       "• الإجازات المعتمدة أو المتبقية\n" +
//                       "• عدد المهمات أو الأعذار المقدمة\n" +
//                       "• الأقسام الأكثر التزامًا 👥";
//            }

//            if (!_intents.ContainsKey(intentName))
//                return $"❌ لا يوجد استعلام معرف للـ intent '{intentName}'.";

//            var intent = _intents[intentName];

//            // ✅ التحقق من المعاملات المطلوبة
//            foreach (var param in intent.Parameters ?? Enumerable.Empty<string>())
//            {
//                if (!parameters.ContainsKey(param))
//                    return $"⚠️ معلومات ناقصة: يجب توفير '{param}' لتنفيذ هذا الاستعلام.";
//            }

//            try
//            {
//                var resultList = new List<Dictionary<string, object>>();

//                var connection = _db.Database.GetDbConnection();
//                if (connection.State != ConnectionState.Open)
//                    await connection.OpenAsync();

//                using var command = connection.CreateCommand();
//                command.CommandText = intent.Sql;

//                // ✅ إضافة المعاملات للـ SQL
//                foreach (var param in intent.Parameters ?? Enumerable.Empty<string>())
//                {
//                    var dbParameter = command.CreateParameter();
//                    dbParameter.ParameterName = $"@{param}";
//                    dbParameter.Value = parameters[param] ?? DBNull.Value;
//                    command.Parameters.Add(dbParameter);
//                }

//                // ✅ تنفيذ الاستعلام
//                using (var reader = await command.ExecuteReaderAsync())
//                {
//                    while (await reader.ReadAsync())
//                    {
//                        var row = new Dictionary<string, object>();
//                        for (int i = 0; i < reader.FieldCount; i++)
//                        {
//                            var value = reader.IsDBNull(i) ? null : reader.GetValue(i);
//                            row[reader.GetName(i)] = value;
//                        }
//                        resultList.Add(row);
//                    }
//                }

//                // ✅ تنسيق النتيجة حسب نوع الـ Intent
//                return FormatResults(intentName, resultList);
//            }
//            catch (Exception ex)
//            {
//                return $"❌ حدث خطأ أثناء تنفيذ الاستعلام:\n{ex.Message}";
//            }
//        }

//        // ==============================
//        // 🔹 تنسيق النتائج حسب نوع السؤال
//        // ==============================

//        private string FormatResults(string intentName, List<Dictionary<string, object>> results)
//        {
//            if (results.Count == 0)
//            {
//                return intentName switch
//                {
//                    "absent_today_all" or "absent_today_by_department" =>
//                        "✅ لا يوجد موظفون غائبون اليوم 👏",
//                    "late_today_all" or "late_today_by_department" =>
//                        "✅ لا يوجد تأخيرات اليوم 🎉",
//                    "vacation_requests_this_month" =>
//                        "📅 لا توجد طلبات إجازة هذا الشهر.",
//                    _ => "⚠️ لا توجد نتائج مطابقة للاستعلام."
//                };
//            }

//            return intentName switch
//            {
//                "absent_today_all" => FormatAbsentEmployees(results),
//                "absent_today_by_department" => FormatAbsentEmployees(results),
//                "late_today_by_department" => FormatLateEmployees(results),
//                "absent_more_than_days_year" => FormatMostAbsentEmployees(results),
//                "vacation_requests_this_month" => FormatVacationCount(results),
//                "vacation_balance_by_department" => FormatVacationBalances(results),
//                "WorkOnHoliday" => FormatWorkOnHolidays(results),
//                "top_mission_request_by_department_month" => FormatTopMissions(results),
//                _ => FormatGenericResults(results)
//            };
//        }

//        // ==============================
//        // 🔸 دوال تنسيق حسب الـ Intent
//        // ==============================

//        private string FormatAbsentEmployees(List<Dictionary<string, object>> results)
//        {
//            var sb = new StringBuilder();
//            sb.AppendLine($"👥 الموظفون الغائبون ({results.Count}):\n");
//            foreach (var row in results)
//            {
//                var name = SafeToString(row.GetValueOrDefault("ArabicFullName"));
//                var dept = SafeToString(row.GetValueOrDefault("DepartmentName"));
//                sb.AppendLine($"• {name} ({dept})");
//            }
//            return sb.ToString();
//        }

//        private string FormatLateEmployees(List<Dictionary<string, object>> results)
//        {
//            var sb = new StringBuilder();
//            sb.AppendLine($"⏰ الموظفون المتأخرون اليوم ({results.Count}):\n");
//            foreach (var row in results)
//            {
//                var name = SafeToString(row.GetValueOrDefault("ArabicFullName"));
//                var dept = SafeToString(row.GetValueOrDefault("DepartmentName"));
//                var time = SafeToString(row.GetValueOrDefault("FirstPuchin"));
//                sb.AppendLine($"• {name} ({dept}) — 🕐 {time}");
//            }
//            return sb.ToString();
//        }

//        private string FormatMostAbsentEmployees(List<Dictionary<string, object>> results)
//        {
//            var sb = new StringBuilder();
//            sb.AppendLine($"📉 الموظفون الأكثر غيابًا:\n");
//            int rank = 1;
//            foreach (var row in results)
//            {
//                var name = SafeToString(row.GetValueOrDefault("ArabicFullName"));
//                var count = SafeToString(row.GetValueOrDefault("AbsentDays"));
//                var medal = rank switch { 1 => "🥇", 2 => "🥈", 3 => "🥉", _ => $"{rank}." };
//                sb.AppendLine($"{medal} {name} — {count} أيام غياب");
//                rank++;
//            }
//            return sb.ToString();
//        }

//        private string FormatVacationCount(List<Dictionary<string, object>> results)
//        {
//            var count = SafeToString(results[0].GetValueOrDefault("RequestsCount"));
//            return $"🏖️ عدد طلبات الإجازة هذا الشهر: {count}";
//        }

//        private string FormatVacationBalances(List<Dictionary<string, object>> results)
//        {
//            var sb = new StringBuilder();
//            sb.AppendLine("🏖️ رصيد الإجازات حسب القسم:\n");
//            foreach (var row in results)
//            {
//                var name = SafeToString(row.GetValueOrDefault("ArabicFullName"));
//                var total = SafeToString(row.GetValueOrDefault("TotalDays"));
//                var used = SafeToString(row.GetValueOrDefault("UsedDays"));
//                var remaining = SafeToString(row.GetValueOrDefault("RemainingDays"));
//                sb.AppendLine($"• {name}: {remaining}/{total} (مستخدم {used})");
//            }
//            return sb.ToString();
//        }

//        private string FormatWorkOnHolidays(List<Dictionary<string, object>> results)
//        {
//            var sb = new StringBuilder();
//            sb.AppendLine("🛠️ الموظفون الذين عملوا في عطلات رسمية:\n");
//            foreach (var row in results)
//            {
//                var name = SafeToString(row.GetValueOrDefault("ArabicFullName"));
//                var date = SafeToString(row.GetValueOrDefault("Date"));
//                sb.AppendLine($"• {name} — 📅 {date}");
//            }
//            return sb.ToString();
//        }

//        private string FormatTopMissions(List<Dictionary<string, object>> results)
//        {
//            var dept = SafeToString(results[0].GetValueOrDefault("DepartmentName"));
//            var count = SafeToString(results[0].GetValueOrDefault("MissionCount"));
//            return $"🚀 أكثر قسم طلب مهمات هذا الشهر هو: {dept} ({count} مهمة)";
//        }

//        private string FormatGenericResults(List<Dictionary<string, object>> results)
//        {
//            var sb = new StringBuilder();
//            sb.AppendLine($"📊 النتائج ({results.Count} سجل):\n");

//            foreach (var row in results.Take(10))
//            {
//                sb.AppendLine("────────────────────────────");
//                foreach (var kvp in row)
//                    sb.AppendLine($"  {kvp.Key}: {SafeToString(kvp.Value)}");
//            }

//            if (results.Count > 10)
//                sb.AppendLine($"\n... و {results.Count - 10} نتيجة أخرى");

//            return sb.ToString();
//        }

//        // 🧩 أداة تحويل آمنة
//        private static string SafeToString(object? value)
//            => value switch
//            {
//                null or DBNull => "N/A",
//                DateTime dt => dt.ToString("yyyy-MM-dd HH:mm"),
//                DateOnly d => d.ToString("yyyy-MM-dd"),
//                _ => value?.ToString() ?? "N/A"
//            };
//    }
//}