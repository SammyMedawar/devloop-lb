using DevLoopLB.DTO;
using DevLoopLB.Exceptions;
using DevLoopLB.Models;
using DevLoopLB.Repositories.Interfaces;
using DevLoopLB.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DevLoopLB.Services
{
    public class AccountService(
        IAccountRepository repository,
        IConfiguration configuration,
        IMemoryCache cache) : IAccountService
    {
        private readonly string _jwtKey = configuration["Jwt:Key"]!;
        private readonly string _jwtIssuer = configuration["Jwt:Issuer"]!;
        private readonly string _jwtAudience = configuration["Jwt:Audience"]!;
        private readonly int _accessTokenExpiration = int.Parse(configuration["Jwt:AccessTokenExpirationMinutes"]!);
        private readonly int _refreshTokenExpiration = int.Parse(configuration["Jwt:RefreshTokenExpirationDays"]!);

        public async Task<JwtTokenResponse> LoginAsync(string username, string password, bool checkIfAdmin = false)
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
            if (checkIfAdmin && !account.IsAdmin)
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }
            return GenerateTokens(account);
        }


        public async Task<JwtTokenResponse> RefreshTokenAsync(string refreshToken)
        {
            if (!cache.TryGetValue($"refresh_token_{refreshToken}", out Account? account))
            {
                throw new InvalidLoginException("Invalid or expired refresh token");
            }
            cache.Remove($"refresh_token_{refreshToken}");

            return GenerateTokens(account!);
        }

        public Task LogoutAsync(string refreshToken)
        {
            cache.Remove($"refresh_token_{refreshToken}");
            return Task.CompletedTask;
        }

        #region Login Helpers

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
        #endregion

        #region Jwt Helpers
        private JwtTokenResponse GenerateTokens (Account account)
        {
            var accessToken = GenerateAccessToken(account);
            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpiry = TimeSpan.FromDays(_refreshTokenExpiration);
            cache.Set($"refresh_token_{refreshToken}", account, refreshTokenExpiry);

            return new JwtTokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_accessTokenExpiration),
                TokenType = "Bearer"
            };
        }
        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
        private string GenerateAccessToken(Account account)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtKey);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new(ClaimTypes.Name, account.Username),
                new(ClaimTypes.Email, account.Email),
                new("isAdmin", account.IsAdmin.ToString().ToLower())
            };
            if (account.IsAdmin)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_accessTokenExpiration),
                Issuer = _jwtIssuer,
                Audience = _jwtAudience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        #endregion
    }
}