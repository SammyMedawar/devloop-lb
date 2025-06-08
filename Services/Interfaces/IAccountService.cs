using DevLoopLB.DTO;

namespace DevLoopLB.Services.Interfaces
{
    public interface IAccountService
    {
        Task<JwtTokenResponse> LoginAsync(string username, string password, bool checkIfAdmin = false);
        Task<JwtTokenResponse> RefreshTokenAsync(string refreshToken);
        Task LogoutAsync(string refreshToken);
    }
}
