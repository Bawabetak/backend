
namespace bawabetak_backend.Dtos.AuthDtos
{
    public class LoginDto
    {

        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
