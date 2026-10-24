using HealthcareSaaS.Domain.Common;
using HealthcareSaaS.Domain.Enums;

namespace HealthcareSaaS.Domain.Entities;

public class TimeSlot : BaseTenantEntity
{
    public Guid DoctorId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public bool IsBooked { get; set; }
    public bool IsAvailable { get; set; } = true;
    public AppointmentType SlotType { get; set; }

    // Navigation Properties
    public Doctor Doctor { get; set; } = null!;
    public Tenant Tenant { get; set; } = null!;
    public Appointment? Appointment { get; set; }
}
