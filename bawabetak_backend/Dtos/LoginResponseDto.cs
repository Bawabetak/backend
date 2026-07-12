namespace bawabetak_backend.Dtos
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; } 
        public string? RefreshToken { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsApproved { get; set; }
        public bool IsCompleteRegistration { get; set; }
    }
}
