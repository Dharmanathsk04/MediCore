using HealthcareSaaS.Application.Common.Models;
using HealthcareSaaS.Application.Features.Appointments.DTOs;
using MediatR;

namespace HealthcareSaaS.Application.Features.Appointments.Queries;

public class GetAppointmentByIdQuery : IRequest<Result<AppointmentDto>>
{
    public Guid Id { get; set; }
}
