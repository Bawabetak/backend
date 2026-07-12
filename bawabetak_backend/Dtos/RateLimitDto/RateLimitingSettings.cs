namespace bawabetak_backend.Dtos.RateLimitDto
{
    public class RateLimitingSettings
    {
        public RateLimitOptions Default { get; set; } = new();

        public Dictionary<string, RateLimitOptions> Endpoints { get; set; } = new();

        public RateLimitOptions GetPolicy(string path)
        {
            path = path.ToLower();

            return Endpoints.TryGetValue(path, out var policy)
                ? policy
                : Default;
        }
    }
}
