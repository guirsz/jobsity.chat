using Jobsity.Chat.Domain.Dtos.User;

namespace Jobsity.Chat.Domain.Interfaces.Services
{
    public interface IUserService
    {
        public Task<List<UserDto>> GetAvailableUsers();
    }
}
