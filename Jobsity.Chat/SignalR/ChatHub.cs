using Jobsity.Chat.BotQueue;
using Jobsity.Chat.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Jobsity.Chat.SignalR
{
    [Authorize("Bearer")]
    public class ChatHub : Hub
    {
        private BotQueueOperations botQueueOperations;
        private readonly IMessageService messageService;

        public ChatHub(IMessageService messageService, BotQueueOperations botQueueOperations)
        {
            this.messageService = messageService;
            this.botQueueOperations = botQueueOperations;
        }

        public async Task PostMessage(string userName, string message)
        {
            var stockCommands = messageService.GetStockCommandsFromMessage(ref message);
            foreach (var command in stockCommands)
            {
                botQueueOperations.PushMessageToQueue(command);
            }
            if (!string.IsNullOrWhiteSpace(message))
            {
                if (botQueueOperations.IsThisUserTheBot(userName))
                {
                    userName = "Bot";
                }
                else
                {
                    var senderEmail = Context.User.Identities.FirstOrDefault().Name;
                    await messageService.RegisterMessage(senderEmail, message);
                }
                await Clients.All.SendAsync("messageReceived", userName, message);

            }
        }
    }
}
