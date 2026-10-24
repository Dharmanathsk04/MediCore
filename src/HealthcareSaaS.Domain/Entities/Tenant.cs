using HealthcareSaaS.Domain.Common;

namespace HealthcareSaaS.Domain.Entities;

public class Tenant : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public string SubscriptionPlan { get; set; } = "Basic";
    public bool IsActive { get; set; } = true;
    public DateTime? SubscriptionStartDate { get; set; }
    public DateTime? SubscriptionEndDate { get; set; }

    // Navigation Properties
    public ICollection<DoctorHospitalAssociation> DoctorAssociations { get; set; } = new List<DoctorHospitalAssociation>();
    public ICollection<ServiceUnit> ServiceUnits { get; set; } = new List<ServiceUnit>();
    public ICollection<TimeSlot> TimeSlots { get; set; } = new List<TimeSlot>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<Staff> Staff { get; set; } = new List<Staff>();
    public ICollection<Admin> Admins { get; set; } = new List<Admin>();
}
