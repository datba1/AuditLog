namespace AuditLog.Models
{
    public class AuditEntry
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public LogLevel Level { get; set; } = LogLevel.Info;
        public required string UserId { get; set; }
        public required string UserName { get; set; }
        public required string Action { get; set; }
        public required string EntityName { get; set; }
        public required string EntityKey { get; set; }
        public required string OldValues { get; set; }
        public required string NewValues { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string? IpAddress { get; set; }
    }
}
