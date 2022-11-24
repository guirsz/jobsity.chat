using System.ComponentModel.DataAnnotations.Schema;

namespace Jobsity.Chat.Domain.Entities
{
    [Table("Users")]
    public class UserEntity : BaseEntity
    {
        public UserEntity()
        {
            Id = Guid.NewGuid();
            Deactivated = false;
            CreateAt = DateTime.UtcNow;
            Email = string.Empty;
            Password = string.Empty;
        }

        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
    }
}
