using HealthcareSaaS.Application.Common.Models;
using HealthcareSaaS.Application.Features.Appointments.DTOs;
using MediatR;

namespace HealthcareSaaS.Application.Features.Appointments.Commands;

public class CreateAppointmentCommand : IRequest<Result<AppointmentDto>>
{
    public CreateAppointmentDto AppointmentDto { get; set; } = null!;
}
