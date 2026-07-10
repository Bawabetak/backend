namespace bawabetak_backend.Strategies.Login.Interface
{
    public interface ILoginFactory
    {
        ILoginStrategy GetStrategy(string clientType);
    }
}
