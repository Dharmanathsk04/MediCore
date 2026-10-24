using HealthcareSaaS.Domain.Common;
using HealthcareSaaS.Domain.Enums;

namespace HealthcareSaaS.Domain.Entities;

public class ServiceRequest : BaseTenantEntity
{
    public Guid AppointmentId { get; set; }
    public Guid PatientId { get; set; }
    public ServiceType ServiceType { get; set; }
    public Guid ServiceUnitId { get; set; }
    public ServiceRequestStatus Status { get; set; }
    public DateTime RequestDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public string? Instructions { get; set; }
    public string? Notes { get; set; }
    public decimal? TotalAmount { get; set; }

    // Navigation Properties
    public Appointment Appointment { get; set; } = null!;
    public Patient Patient { get; set; } = null!;
    public ServiceUnit ServiceUnit { get; set; } = null!;
    public Tenant Tenant { get; set; } = null!;
    public LabTest? LabTest { get; set; }
    public PharmacyOrder? PharmacyOrder { get; set; }
    public ImagingStudy? ImagingStudy { get; set; }
}
