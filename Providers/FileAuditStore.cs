using AuditLog.Interfaces;
using AuditLog.Models;
using System.Text.Json;

namespace AuditLog.Providers
{
    public class FileAuditStore : IAuditStore
    {
        private readonly string _logPath;

        public FileAuditStore(string logPath = "Logs")
        {
            _logPath = logPath;
            if (!Directory.Exists(_logPath))
            {
                Directory.CreateDirectory(_logPath);
            }
        }

        public async Task SaveLogAsync(AuditEntry entry)
        {
            try
            {
                string datePart = DateTime.UtcNow.ToString("yyyy-MM-dd");
                string fileName = $"audit-{datePart}.log";
                string fullPath = Path.Combine(_logPath, fileName);
                string jsonLog = JsonSerializer.Serialize(entry);

                await File.AppendAllTextAsync(fullPath, jsonLog + Environment.NewLine);
            }
            catch
            {
                // Swallow exceptions to avoid impacting main application flow
            }
        }
    }
}
