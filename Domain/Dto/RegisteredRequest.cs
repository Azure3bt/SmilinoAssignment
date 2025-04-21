namespace Domain.Dto;

public class RegisterRequest
{
    public required string NickName { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}
