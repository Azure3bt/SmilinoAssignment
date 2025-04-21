namespace Domain.Dto;

public class UserMessageRequest
{
    public long SenderId { get; set; }
    public string SenderNickName { get; set; }
    public string SenderEmail { get; set; }
    public string Message { get; set; }
    public DateTime SentAt { get; set; }
}
