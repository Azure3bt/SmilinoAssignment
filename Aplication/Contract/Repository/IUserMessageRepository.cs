using Domain.Dto;

namespace Aplication.Contract.Repository;
public interface IUserMessageRepository
{
    Task AddMessage(UserMessageRequest userMessageRequest);
    Task<IEnumerable<UserMessageResponse>> GetAllMessages();
    Task<IEnumerable<UserMessageResponse>> GetUserMessages(long userId);
}
