namespace bawabetak_backend.Dtos.AuthDtos
{
    public class VerifyCodeDto
    {
        public string Email { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public VerficationType Type { get; set; }
    }
}
