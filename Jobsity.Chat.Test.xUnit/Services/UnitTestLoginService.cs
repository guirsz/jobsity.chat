using Jobsity.Chat.Domain.Entities;
using Jobsity.Chat.Domain.Helpers;
using Jobsity.Chat.Domain.Interfaces.Repositories;
using Jobsity.Chat.Domain.Security;
using Jobsity.Chat.Service.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Jobsity.Chat.Test.xUnit.Services
{
    public class UnitTestLoginService
    {
        private Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        private Mock<IConfiguration> mockConfiguration = new Mock<IConfiguration>();
        private Mock<SigningConfigurations> mockSigningConfigurations = new Mock<SigningConfigurations>();
        private Mock<TokenConfigurations> mockTokenConfigurations = new Mock<TokenConfigurations>();

        private readonly string mockEmail = "mock@email.com";
        private readonly string mockHashPassword = "mock".HashPassword();
        private readonly string mockPassword = "mock";
        private readonly string mockWrongPassword = "wrong";

        public UnitTestLoginService()
        {
            mockUserRepository.Setup(a => a.FindByLogin(It.IsAny<string>())).ReturnsAsync(() =>
                new UserEntity() { Name = "mock user", Email = mockEmail, Password = mockHashPassword });
            mockTokenConfigurations.Object.Seconds = 10000;
        }

        [Fact]
        public async Task AuthenticateSuccess()
        {
            var service = new LoginService(mockUserRepository.Object, mockSigningConfigurations.Object, mockTokenConfigurations.Object, mockConfiguration.Object);

            object result = await service.Authenticate(new Domain.Dtos.Login.LoginDto()
            {
                Email = mockEmail,
                Password = mockPassword
            });
            
            System.Reflection.PropertyInfo prop = result.GetType()?.GetProperty("authenticated");
            var value = (bool)(prop.GetValue(result) ?? false);

            Assert.True(value);
        }

        [Fact]
        public async Task AuthenticateFailed()
        {
            var service = new LoginService(mockUserRepository.Object, mockSigningConfigurations.Object, mockTokenConfigurations.Object, mockConfiguration.Object);

            dynamic result = await service.Authenticate(new Domain.Dtos.Login.LoginDto()
            {
                Email = mockEmail,
                Password = mockWrongPassword
            });

            System.Reflection.PropertyInfo prop = result.GetType()?.GetProperty("authenticated");
            var value = (bool)(prop.GetValue(result) ?? false);

            Assert.False(value);
        }
    }
}
