using HealthcareSaaS.Domain.Common;

namespace HealthcareSaaS.Domain.Entities;

public class DoctorHospitalAssociation : BaseTenantEntity
{
    public Guid DoctorId { get; set; }
    public decimal ConsultationFee { get; set; }
    public bool IsAvailableForHomeVisit { get; set; }
    public decimal? HomeVisitFee { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime JoinDate { get; set; }
    public DateTime? EndDate { get; set; }

    // Navigation Properties
    public Doctor Doctor { get; set; } = null!;
    public Tenant Tenant { get; set; } = null!;
}
