using Aplication.Impl;
using Domain.Dto;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Utils;

public static class RegisterUtilsService
{
    public static IServiceCollection AddUtilsService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<JwtTokenProcessor>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddHttpContextAccessor();
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.JwtOptionsKey));

        return services;
    }
}
