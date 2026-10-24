namespace HealthcareSaaS.Application.Features.Doctors.DTOs;

public class DoctorDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public string Qualification { get; set; } = string.Empty;
    public int ExperienceYears { get; set; }
    public decimal ConsultationFee { get; set; }
    public bool IsAvailableForHomeVisit { get; set; }
    public decimal? HomeVisitFee { get; set; }
    public string? Bio { get; set; }
}
