using HealthcareSaaS.Domain.Enums;

namespace HealthcareSaaS.Application.Features.TimeSlots.DTOs;

public class CreateTimeSlotDto
{
    public Guid DoctorId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public AppointmentType SlotType { get; set; }
}
