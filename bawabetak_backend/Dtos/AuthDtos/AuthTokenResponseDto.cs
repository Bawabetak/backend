namespace bawabetak_backend.Dtos.AuthDtos
{
    public class AuthTokenResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string? RefreshToken { get; set; } 
         
    }
}
