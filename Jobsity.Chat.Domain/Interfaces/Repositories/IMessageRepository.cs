using Jobsity.Chat.Domain.Dtos.Messages;
using Jobsity.Chat.Domain.Entities;

namespace Jobsity.Chat.Domain.Interfaces.Repositories
{
    public interface IMessageRepository : IRepository<MessageEntity>
    {
        Task<MessageDto[]> SelectLast50Messages();
    }
}
