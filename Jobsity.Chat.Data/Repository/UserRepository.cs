using Jobsity.Chat.Data.Context;
using Jobsity.Chat.Domain.Entities;
using Jobsity.Chat.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Jobsity.Chat.Data.Repository
{
    public class UserRepository : BaseRepository<UserEntity>, IUserRepository
    {
        public UserRepository(MyContext context) : base(context)
        {
            dataset = context.Set<UserEntity>();
        }

        public async Task<UserEntity> FindByLogin(string email)
        {
            return await dataset.FirstOrDefaultAsync(x => x.Email.Equals(email));
        }
    }
}
