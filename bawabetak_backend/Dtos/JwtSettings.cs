namespace bawabetak_backend.Dtos
{
    public class JwtSettings
    {
        public String SecretKey { get; set; }
        public String Issuer { get; set; }
        public String Audience { get; set; }
        public int DurationInMinutes { get; set; }
    }
}
