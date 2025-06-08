namespace DevLoopLB.Services.Interfaces
{
    public interface IAccountService
    {
        Task<string?> LoginAsync(string username, string password);
    }
}
