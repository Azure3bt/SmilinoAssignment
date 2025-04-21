using Domain.Dto;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class User : IdentityUser<long>
{
    public string NickName { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpire { get; set; }
    public virtual ICollection<UserMessage> Messages { get; set; }

    public static User CreateFromRequest(RegisterRequest request)
    {
        return new User
        {
            NickName = request.NickName,
            Email = request.Email,
            UserName = request.Email,
            PasswordHash = request.Password,
        };
    }
}
