namespace Jobsity.Chat.Domain.Interfaces.Services
{
    public interface IMessageService
    {
        List<string> GetStockCommandsFromMessage(ref string message);
        Task<bool> RegisterMessage(string senderEmail, string message);
    }
}
