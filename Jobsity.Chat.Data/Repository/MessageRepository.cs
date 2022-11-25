using Jobsity.Chat.Data.Context;
using Jobsity.Chat.Domain.Entities;
using Jobsity.Chat.Domain.Interfaces.Repositories;

namespace Jobsity.Chat.Data.Repository
{
    public class MessageRepository : BaseRepository<MessageEntity>, IMessageRepository
    {
        public MessageRepository(MyContext context) : base(context)
        {
            dataset = context.Set<MessageEntity>();
        }
    }
}
