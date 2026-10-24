using HealthcareSaaS.Domain.Common;
using HealthcareSaaS.Domain.Enums;

namespace HealthcareSaaS.Domain.Entities;

public class Patient : BaseEntity
{
    public Guid UserId { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string? BloodGroup { get; set; }
    public string Address { get; set; } = string.Empty;
    public string? EmergencyContact { get; set; }
    public string? MedicalHistory { get; set; }
    public string? Allergies { get; set; }

    // Navigation Properties
    public User User { get; set; } = null!;
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    public ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();
}
