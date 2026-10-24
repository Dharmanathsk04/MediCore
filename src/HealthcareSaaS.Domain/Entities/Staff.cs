using HealthcareSaaS.Domain.Common;

namespace HealthcareSaaS.Domain.Entities;

public class Staff : BaseTenantEntity
{
    public Guid UserId { get; set; }
    public string Department { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime JoinDate { get; set; }
    public Guid? ServiceUnitId { get; set; }

    // Navigation Properties
    public User User { get; set; } = null!;
    public Tenant Tenant { get; set; } = null!;
    public ServiceUnit? ServiceUnit { get; set; }
}
