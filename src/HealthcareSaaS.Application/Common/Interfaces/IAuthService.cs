using HealthcareSaaS.Application.Features.Auth.DTOs;

namespace HealthcareSaaS.Application.Common.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default);
    Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken = default);
    Task<AuthResponseDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task<bool> RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    string GenerateJwtToken(Guid userId, string email, string userType);
    string GenerateRefreshToken();
}
