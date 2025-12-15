using AuditLog.Dtos;
using AuditLog.Interfaces;
using AuditLog.Models;
using System.Text.Json;

namespace AuditLog.Services
{
    public class AuditService : IAuditService
    {
        private readonly IAuditStore _store;
        private readonly IUserContext _userContext;

        public AuditService(IAuditStore store, IUserContext userContext)
        {
            _store = store;
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

                await _store.SaveLogAsync(entry);
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Audit Log Error: {0}", ex.Message));
            }
        }
    }
}
