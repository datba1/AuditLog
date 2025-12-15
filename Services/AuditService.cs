using AuditLog.Dtos;
using AuditLog.Interfaces;
using AuditLog.Models;
using System.Text.Json;

namespace AuditLog.Services
{
    public class AuditService : IAuditService
    {
        private readonly IEnumerable<IAuditStore> _stores;
        private readonly IUserContext _userContext;

        public AuditService(IEnumerable<IAuditStore> stores, IUserContext userContext)
        {
            _stores = stores;
            _userContext = userContext;
        }

        public async Task LogAsync(LogDto logDto)
        {
            try
            {
                var entry = new AuditEntry
                {
                    UserId = _userContext.GetUserId() ?? "Anonymous",
                    UserName = _userContext.GetUserName(),
                    IpAddress = _userContext.GetIpAddress(),
                    Action = logDto.Action,
                    EntityName = logDto.EntityName,
                    EntityKey = logDto.EntityKey,
                    OldValues = JsonSerializer.Serialize(logDto.OldObj),
                    NewValues = JsonSerializer.Serialize(logDto.NewObj),
                };

                foreach (var store in _stores)
                {
                    await store.SaveLogAsync(entry);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Audit Log Error: {0}", ex.Message));
            }
        }
    }
}
