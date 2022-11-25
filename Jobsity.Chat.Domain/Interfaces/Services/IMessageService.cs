namespace Jobsity.Chat.Domain.Interfaces.Services
{
    public interface IMessageService
    {
        Task<bool> RegisterMessage(string senderEmail, string message);
    }
}
