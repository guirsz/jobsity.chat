using Jobsity.Chat.Domain.Entities;
using Jobsity.Chat.Domain.Interfaces.Repositories;
using Jobsity.Chat.Service.Services;
using Moq;

namespace Jobsity.Chat.Test.xUnit.Services
{
    public class UnitTestUserService
    {
        [Fact]
        public async Task GetAvailableUsers()
        {
            var user1 = new UserEntity() { Id = Guid.NewGuid(), Name = "Name1", Email = "Email1" };
            var user2 = new UserEntity() { Id = Guid.NewGuid(), Name = "Name2", Email = "Email2" };
            List<UserEntity> usersInDataBase = new List<UserEntity>() { user1, user2 };
            Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(a => a.SelectAsync()).ReturnsAsync(() => usersInDataBase);

            var service = new UserService(mockUserRepository.Object);

            var result = await service.GetAvailableUsers();

            Assert.True(result != null && result.Count == 2);
        }

        [Fact]
        public async Task GetAvailableUsersWhenThereIsNoUser()
        {
            List<UserEntity> usersInDataBase = new List<UserEntity>();
            Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(a => a.SelectAsync()).ReturnsAsync(() => usersInDataBase);

            var service = new UserService(mockUserRepository.Object);

            var result = await service.GetAvailableUsers();

            Assert.True(result != null && result.Count == 0);
        }
    }
}
