using Jobsity.Chat.Domain.Entities;
using Jobsity.Chat.Domain.Interfaces.Repositories;
using Jobsity.Chat.Domain.Interfaces.Services;

namespace Jobsity.Chat.Service.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUserRepository userRepository;
        private readonly IMessageRepository messageRepository;

        public MessageService(IUserRepository userRepository, IMessageRepository messageRepository)
        {
            this.userRepository = userRepository;
            this.messageRepository = messageRepository;
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
    }
}
