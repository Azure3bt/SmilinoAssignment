using Aplication.Contract;
using Aplication.Impl;
using Domain.Dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Aplication;

public static class RegisterApplication
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            var jwtOptions = configuration.GetSection(JwtOptions.JwtOptionsKey)
                .Get<JwtOptions>() ?? throw new ArgumentException(nameof(JwtOptions));
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret))
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    context.Token = context.Request.Cookies["ACCESS_TOKEN"];
                    return Task.CompletedTask;
                }
            };
        });

        services.AddAuthorization();

        return services;
    }
    
    public static void UseAuthApiEndpoints(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseEndpoints(endPointBuilder =>
        {
            endPointBuilder.MapPost("/api/account/register", async (RegisterRequest registerRequest, IAuthService accountService) =>
            {
                await accountService.RegisterAsync(registerRequest);

                return Results.Ok();
            });

            endPointBuilder.MapPost("/api/account/login", async (LoginRequest loginRequest, IAuthService accountService) =>
            {
                await accountService.LoginAsync(loginRequest);

                return Results.Ok();
            });

            endPointBuilder.MapPost("/api/account/refresh", async (HttpContext httpContext, IAuthService accountService) =>
            {
                var refreshToken = httpContext.Request.Cookies["REFRESH_TOKEN"];

                await accountService.RefreshTokenAsync(refreshToken);

                return Results.Ok();
            });
        });
    }
}
