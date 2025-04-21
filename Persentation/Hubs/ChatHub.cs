using Aplication.Contract.Repository;
using Domain.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Persentation.Hubs;
[Authorize]
public class ChatHub : Hub
{
    private readonly IUserMessageRepository _userMessageRepository;

    public ChatHub(IUserMessageRepository userMessageRepository)
    {
        _userMessageRepository = userMessageRepository;
    }

    public override async Task OnConnectedAsync()
    {
        await Clients.Caller.SendAsync("ReceiveMessage", "System", $"Welcome {GetUsername()}, you are successfully connected to the chat!");
        await base.OnConnectedAsync();
    }

    public async Task SendMessage(string message)
    {
        var user = Context.User;
        await _userMessageRepository.AddMessage(new UserMessageRequest()
        {
            Message = message,
            SenderId = long.Parse(Context.UserIdentifier ?? "")
        });

        await Clients.All.SendAsync("ReceiveMessage", GetUsername(), message);
    }

    private string GetUsername()
    {
        return Context.User?.Claims.FirstOrDefault(c => c.Type == "name")?.Value ?? "Unknown User";
    }
}