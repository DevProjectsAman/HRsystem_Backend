using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using System.Text.Json;

namespace HRsystem.Api.Services.AuditLog
{
    public class AuditLogService
    {
       
        private readonly DBContextHRsystem _dbContext;

        public AuditLogService(DBContextHRsystem dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task LogActionAsync(
            int companyId,
            int userId,
            string tableName,
            string actionType,
            string recordId,
            object? oldData = null,
            object? newData = null)
        {
            var auditLog = new TbAuditLog
            {
                CompanyId = companyId,
                UserId = userId,
                ActionDatetime = DateTime.UtcNow,
                TableName = tableName,
                ActionType = actionType,
                RecordId = recordId,
                OldData = oldData != null ? JsonSerializer.Serialize(oldData) : null,
                NewData = newData != null ? JsonSerializer.Serialize(newData) : null
            };

            _dbContext.TbAuditLogs.Add(auditLog);
            await _dbContext.SaveChangesAsync();
        }
    }

}

