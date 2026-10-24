using HealthcareSaaS.Domain.Enums;

namespace HealthcareSaaS.Application.Features.Appointments.DTOs;

public class CreateAppointmentDto
{
    public Guid PatientId { get; set; }
    public Guid DoctorId { get; set; }
    public Guid TimeSlotId { get; set; }
    public AppointmentType AppointmentType { get; set; }
    public string? Symptoms { get; set; }
    public string? VisitAddress { get; set; }
    public string? Notes { get; set; }
}
