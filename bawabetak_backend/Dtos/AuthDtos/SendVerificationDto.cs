namespace bawabetak_backend.Dtos.AuthDtos
{
    public class SendVerificationDto
    {
        public string Email { get; set; }
        public VerficationType Type { get; set; }
    }
}
