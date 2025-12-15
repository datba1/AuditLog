using AuditLog.Dtos;

namespace AuditLog.Interfaces
{
    public interface IAuditService
    {
        Task LogAsync(LogDto logDto);
    }
}
