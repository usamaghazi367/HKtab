using HKtab.Backend.DTOs;

namespace HKtab.Backend.Services;

public interface IAuthService
{
    Task<(bool Success, string Message, AuthResponse? Response)> RegisterAsync(RegisterRequest request);
    Task<(bool Success, string Message, AuthResponse? Response)> LoginAsync(LoginRequest request);
}
