using Jobsity.Chat.Domain.Dtos.Messages;
using Jobsity.Chat.Domain.Entities;
using Jobsity.Chat.Domain.Interfaces.Repositories;
using Jobsity.Chat.Domain.Interfaces.Services;

namespace Jobsity.Chat.Service.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUserRepository userRepository;
        private readonly IMessageRepository messageRepository;
        private const string StockCommand = "/stock=";

        public MessageService(IUserRepository userRepository, IMessageRepository messageRepository)
        {
            this.userRepository = userRepository;
            this.messageRepository = messageRepository;
        }

        public List<string> GetStockCommandsFromMessage(ref string message)
        {
            List<string> results = new List<string>();
            var indexOfCommand = message.IndexOf(StockCommand);
            while (indexOfCommand != -1) 
            {
                var command = string.Empty;
                for (int i = indexOfCommand + StockCommand.Length; i < message.Length; i++)
                {
                    if (message[i] == ' ') break;
                    command = $"{command}{message[i]}";
                }
                if (command != string.Empty)
                {
                    results.Add(command);
                }
                message = message.Replace($"{StockCommand}{command}", string.Empty, true, null);
                indexOfCommand = message.IndexOf(StockCommand);
            }
            return results;
        }

        public async Task<bool> RegisterMessage(string senderEmail, string message)
        {
            if (string.IsNullOrEmpty(message)) return false;

            var user = await userRepository.FindByLogin(senderEmail);
            if (user == null) return false;

            var result = await messageRepository.InsertAsync(new MessageEntity()
            {
                User = user,
                Message = message
            });

            return result != null;
        }

        public async Task<MessageDto[]> Getlast50Messages()
        {
            var result = await messageRepository.SelectLast50Messages();
            return result.OrderBy(a => a.Timestamp).ToArray();
        }
    }
}
