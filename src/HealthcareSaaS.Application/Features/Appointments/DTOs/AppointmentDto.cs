using HealthcareSaaS.Domain.Enums;

namespace HealthcareSaaS.Application.Features.Appointments.DTOs;

public class AppointmentDto
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public Guid DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public Guid TimeSlotId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public AppointmentType AppointmentType { get; set; }
    public AppointmentStatus Status { get; set; }
    public string? Symptoms { get; set; }
    public string? Diagnosis { get; set; }
    public string? VisitAddress { get; set; }
    public decimal? TotalFee { get; set; }
}
