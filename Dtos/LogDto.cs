namespace AuditLog.Dtos
{
    public class LogDto
    {
        public required string Action;
        public required string EntityName;
        public required string EntityKey;
        public required object OldObj;
        public required object NewObj;
    }
}
