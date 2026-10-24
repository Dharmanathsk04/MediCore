using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using HealthcareSaaS.Application.Common.Interfaces;
using HealthcareSaaS.Application.Features.Auth.DTOs;
using HealthcareSaaS.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HealthcareSaaS.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Users.FirstOrDefaultAsync(
            u => u.Email == loginDto.Email,
            cancellationToken);

        if (user == null || !VerifyPassword(loginDto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password");

        if (!user.IsActive)
            throw new UnauthorizedAccessException("User account is inactive");

        var token = GenerateJwtToken(user.Id, user.Email, user.UserType.ToString());
        var refreshToken = GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        user.LastLoginDate = DateTime.UtcNow;

        await _unitOfWork.Users.UpdateAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthResponseDto
        {
            Token = token,
            RefreshToken = refreshToken,
            Expiration = DateTime.UtcNow.AddHours(24),
            UserId = user.Id,
            Email = user.Email,
            UserType = user.UserType.ToString(),
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken = default)
    {
        var existingUser = await _unitOfWork.Users.FirstOrDefaultAsync(
            u => u.Email == registerDto.Email,
            cancellationToken);

        if (existingUser != null)
            throw new InvalidOperationException("User with this email already exists");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = registerDto.Email,
            PasswordHash = HashPassword(registerDto.Password),
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Phone = registerDto.Phone,
            UserType = registerDto.UserType,
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        };

        await _unitOfWork.Users.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var token = GenerateJwtToken(user.Id, user.Email, user.UserType.ToString());
        var refreshToken = GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await _unitOfWork.Users.UpdateAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthResponseDto
        {
            Token = token,
            RefreshToken = refreshToken,
            Expiration = DateTime.UtcNow.AddHours(24),
            UserId = user.Id,
            Email = user.Email,
            UserType = user.UserType.ToString(),
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Users.FirstOrDefaultAsync(
            u => u.RefreshToken == refreshToken,
            cancellationToken);

        if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            throw new UnauthorizedAccessException("Invalid or expired refresh token");

        var token = GenerateJwtToken(user.Id, user.Email, user.UserType.ToString());
        var newRefreshToken = GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await _unitOfWork.Users.UpdateAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthResponseDto
        {
            Token = token,
            RefreshToken = newRefreshToken,
            Expiration = DateTime.UtcNow.AddHours(24),
            UserId = user.Id,
            Email = user.Email,
            UserType = user.UserType.ToString(),
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }

    public async Task<bool> RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Users.FirstOrDefaultAsync(
            u => u.RefreshToken == refreshToken,
            cancellationToken);

        if (user == null)
            return false;

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;

        await _unitOfWork.Users.UpdateAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    public string GenerateJwtToken(Guid userId, string email, string userType)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "YourSuperSecretKeyHere123456789012"));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.Role, userType),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"] ?? "HealthcareSaaS",
            audience: _configuration["Jwt:Audience"] ?? "HealthcareSaaSClient",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private string HashPassword(string password)
    {
        // In production, use BCrypt or similar
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private bool VerifyPassword(string password, string hash)
    {
        var hashedPassword = HashPassword(password);
        return hashedPassword == hash;
    }
}
