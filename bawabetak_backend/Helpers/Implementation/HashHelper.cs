namespace bawabetak_backend.Helpers.Implementation
{
    public class HashHelper : IHashHelper
    {
        public string HashText(string text)
        {
            using var sha256 = SHA256.Create();

            var bytes = Encoding.UTF8.GetBytes(text);
            var hash = sha256.ComputeHash(bytes);

            return Convert.ToHexString(hash);
        }

        public bool VerifyHash(string text, string hash)
        {
            var computedHash = HashText(text);

            return string.Equals(
                computedHash,
                hash,
                StringComparison.OrdinalIgnoreCase);
        }
    }
}
