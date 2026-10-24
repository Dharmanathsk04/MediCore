namespace HealthcareSaaS.Application.Common.Interfaces;

public interface ITenantService
{
    Guid GetTenantId();
    void SetTenantId(Guid tenantId);
}
