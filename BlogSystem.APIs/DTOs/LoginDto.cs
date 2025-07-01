using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlogSystem.APIs.DTOs
{
    public class LoginDto
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
