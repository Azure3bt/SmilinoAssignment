using Domain.Dto;

namespace Aplication.Contract;

public interface IAuthService
{
    Task RegisterAsync(RegisterRequest registerRequest);
    Task LoginAsync(LoginRequest loginRequest);
    Task RefreshTokenAsync(string? refreshToken);
}
