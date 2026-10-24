using HealthcareSaaS.Application.Common.Interfaces;
using HealthcareSaaS.Application.Common.Models;
using HealthcareSaaS.Application.Features.Appointments.DTOs;
using MediatR;

namespace HealthcareSaaS.Application.Features.Appointments.Queries;

public class GetAppointmentByIdQueryHandler : IRequestHandler<GetAppointmentByIdQuery, Result<AppointmentDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAppointmentByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AppointmentDto>> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
    {
        var appointment = await _unitOfWork.Appointments.GetByIdAsync(request.Id, cancellationToken);
        
        if (appointment == null)
            return Result<AppointmentDto>.Failure("Appointment not found");

        var timeSlot = await _unitOfWork.TimeSlots.GetByIdAsync(appointment.TimeSlotId, cancellationToken);
        var patient = await _unitOfWork.Patients.GetByIdAsync(appointment.PatientId, cancellationToken);
        var doctor = await _unitOfWork.Doctors.GetByIdAsync(appointment.DoctorId, cancellationToken);

        var result = new AppointmentDto
        {
            Id = appointment.Id,
            PatientId = appointment.PatientId,
            PatientName = patient != null ? $"{patient.User.FirstName} {patient.User.LastName}" : "",
            DoctorId = appointment.DoctorId,
            DoctorName = doctor != null ? $"{doctor.User.FirstName} {doctor.User.LastName}" : "",
            TimeSlotId = appointment.TimeSlotId,
            AppointmentDate = appointment.AppointmentDate,
            StartTime = timeSlot?.StartTime ?? TimeSpan.Zero,
            EndTime = timeSlot?.EndTime ?? TimeSpan.Zero,
            AppointmentType = appointment.AppointmentType,
            Status = appointment.Status,
            Symptoms = appointment.Symptoms,
            Diagnosis = appointment.Diagnosis,
            VisitAddress = appointment.VisitAddress,
            TotalFee = appointment.TotalFee
        };

        return Result<AppointmentDto>.Success(result);
    }
}
