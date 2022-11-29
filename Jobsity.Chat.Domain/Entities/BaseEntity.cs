using System.ComponentModel.DataAnnotations;

namespace Jobsity.Chat.Domain.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public bool Deactivated { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }

    }
}
