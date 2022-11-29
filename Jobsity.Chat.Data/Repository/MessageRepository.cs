using Jobsity.Chat.Data.Context;
using Jobsity.Chat.Domain.Dtos.Messages;
using Jobsity.Chat.Domain.Entities;
using Jobsity.Chat.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Jobsity.Chat.Data.Repository
{
    public class MessageRepository : BaseRepository<MessageEntity>, IMessageRepository
    {
        public MessageRepository(MyContext context) : base(context)
        {
            dataset = context.Set<MessageEntity>();
        }

        public async Task<MessageDto[]> SelectLast50Messages()
        {
            return await dataset.Where(a => a.Deactivated == false)
                                .OrderByDescending(a => a.CreateAt)
                                .Take(50)
                                .Select(a=> new MessageDto(a.CreateAt, a.User.Name, a.Message))
                                .ToArrayAsync();
        }
    }
}
