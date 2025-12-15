namespace AuditLog.Interfaces
{
    public interface IUserContext
    {
        string GetUserId();
        string GetUserName();
        string GetIpAddress();
    }
}
