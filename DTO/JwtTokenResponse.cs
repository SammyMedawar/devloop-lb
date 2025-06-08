namespace DevLoopLB.DTO
{
    public class JwtTokenResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public string TokenType { get; set; } = "Bearer";
    }

    public class RefreshJwtTokenRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
