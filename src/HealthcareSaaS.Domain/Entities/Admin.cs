using HealthcareSaaS.Domain.Common;

namespace HealthcareSaaS.Domain.Entities;

public class Admin : BaseTenantEntity
{
    public Guid UserId { get; set; }
    public string AccessLevel { get; set; } = "Standard";
    public string Permissions { get; set; } = string.Empty; // JSON string of permissions

    // Navigation Properties
    public User User { get; set; } = null!;
    public Tenant Tenant { get; set; } = null!;
}
