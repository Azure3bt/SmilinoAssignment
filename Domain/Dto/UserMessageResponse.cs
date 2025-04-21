namespace Domain.Dto;

public class UserMessageResponse
{
    public string NickName { get; set; }
    public string Email { get; set; }
    public string Message { get; set; }
    public DateTime SentAt { get; set; }
}
