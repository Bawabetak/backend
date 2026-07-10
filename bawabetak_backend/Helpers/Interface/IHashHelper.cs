namespace bawabetak_backend.Helpers.Interface
{
    public interface IHashHelper
    {
        string HashText(string text);
        bool VerifyHash(string text, string hash);
    }
}
