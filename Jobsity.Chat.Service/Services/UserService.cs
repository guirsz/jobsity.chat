using Jobsity.Chat.Domain.Dtos.User;
using Jobsity.Chat.Domain.Interfaces.Repositories;
using Jobsity.Chat.Domain.Interfaces.Services;

namespace Jobsity.Chat.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<List<UserDto>> GetAvailableUsers()
        {
            var users = await userRepository.SelectAsync();
            var result = users.Select(a => new UserDto() { Email = a.Email, Name = a.Name }).ToList();
            return result;
        }
    }
}
