using Jobsity.Chat.Domain.Dtos.Login;

namespace Jobsity.Chat.Domain.Interfaces.Services
{
    public interface ILoginService
    {
        Task<object> Authenticate(LoginDto user);
    }
}
