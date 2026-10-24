using HealthcareSaaS.Domain.Common;

namespace HealthcareSaaS.Domain.Entities;

public class Doctor : BaseEntity
{
    public Guid UserId { get; set; }
    public string Specialization { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public string Qualification { get; set; } = string.Empty;
    public int ExperienceYears { get; set; }
    public string? Bio { get; set; }

    // Navigation Properties
    public User User { get; set; } = null!;
    public ICollection<DoctorHospitalAssociation> HospitalAssociations { get; set; } = new List<DoctorHospitalAssociation>();
    public ICollection<TimeSlot> TimeSlots { get; set; } = new List<TimeSlot>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}
