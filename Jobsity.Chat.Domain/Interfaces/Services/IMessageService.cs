using Jobsity.Chat.Domain.Dtos.Messages;

namespace Jobsity.Chat.Domain.Interfaces.Services
{
    public interface IMessageService
    {
        Task<MessageDto[]> Getlast50Messages();
        List<string> GetStockCommandsFromMessage(ref string message);
        Task<bool> RegisterMessage(string senderEmail, string message);
    }
}
