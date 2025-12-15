using AuditLog.Dtos;
using AuditLog.Interfaces;
using AuditLog.Models;

namespace AuditLog.Extensions
{
    public static class AuditServiceExtensions
    {
        public static Task LogInfoAsync(this IAuditService service, string action, string entityName, string entityKey, object? oldObj, object? newObj)
        {
            var dto = new LogDto
            {
                Level = LogLevel.Info,
                Action = action,
                EntityName = entityName,
                EntityKey = entityKey,
                OldObj = oldObj,
                NewObj = newObj
            };

            return service.LogAsync(dto);
        }

        public static Task LogErrorAsync(this IAuditService service, string action, string entityName, string entityKey, object? oldObj, object? newObj)
        {
            var dto = new LogDto
            {
                Level = LogLevel.Error,
                Action = action,
                EntityName = entityName,
                EntityKey = entityKey,
                OldObj = oldObj,
                NewObj = newObj
            };

            return service.LogAsync(dto);
        }

        public static Task LogWarnAsync(this IAuditService service, string action, string entityName, string entityKey, object? oldObj, object? newObj)
        {
            var dto = new LogDto
            {
                Level = LogLevel.Warn,
                Action = action,
                EntityName = entityName,
                EntityKey = entityKey,
                OldObj = oldObj,
                NewObj = newObj
            };

            return service.LogAsync(dto);
        }
    }
}
