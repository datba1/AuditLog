using AuditLog.Models;

namespace AuditLog.Dtos
{
    public class LogDto
    {
        public required LogLevel Level;
        public required string Action;
        public required string EntityName;
        public required string EntityKey;
        public object? OldObj;
        public object? NewObj;
    }
}
