using Domain.Entities;

namespace Aplication.Contract.Repository;

public interface IUserRepository
{
    Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
}
