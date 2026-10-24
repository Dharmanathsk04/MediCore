using HealthcareSaaS.Domain.Common;

namespace HealthcareSaaS.Domain.Entities;

public class PharmacyOrder : BaseEntity
{
    public Guid ServiceRequestId { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Pending";
    public string? DispensedBy { get; set; }
    public DateTime? DispensedDate { get; set; }
    public string? PaymentMethod { get; set; }
    public bool IsPaid { get; set; }

    // Navigation Properties
    public ServiceRequest ServiceRequest { get; set; } = null!;
    public ICollection<PharmacyOrderItem> OrderItems { get; set; } = new List<PharmacyOrderItem>();
}
