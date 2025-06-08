using DevLoopLB.Models;

namespace DevLoopLB.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account?> GetByUsernameAsync(string username);
        Task SaveChangesAsync();
    }
}
