using HealthcareSaaS.Domain.Common;

namespace HealthcareSaaS.Domain.Entities;

public class ImagingStudy : BaseEntity
{
    public Guid ServiceRequestId { get; set; }
    public string StudyType { get; set; } = string.Empty; // X-Ray, CT Scan, MRI, etc.
    public string BodyPart { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? Report { get; set; }
    public string? PerformedBy { get; set; }
    public DateTime? PerformedDate { get; set; }
    public string? Findings { get; set; }
    public string? Recommendations { get; set; }

    // Navigation Properties
    public ServiceRequest ServiceRequest { get; set; } = null!;
}
