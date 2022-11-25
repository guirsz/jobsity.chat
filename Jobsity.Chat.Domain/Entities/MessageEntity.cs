using System.ComponentModel.DataAnnotations.Schema;

namespace Jobsity.Chat.Domain.Entities
{
    [Table("Messages")]
    public class MessageEntity : BaseEntity
    {
        public string Message { get; set; }
        public UserEntity User { get; set; }
    }
}
