using HealthcareSaaS.Application.Common.Interfaces;
using HealthcareSaaS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareSaaS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TenantsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public TenantsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var tenants = await _unitOfWork.Tenants.GetAllAsync();
            return Ok(tenants);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var tenant = await _unitOfWork.Tenants.GetByIdAsync(id);
            if (tenant == null)
                return NotFound(new { message = "Tenant not found" });

            return Ok(tenant);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTenantDto dto)
    {
        try
        {
            var tenant = new Tenant
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Address = dto.Address,
                ContactEmail = dto.ContactEmail,
                ContactPhone = dto.ContactPhone,
                SubscriptionPlan = dto.SubscriptionPlan ?? "Basic",
                IsActive = true,
                SubscriptionStartDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow
            };

            await _unitOfWork.Tenants.AddAsync(tenant);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = tenant.Id }, tenant);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTenantDto dto)
    {
        try
        {
            var tenant = await _unitOfWork.Tenants.GetByIdAsync(id);
            if (tenant == null)
                return NotFound(new { message = "Tenant not found" });

            tenant.Name = dto.Name ?? tenant.Name;
            tenant.Address = dto.Address ?? tenant.Address;
            tenant.ContactEmail = dto.ContactEmail ?? tenant.ContactEmail;
            tenant.ContactPhone = dto.ContactPhone ?? tenant.ContactPhone;
            tenant.SubscriptionPlan = dto.SubscriptionPlan ?? tenant.SubscriptionPlan;
            tenant.IsActive = dto.IsActive ?? tenant.IsActive;
            tenant.ModifiedDate = DateTime.UtcNow;

            await _unitOfWork.Tenants.UpdateAsync(tenant);
            await _unitOfWork.SaveChangesAsync();

            return Ok(tenant);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var tenant = await _unitOfWork.Tenants.GetByIdAsync(id);
            if (tenant == null)
                return NotFound(new { message = "Tenant not found" });

            await _unitOfWork.Tenants.DeleteAsync(tenant);
            await _unitOfWork.SaveChangesAsync();

            return Ok(new { message = "Tenant deleted successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}

public class CreateTenantDto
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public string? SubscriptionPlan { get; set; }
}

public class UpdateTenantDto
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? ContactEmail { get; set; }
    public string? ContactPhone { get; set; }
    public string? SubscriptionPlan { get; set; }
    public bool? IsActive { get; set; }
}
