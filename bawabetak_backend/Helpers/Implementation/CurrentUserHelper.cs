namespace bawabetak_backend.Helpers.Implementation
{
    public class CurrentUserHelper : ICurrentUserHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUserId()
        {
            var id = _httpContextAccessor.HttpContext?.User
                .FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(id))
                throw new UnauthorizedCustomException(ResponseKeys.Unauthorized);

            return id;
        }

        public string GetCurrentUserEmail()
        {
            var email = _httpContextAccessor.HttpContext?.User
                .FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrWhiteSpace(email))
                throw new UnauthorizedCustomException(ResponseKeys.Unauthorized);

            return email;
        }
    }

}
