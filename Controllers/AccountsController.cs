using DevLoopLB.DTO;
using DevLoopLB.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevLoopLB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController(IAccountService accountService) : ControllerBase
    {

        [HttpPost("adminlogin")]
        public async Task<ActionResult> AdminLogin([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await accountService.LoginAsync(request.Username, request.Password);
            return Ok(response);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<JwtTokenResponse>> RefreshToken([FromBody] RefreshJwtTokenRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await accountService.RefreshTokenAsync(request.RefreshToken);
            return Ok(response);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult> Logout([FromBody] RefreshJwtTokenRequest request)
        {
            await accountService.LogoutAsync(request.RefreshToken);
            return Ok(new { message = "Logged out successfully" });
        }

    }
}
