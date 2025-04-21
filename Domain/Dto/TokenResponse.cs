namespace Domain.Dto;

public class TokenResponse
{
    public string AccessToken { get; set; }
    public DateTime Expiration { get; set; }
}
