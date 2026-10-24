using HealthcareSaaS.Domain.Common;

namespace HealthcareSaaS.Domain.Entities;

public class LabTest : BaseEntity
{
    public Guid ServiceRequestId { get; set; }
    public string TestName { get; set; } = string.Empty;
    public string TestType { get; set; } = string.Empty;
    public string? Results { get; set; }
    public string? PerformedBy { get; set; }
    public DateTime? PerformedDate { get; set; }
    public string? ReportUrl { get; set; }
    public string? ReferenceRange { get; set; }

    // Navigation Properties
    public ServiceRequest ServiceRequest { get; set; } = null!;
}
