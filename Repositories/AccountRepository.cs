using DevLoopLB.Models;
using DevLoopLB.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DevLoopLB.Repositories
{
    public class AccountRepository (DevLoopLbContext context): IAccountRepository
    {
        public async Task<Account?> GetByUsernameAsync(string username)
        {
            return await context.Accounts
                .FirstOrDefaultAsync(a => a.Username == username);
        }
        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
