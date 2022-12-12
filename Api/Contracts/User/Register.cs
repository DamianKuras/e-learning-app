using System.ComponentModel.DataAnnotations;

namespace Api.Contracts.User
{
    public class Register
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
