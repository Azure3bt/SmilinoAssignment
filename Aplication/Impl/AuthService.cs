using Aplication.Contract;
using Aplication.Contract.Repository;
using Domain.Dto;
using Domain.Entities;
using Domain.Exception;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Aplication.Impl;

internal class AuthService : IAuthService
{
    private readonly JwtTokenProcessor _authTokenProcessor;
    private readonly UserManager<User> _userManager;
    private readonly IUserRepository _userRepository;

    public AuthService(JwtTokenProcessor authTokenProcessor, UserManager<User> userManager,
        IUserRepository userRepository)
    {
        _authTokenProcessor = authTokenProcessor;
        _userManager = userManager;
        _userRepository = userRepository;
    }

    public async Task RegisterAsync(RegisterRequest registerRequest)
    {
        var userExists = await _userManager.FindByEmailAsync(registerRequest.Email) != null;

        if (userExists)
        {
            throw new CustomException(HttpStatusCode.Conflict, $"{registerRequest.Email} is duplicate");
        }

        var user = User.CreateFromRequest(registerRequest);
        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, registerRequest.Password);

        var result = await _userManager.CreateAsync(user);

        if (!result.Succeeded)
        {
            throw new CustomException(HttpStatusCode.BadRequest, string.Join(Environment.NewLine, result.Errors.Select(x => x.Description)));
        }
    }

    public async Task LoginAsync(LoginRequest loginRequest)
    {
        var user = await _userManager.FindByEmailAsync(loginRequest.Email);

        if(user is null)
        {
            throw new CustomException(HttpStatusCode.NotFound, $"User with {loginRequest.Email} can't be find");
        }

        if (!await _userManager.CheckPasswordAsync(user, loginRequest.Password))
        {
            throw new CustomException(HttpStatusCode.Unauthorized, $"{loginRequest.Email} password incorrect");
        }

        await UpdateUserToken(user);
    }

    public async Task RefreshTokenAsync(string? refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new CustomException(HttpStatusCode.Unauthorized, "Refresh token is missing.");
        }

        var user = await _userRepository.GetUserByRefreshTokenAsync(refreshToken);

        if (user == null)
        {
            throw new CustomException(HttpStatusCode.Unauthorized, "Unable to retrieve user for refresh token");
        }

        if (user.RefreshTokenExpire < DateTime.UtcNow)
        {
            throw new CustomException(HttpStatusCode.Unauthorized, "Refresh token is expired.");
        }

        await UpdateUserToken(user);
    }

    private async Task UpdateUserToken(User? user)
    {
        var tokenResponse = _authTokenProcessor.GenerateJwtToken(user);
        var refreshTokenResponse = _authTokenProcessor.GenerateRefreshToken();

        user.RefreshToken = refreshTokenResponse.AccessToken;
        user.RefreshTokenExpire = refreshTokenResponse.Expiration;

        await _userManager.UpdateAsync(user);
    }
}
