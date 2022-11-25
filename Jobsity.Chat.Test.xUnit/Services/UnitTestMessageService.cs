using Jobsity.Chat.Domain.Entities;
using Jobsity.Chat.Domain.Interfaces.Repositories;
using Jobsity.Chat.Service.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobsity.Chat.Test.xUnit.Services
{
    public class UnitTestMessageService
    {
        private Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        private Mock<IUserRepository> mockUserRepositoryWithoutUsers = new Mock<IUserRepository>();
        private Mock<IMessageRepository> mockMessageRepository = new Mock<IMessageRepository>();

        public UnitTestMessageService()
        {
            mockUserRepositoryWithoutUsers.Setup(a => a.FindByLogin(It.IsAny<string>())).ReturnsAsync(() =>
                null);
            mockUserRepository.Setup(a => a.FindByLogin(It.IsAny<string>())).ReturnsAsync(() =>
                new UserEntity() { Name = "mock user", Email = "email@email.com" });
            mockMessageRepository.Setup(a => a.InsertAsync(It.IsAny<MessageEntity>())).ReturnsAsync(() => 
                new MessageEntity());
            
        }

        [Fact]
        public async Task RegisterMessageWithSuccess()
        {
            var service = new MessageService(mockUserRepository.Object, mockMessageRepository.Object);

            var result = await service.RegisterMessage("email@email.com", "some message");

            Assert.True(result);
        }

        [Fact]
        public async Task RegisterMessageWithoutMessage()
        {
            var service = new MessageService(mockUserRepository.Object, mockMessageRepository.Object);

            var result = await service.RegisterMessage("email@email.com", "");

            Assert.False(result);
        }

        [Fact]
        public async Task RegisterMessageWithNonExistentUser()
        {
            var service = new MessageService(mockUserRepositoryWithoutUsers.Object, mockMessageRepository.Object);

            var result = await service.RegisterMessage("non-existent-user@email.com", "some message");

            Assert.False(result);
        }
    }
}
