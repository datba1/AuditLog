# AuditLog Library

A lightweight, cross-platform **Audit Logging** library for .NET applications.

---

## Installation

1. Add the `AuditLog` project to your solution.
2. Add a reference to this library in your main application project (API, WPF, WinForms, etc.).

---

## Core Concepts

To use this library, you need to understand three main interfaces:

1. **`IAuditService`**  
   The primary service you inject into controllers or services to write audit logs.

2. **`IUserContext`**  
   You must implement this interface to tell the library **who** the current user is.

3. **`IAuditStore`**  
   You implement this interface to define **where** audit logs are stored.  
   The library includes a built-in `FileAuditStore`.

---

## Usage Guide

### 1. Implement `IUserContext`

#### ASP.NET Core API Example

```csharp
public class HttpUserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpUserContext(IHttpContextAccessor accessor)
    {
        _httpContextAccessor = accessor;
    }

    public string GetUserId() =>
        _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;

    public string GetUserName() =>
        _httpContextAccessor.HttpContext?.User?.Identity?.Name;

    public string GetIpAddress() =>
        _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
}
```

#### WPF / WinForms Example

```csharp
public class DesktopUserContext : IUserContext
{
    public string GetUserId() => AppGlobal.CurrentUserId ?? "System";
    public string GetUserName() => AppGlobal.CurrentUserName ?? "System";
    public string GetIpAddress() => "Localhost";
}
```

---

### 2. Implement `IAuditStore` (Database)

```csharp
public class DbAuditStore : IAuditStore
{
    private readonly MyDbContext _db;

    public DbAuditStore(MyDbContext db)
    {
        _db = db;
    }

    public async Task SaveLogAsync(AuditEntry entry)
    {
        // _db.AuditLogs.Add(entry);
        // await _db.SaveChangesAsync();
    }
}
```

---

### 3. Register Services

```csharp
builder.Services.AddAuditLog();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContext, HttpUserContext>();

builder.Services.AddScoped<IAuditStore, DbAuditStore>();
builder.Services.AddSingleton<IAuditStore>(
    new FileAuditStore(Path.Combine(builder.Environment.ContentRootPath, "Logs")));
```

---

### 4. Writing Logs

```csharp
await _audit.LogAsync(
    action: "Update",
    entityName: "Product",
    entityKey: dto.Id.ToString(),
    oldObj: null,
    newObj: dto
);
```

---

## Project Structure

```
MyCompany.Common.AuditLog/
├── Extensions/
├── Interfaces/
├── Models/
├── Providers/
└── Services/
```

---

## License

MIT
