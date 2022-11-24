using System.ComponentModel.DataAnnotations;

namespace Jobsity.Chat.Domain.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public bool Deactivated { get; set; }
        private DateTime? _createAt;
        public DateTime? CreateAt
        {
            get { return _createAt; }
            set { _createAt = value == null ? DateTime.UtcNow : value; }
        }
        public DateTime? UpdateAt { get; set; }

    }
}
