
namespace bawabetak_backend.Dtos.AuthDtos
{
    public class VerifyCodeDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(6, MinimumLength = 6)]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Verification code must contain exactly 6 digits.")]
        public string Code { get; set; } = string.Empty;

        [Required]
        public VerficationType Type { get; set; }
    }
}