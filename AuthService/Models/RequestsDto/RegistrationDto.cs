using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.RequestsDto
{
    public class RegistrationDto
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        public string? Role { get; set; } = string.Empty;
    }
}
