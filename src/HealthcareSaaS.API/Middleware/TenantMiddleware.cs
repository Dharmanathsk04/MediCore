using System.Security.Claims;
using HealthcareSaaS.Application.Common.Interfaces;

namespace HealthcareSaaS.API.Middleware;

public class TenantMiddleware
{
    private readonly RequestDelegate _next;

    public TenantMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITenantService tenantService)
    {
        // Try to get tenant ID from JWT claims
        var tenantIdClaim = context.User.FindFirst("TenantId")?.Value;
        
        // If not in claims, try to get from header
        if (string.IsNullOrEmpty(tenantIdClaim))
        {
            tenantIdClaim = context.Request.Headers["X-Tenant-Id"].FirstOrDefault();
        }

        if (!string.IsNullOrEmpty(tenantIdClaim) && Guid.TryParse(tenantIdClaim, out var tenantId))
        {
            tenantService.SetTenantId(tenantId);
        }

        await _next(context);
    }
}
