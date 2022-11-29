namespace Jobsity.Chat.Domain.Dtos.Messages
{
    public class MessageDto
    {
        public MessageDto(DateTime timestamp, string userName, string message)
        {
            Timestamp = timestamp;
            UserName = userName;
            Message = message;
        }

        public DateTime Timestamp { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
    }
}
