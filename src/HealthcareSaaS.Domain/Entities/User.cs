using HealthcareSaaS.Domain.Common;
using HealthcareSaaS.Domain.Enums;

namespace HealthcareSaaS.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public UserType UserType { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? LastLoginDate { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    // Navigation Properties
    public Patient? Patient { get; set; }
    public Doctor? Doctor { get; set; }
    public ICollection<Staff> StaffRoles { get; set; } = new List<Staff>();
    public ICollection<Admin> AdminRoles { get; set; } = new List<Admin>();
}
