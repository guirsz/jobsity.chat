namespace Jobsity.Chat.Domain.Security
{
    public class QueueConfigurations
    {
        public string HostName { get; set; }
        public string SubmissionQueueName { get; set; }
        public string ReceivingQueueName { get; set; }
        public string UninterpretedQueueName { get; set; }
    }
}
