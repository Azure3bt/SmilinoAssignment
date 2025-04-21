using Aplication.Contract.Repository;
using Domain.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Persentation.Pages;
public class ChatModel : PageModel
{
    private readonly IUserMessageRepository _userMessageRepository;

    public ChatModel(IUserMessageRepository userMessageRepository)
    {
        _userMessageRepository = userMessageRepository;
    }

    public async Task OnGetAsync()
    {
        Messages = await _userMessageRepository.GetAllMessages();
    }

    [BindProperty]
    public IEnumerable<UserMessageResponse> Messages { get; set; }
}
