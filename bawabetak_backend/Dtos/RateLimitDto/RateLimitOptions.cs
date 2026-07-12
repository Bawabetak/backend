namespace bawabetak_backend.Dtos.RateLimitDto
{
    public class RateLimitOptions
    {
        public int MaxRequests { get; set; }

        public int WindowSeconds { get; set; }

        public int BlockMinutes { get; set; }
    }
}
