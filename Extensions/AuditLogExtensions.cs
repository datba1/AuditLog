using AuditLog.Interfaces;
using AuditLog.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AuditLog.Extensions
{
    public static class AuditLogExtensions
    {
        public static IServiceCollection AddAuditLog(this IServiceCollection services)
        {
            services.AddScoped<IAuditService, AuditService>();
            return services;
        }
    }
}
