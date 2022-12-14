using System.ComponentModel.DataAnnotations;

namespace Api.Contracts.User
{
    public class Login
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
