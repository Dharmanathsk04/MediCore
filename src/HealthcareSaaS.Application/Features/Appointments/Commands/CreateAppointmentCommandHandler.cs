using HealthcareSaaS.Application.Common.Interfaces;
using HealthcareSaaS.Application.Common.Models;
using HealthcareSaaS.Application.Features.Appointments.DTOs;
using HealthcareSaaS.Domain.Entities;
using HealthcareSaaS.Domain.Enums;
using MediatR;

namespace HealthcareSaaS.Application.Features.Appointments.Commands;

public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Result<AppointmentDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITenantService _tenantService;

    public CreateAppointmentCommandHandler(IUnitOfWork unitOfWork, ITenantService tenantService)
    {
        _unitOfWork = unitOfWork;
        _tenantService = tenantService;
    }

    public async Task<Result<AppointmentDto>> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var dto = request.AppointmentDto;

        // Validate time slot
        var timeSlot = await _unitOfWork.TimeSlots.GetByIdAsync(dto.TimeSlotId, cancellationToken);
        if (timeSlot == null)
            return Result<AppointmentDto>.Failure("Time slot not found");

        if (timeSlot.IsBooked)
            return Result<AppointmentDto>.Failure("Time slot is already booked");

        if (!timeSlot.IsAvailable)
            return Result<AppointmentDto>.Failure("Time slot is not available");

        // Validate doctor
        var doctor = await _unitOfWork.Doctors.GetByIdAsync(dto.DoctorId, cancellationToken);
        if (doctor == null)
            return Result<AppointmentDto>.Failure("Doctor not found");

        // Get doctor-hospital association for fees
        var tenantId = _tenantService.GetTenantId();
        var doctorAssociation = await _unitOfWork.DoctorHospitalAssociations.FirstOrDefaultAsync(
            dha => dha.DoctorId == dto.DoctorId && dha.TenantId == tenantId && dha.IsActive,
            cancellationToken);
        
        if (doctorAssociation == null)
            return Result<AppointmentDto>.Failure("Doctor is not associated with this hospital");

        // Validate patient
        var patient = await _unitOfWork.Patients.GetByIdAsync(dto.PatientId, cancellationToken);
        if (patient == null)
            return Result<AppointmentDto>.Failure("Patient not found");

        // Calculate fee based on appointment type
        decimal totalFee;
        if (dto.AppointmentType == AppointmentType.HomeVisit)
        {
            if (!doctorAssociation.IsAvailableForHomeVisit || !doctorAssociation.HomeVisitFee.HasValue)
                return Result<AppointmentDto>.Failure("Doctor is not available for home visits at this hospital");
            totalFee = doctorAssociation.HomeVisitFee.Value;
        }
        else
        {
            totalFee = doctorAssociation.ConsultationFee;
        }

        // Create appointment
        var appointment = new Appointment
        {
            Id = Guid.NewGuid(),
            PatientId = dto.PatientId,
            DoctorId = dto.DoctorId,
            TimeSlotId = dto.TimeSlotId,
            TenantId = tenantId,
            AppointmentType = dto.AppointmentType,
            Status = AppointmentStatus.Scheduled,
            Symptoms = dto.Symptoms,
            VisitAddress = dto.VisitAddress,
            Notes = dto.Notes,
            AppointmentDate = timeSlot.Date,
            TotalFee = totalFee,
            CreatedDate = DateTime.UtcNow
        };

        await _unitOfWork.Appointments.AddAsync(appointment, cancellationToken);

        // Update time slot
        timeSlot.IsBooked = true;
        await _unitOfWork.TimeSlots.UpdateAsync(timeSlot, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var result = new AppointmentDto
        {
            Id = appointment.Id,
            PatientId = appointment.PatientId,
            DoctorId = appointment.DoctorId,
            TimeSlotId = appointment.TimeSlotId,
            AppointmentDate = appointment.AppointmentDate,
            StartTime = timeSlot.StartTime,
            EndTime = timeSlot.EndTime,
            AppointmentType = appointment.AppointmentType,
            Status = appointment.Status,
            Symptoms = appointment.Symptoms,
            VisitAddress = appointment.VisitAddress,
            TotalFee = appointment.TotalFee
        };

        return Result<AppointmentDto>.Success(result);
    }
}
