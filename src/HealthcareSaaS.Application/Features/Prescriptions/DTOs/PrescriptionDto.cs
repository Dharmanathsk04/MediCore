namespace HealthcareSaaS.Application.Features.Prescriptions.DTOs;

public class PrescriptionDto
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public Guid DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public Guid PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public DateTime PrescriptionDate { get; set; }
    public string? Notes { get; set; }
    public string? DietaryInstructions { get; set; }
    public string? FollowUpInstructions { get; set; }
    public DateTime? NextVisitDate { get; set; }
    public List<MedicineDto> Medicines { get; set; } = new();
}

public class MedicineDto
{
    public Guid Id { get; set; }
    public string MedicineName { get; set; } = string.Empty;
    public string Dosage { get; set; } = string.Empty;
    public string Frequency { get; set; } = string.Empty;
    public int DurationDays { get; set; }
    public string? Instructions { get; set; }
    public int Quantity { get; set; }
}
