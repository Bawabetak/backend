
namespace bawabetak_backend.Dtos.AuthDtos
{
    public class SendVerificationDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public VerficationType Type { get; set; }
    }
}