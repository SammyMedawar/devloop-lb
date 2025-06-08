using DevLoopLB.Exceptions;
using DevLoopLB.Repositories.Interfaces;
using DevLoopLB.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace DevLoopLB.Services
{
    public class AccountService(IAccountRepository repository) : IAccountService
    {
        public async Task<string?> LoginAsync(string username, string password)
        {
            var account = await repository.GetByUsernameAsync(username);
            if (account == null)
            {
                throw new EntityNotFoundException("account", 0);
            }
            if(!VerifyPassword(password, account.HashedPassword))
            {
                throw new InvalidLoginException("Invalid username or password");
            }
            return "";
        }

        private bool VerifyPassword(string password, byte[] hashedPassword)
        {
            var passwordHash = HashPassword(password);
            return passwordHash.SequenceEqual(hashedPassword);
        }

        private byte[] HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}