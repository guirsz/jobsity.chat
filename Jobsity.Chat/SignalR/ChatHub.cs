using Jobsity.Chat.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Jobsity.Chat.SignalR
{
    [Authorize("Bearer")]
    public class ChatHub : Hub
    {
        private readonly IMessageService messageService;

        public ChatHub(IMessageService messageService)
        {
            this.messageService = messageService;
        }

        public async Task NewMessage(string userName, string message)
        {
            var senderEmail = this.Context.User.Identities.FirstOrDefault().Name;
            await messageService.RegisterMessage(senderEmail, message);
            await Clients.All.SendAsync("messageReceived", userName, message);
        }
    }
}
