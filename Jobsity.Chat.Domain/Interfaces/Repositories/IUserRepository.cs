using Jobsity.Chat.Domain.Entities;

namespace Jobsity.Chat.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<UserEntity>
    {
        Task<UserEntity> FindByLogin(string email);
    }
}
