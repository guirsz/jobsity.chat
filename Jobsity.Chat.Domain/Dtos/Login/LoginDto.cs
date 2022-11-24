using System.ComponentModel.DataAnnotations;

namespace Jobsity.Chat.Domain.Dtos.Login
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }
    }
}
