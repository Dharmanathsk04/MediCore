using HealthcareSaaS.Domain.Common;

namespace HealthcareSaaS.Domain.Entities;

public class PharmacyOrderItem : BaseEntity
{
    public Guid PharmacyOrderId { get; set; }
    public string MedicineName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public string? BatchNumber { get; set; }
    public DateTime? ExpiryDate { get; set; }

    // Navigation Properties
    public PharmacyOrder PharmacyOrder { get; set; } = null!;
}
