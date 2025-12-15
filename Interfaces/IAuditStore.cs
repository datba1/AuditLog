using AuditLog.Models;

namespace AuditLog.Interfaces
{
    public interface IAuditStore
    {
        Task SaveLogAsync(AuditEntry entry);
    }
}
