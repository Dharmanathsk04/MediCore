using HealthcareSaaS.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthcareSaaS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public UsersController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? userType = null,
        [FromQuery] string? searchTerm = null)
    {
        try
        {
            var query = await _unitOfWork.Users.GetAllAsync();

            // Filter by user type if provided
            if (!string.IsNullOrEmpty(userType) && Enum.TryParse<Domain.Enums.UserType>(userType, true, out var type))
            {
                query = query.Where(u => u.UserType == type).ToList();
            }

            // Search by name or email
            if (!string.IsNullOrEmpty(searchTerm))
            {
                var search = searchTerm.ToLower();
                query = query.Where(u => 
                    u.Email.ToLower().Contains(search) ||
                    u.FirstName.ToLower().Contains(search) ||
                    u.LastName.ToLower().Contains(search)
                ).ToList();
            }

            // Filter out deleted users
            query = query.Where(u => !u.IsDeleted).ToList();

            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var users = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new
                {
                    u.Id,
                    u.Email,
                    u.FirstName,
                    u.LastName,
                    u.Phone,
                    UserType = u.UserType.ToString(),
                    u.IsActive,
                    u.LastLoginDate,
                    u.CreatedDate
                })
                .ToList();

            return Ok(new
            {
                data = users,
                pagination = new
                {
                    currentPage = page,
                    pageSize,
                    totalCount,
                    totalPages
                }
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        try
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);

            if (user == null || user.IsDeleted)
                return NotFound(new { message = "User not found" });

            return Ok(new
            {
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                user.Phone,
                UserType = user.UserType.ToString(),
                user.IsActive,
                user.LastLoginDate,
                user.CreatedDate,
                user.ModifiedDate
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetUserStats()
    {
        try
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            var activeUsers = users.Where(u => !u.IsDeleted && u.IsActive).ToList();

            var stats = new
            {
                totalUsers = users.Count(u => !u.IsDeleted),
                activeUsers = activeUsers.Count,
                inactiveUsers = users.Count(u => !u.IsDeleted && !u.IsActive),
                usersByType = new
                {
                    patients = activeUsers.Count(u => u.UserType == Domain.Enums.UserType.Patient),
                    doctors = activeUsers.Count(u => u.UserType == Domain.Enums.UserType.Doctor),
                    staff = activeUsers.Count(u => u.UserType == Domain.Enums.UserType.Staff),
                    admins = activeUsers.Count(u => u.UserType == Domain.Enums.UserType.Admin),
                    superAdmins = activeUsers.Count(u => u.UserType == Domain.Enums.UserType.SuperAdmin)
                },
                recentRegistrations = users
                    .Where(u => !u.IsDeleted)
                    .OrderByDescending(u => u.CreatedDate)
                    .Take(5)
                    .Select(u => new
                    {
                        u.Id,
                        u.Email,
                        u.FirstName,
                        u.LastName,
                        UserType = u.UserType.ToString(),
                        u.CreatedDate
                    })
                    .ToList()
            };

            return Ok(stats);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}/toggle-status")]
    public async Task<IActionResult> ToggleUserStatus(Guid id)
    {
        try
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);

            if (user == null || user.IsDeleted)
                return NotFound(new { message = "User not found" });

            user.IsActive = !user.IsActive;
            user.ModifiedDate = DateTime.UtcNow;

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return Ok(new
            {
                message = $"User {(user.IsActive ? "activated" : "deactivated")} successfully",
                isActive = user.IsActive
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        try
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);

            if (user == null || user.IsDeleted)
                return NotFound(new { message = "User not found" });

            // Soft delete
            user.IsDeleted = true;
            user.ModifiedDate = DateTime.UtcNow;

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return Ok(new { message = "User deleted successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
