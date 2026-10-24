using HealthcareSaaS.Domain.Enums;

namespace HealthcareSaaS.Application.Features.TimeSlots.DTOs;

public class TimeSlotDto
{
    public Guid Id { get; set; }
    public Guid DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public bool IsBooked { get; set; }
    public bool IsAvailable { get; set; }
    public AppointmentType SlotType { get; set; }
}
