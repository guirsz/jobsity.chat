using Jobsity.Chat.Domain.Entities;
using Jobsity.Chat.Domain.Interfaces.Repositories;
using Jobsity.Chat.Service.Services;
using Moq;

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

        [Theory]
        [InlineData("/stock=aapl.us")]
        [InlineData("Some information /stock=aapl.us and another one")]
        [InlineData("Some information /stock=SameCommand another one /stock=SameCommand")]
        [InlineData("Some information /stock=SameCommandNoCase another one /stock=SameCommandNOCASE")]
        public void RecognizeJustOneStockCommand(string message)
        {
            var service = new MessageService(mockUserRepository.Object, mockMessageRepository.Object);

            var result = service.GetStockCommandsFromMessage(ref message);

            Assert.True(result.Count == 1);
        }

        [Theory]
        [InlineData("Some information /stock=SameCommand another one /stock=AnotherCommand")]
        [InlineData("/stock=SameCommand /stock=AnotherCommand")]
        public void RecognizeMoreThanOneStockCommand(string message)
        {
            var service = new MessageService(mockUserRepository.Object, mockMessageRepository.Object);

            var result = service.GetStockCommandsFromMessage(ref message);

            Assert.True(result.Count > 1);
        }

        [Theory]
        [InlineData("Some message without stock command")]
        [InlineData("/stock= command")]
        [InlineData("/stock =command")]
        [InlineData("stock=command")]
        public void DoesNotRecognizeAnyStockCommand(string message)
        {
            var service = new MessageService(mockUserRepository.Object, mockMessageRepository.Object);

            var result = service.GetStockCommandsFromMessage(ref message);

            Assert.True(result.Count == 0);
        }

        [Theory]
        [InlineData("/stock=aapl.us")]
        [InlineData(" /stock=SameCommand     ")]
        public void ReturnEmptyMessageAfterGetStockCommand(string message)
        {
            var service = new MessageService(mockUserRepository.Object, mockMessageRepository.Object);

            var result = service.GetStockCommandsFromMessage(ref message);

            Assert.True(string.IsNullOrWhiteSpace(message));
        }

        [Theory]
        [InlineData("Some message without stock command")]
        [InlineData("Some message /stock=aapl.us")]
        [InlineData(" /stock=SameCommand    Some message  ")]
        public void ItReturnsTheMessageAfterGetStockCommand(string message)
        {
            var service = new MessageService(mockUserRepository.Object, mockMessageRepository.Object);

            var result = service.GetStockCommandsFromMessage(ref message);

            Assert.False(string.IsNullOrWhiteSpace(message));
        }
    }
}