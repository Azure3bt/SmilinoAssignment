namespace Domain.Entities;

public class UserMessage
{
    public int MessageId { get; set; }
    public string Message { get; set; }

    public long UserId { get; set; }
    public User User { get; set; }
}
