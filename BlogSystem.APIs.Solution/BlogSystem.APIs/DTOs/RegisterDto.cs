using System.ComponentModel.DataAnnotations;

namespace BlogSystem.APIs.DTOs
{
    public class RegisterDto
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required] 
        public string DisplayName { get; set; }

    }
}
