
namespace bawabetak_backend.Dtos.AuthDtos
{
    public class RegisterDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public IFormFile Photo { get; set; } = null!;

        [Required]
        [StringLength(255)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [StringLength(14, MinimumLength = 14)]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "National Number must contain exactly 14 digits.")]
        public string NationalNumber { get; set; } = string.Empty;

        [Required]
        public IFormFile IdentityDocument { get; set; } = null!;
        [RegularExpression(@"^01[0125]\d{8}$",
            ErrorMessage = "Phone number must be a valid Egyptian mobile number.")]
        public string? PhoneNumber { get; set; }
    }
}