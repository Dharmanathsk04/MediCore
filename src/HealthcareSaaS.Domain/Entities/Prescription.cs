using HealthcareSaaS.Domain.Common;

namespace HealthcareSaaS.Domain.Entities;

public class Prescription : BaseTenantEntity
{
    public Guid AppointmentId { get; set; }
    public Guid DoctorId { get; set; }
    public Guid PatientId { get; set; }
    public DateTime PrescriptionDate { get; set; }
    public string? Notes { get; set; }
    public string? DietaryInstructions { get; set; }
    public string? FollowUpInstructions { get; set; }
    public DateTime? NextVisitDate { get; set; }

    // Navigation Properties
    public Appointment Appointment { get; set; } = null!;
    public Doctor Doctor { get; set; } = null!;
    public Patient Patient { get; set; } = null!;
    public Tenant Tenant { get; set; } = null!;
    public ICollection<Medicine> Medicines { get; set; } = new List<Medicine>();
}
