using HealthcareSaaS.Domain.Common;
using HealthcareSaaS.Domain.Enums;

namespace HealthcareSaaS.Domain.Entities;

public class Appointment : BaseTenantEntity
{
    public Guid PatientId { get; set; }
    public Guid DoctorId { get; set; }
    public Guid TimeSlotId { get; set; }
    public AppointmentType AppointmentType { get; set; }
    public AppointmentStatus Status { get; set; }
    public string? Symptoms { get; set; }
    public string? Diagnosis { get; set; }
    public string? VisitAddress { get; set; } // For home visits
    public DateTime AppointmentDate { get; set; }
    public string? Notes { get; set; }
    public decimal? TotalFee { get; set; }

    // Navigation Properties
    public Patient Patient { get; set; } = null!;
    public Doctor Doctor { get; set; } = null!;
    public TimeSlot TimeSlot { get; set; } = null!;
    public Tenant Tenant { get; set; } = null!;
    public Prescription? Prescription { get; set; }
    public ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();
}
