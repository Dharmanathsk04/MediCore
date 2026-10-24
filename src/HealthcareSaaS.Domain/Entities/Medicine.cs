using HealthcareSaaS.Domain.Common;

namespace HealthcareSaaS.Domain.Entities;

public class Medicine : BaseEntity
{
    public Guid PrescriptionId { get; set; }
    public string MedicineName { get; set; } = string.Empty;
    public string Dosage { get; set; } = string.Empty;
    public string Frequency { get; set; } = string.Empty;
    public int DurationDays { get; set; }
    public string? Instructions { get; set; }
    public int Quantity { get; set; }

    // Navigation Properties
    public Prescription Prescription { get; set; } = null!;
}
