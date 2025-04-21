using Aplication.Contract.Repository;
using Domain.Dto;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;
public class UserMessageRepository : IUserMessageRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public UserMessageRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task AddMessage(UserMessageRequest userMessage)
    {
        _applicationDbContext.UserMessages.Add(new UserMessage()
        {
            Message = userMessage.Message,
            UserId = userMessage.SenderId
        });
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<UserMessageResponse>> GetAllMessages()
    {
        return await _applicationDbContext.UserMessages.Include(m => m.User).AsNoTracking()
            .Select(x => new UserMessageResponse()
            {
                Message = x.Message,
                NickName = x.User.NickName
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<UserMessageResponse>> GetUserMessages(long userId)
    {
        return await _applicationDbContext.UserMessages.Include(m => m.User).AsNoTracking().Where(x => x.UserId == userId)
            .Select(x => new UserMessageResponse()
            {
                Message = x.Message,
                NickName = x.User.NickName
            })
            .ToListAsync();
    }
}