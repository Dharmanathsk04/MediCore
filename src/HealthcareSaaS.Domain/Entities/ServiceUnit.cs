using HealthcareSaaS.Domain.Common;
using HealthcareSaaS.Domain.Enums;

namespace HealthcareSaaS.Domain.Entities;

public class ServiceUnit : BaseTenantEntity
{
    public ServiceType UnitType { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public string? OperatingHours { get; set; }

    // Navigation Properties
    public Tenant Tenant { get; set; } = null!;
    public ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();
    public ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
