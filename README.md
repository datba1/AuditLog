# AuditLog Library

A lightweight, cross-platform Audit Logging library for .NET. 

## Installation

1.  Add the `AuditLog` project to your solution.
2.  Add a reference to this library in your main application project (API, WPF, etc.).

## Core Concepts

To use this library, you need to understand three interfaces:

1.  **`IAuditService`**: The main service you inject into your controllers/classes to write logs.
2.  **`IUserContext`**: You must implement this to tell the library *how* to get the current user (e.g., from `HttpContext` or a static variable).
3.  **`IAuditStore`**: You implement this to tell the library *where* to save the log (Database, API, etc.). The library includes a built-in `FileAuditStore`.

## Usage Guide

### 1. Implement `IUserContext`

You need to tell the library who is currently logged in.

**For ASP.NET Core API:**
public class HttpUserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public HttpUserContext(IHttpContextAccessor accessor) => _httpContextAccessor = accessor;

    public string GetUserId() => _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;
    public string GetUserName() => _httpContextAccessor.HttpContext?.User?.Identity?.Name;
    public string GetIpAddress() => _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
}

**For WPF / WinForms:
public class DesktopUserContext : IUserContext
{
    public string GetUserId() => AppGlobal.CurrentUserId ?? "System"; 
    public string GetUserName() => AppGlobal.CurrentUserName ?? "System";
    public string GetIpAddress() => "Localhost";
}

### 2. Implement IAuditStore (Optional for DB)
If you want to save to a Database, implement the store in your main app (so it can access your DbContext).

public class DbAuditStore : IAuditStore
{
    private readonly MyDbContext _db;
    public DbAuditStore(MyDbContext db) => _db = db;

    public async Task SaveLogAsync(AuditEntry entry)
    {
        // Map common model to your DB Entity if necessary
        // Example: _db.AuditLogs.Add(entry);
        // await _db.SaveChangesAsync();
    }
}
Note: The library already contains FileAuditStore for logging to text files.

### 3. Registration (Program.cs / Startup.cs)
Register the services in your DI container. You can register multiple stores to write to multiple places at once.

using MyCompany.Common.AuditLog.Extensions;
using MyCompany.Common.AuditLog.Providers;

var builder = WebApplication.CreateBuilder(args);

// 1. Add the Core Audit Service
builder.Services.AddAuditLog();

// 2. Register your User Context
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContext, HttpUserContext>();

// 3. Register Storage Providers (You can register multiple!)

// A. Save to Database (Scoped is recommended for DB)
builder.Services.AddScoped<IAuditStore, DbAuditStore>();

// B. Save to File (Built-in provider)
// Registers FileAuditStore as a Singleton or Scoped implementation of IAuditStore
builder.Services.AddSingleton<IAuditStore>(provider => 
    new FileAuditStore(Path.Combine(builder.Environment.ContentRootPath, "Logs")));

### 4. Writing Logs
Inject IAuditService into any Controller or Service.


public class ProductController : ControllerBase
{
    private readonly IAuditService _audit;

    public ProductController(IAuditService audit)
    {
        _audit = audit;
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProduct(ProductDto dto)
    {
        // ... Your business logic ...
        
        // Log the action
        await _audit.LogAsync(
            action: "Update",
            entityName: "Product",
            entityKey: dto.Id.ToString(),
            oldObj: null, // or oldProduct
            newObj: dto
        );

        return Ok();
    }
}
### Project Structure

MyCompany.Common.AuditLog/
├── Extensions/
│   └── ServiceCollectionExtensions.cs  # DI Helper
├── Interfaces/
│   ├── IAuditService.cs
│   ├── IAuditStore.cs
│   └── IUserContext.cs
├── Models/
│   └── AuditEntry.cs                   # The Log Data Structure
├── Providers/
│   └── FileAuditStore.cs               # Built-in File Logger
└── Services/
    └── AuditService.cs                 # Main Logic