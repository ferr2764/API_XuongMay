using Microsoft.AspNetCore.Mvc;
using XuongMay.Contract.Repositories.Entity;
using XuongMay.Contract.Services.Interface;
using XuongMay.ModelViews.AuthModelViews;


namespace XuongMayBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Account account)
        {
            if (account == null)
            {
                return BadRequest("Invalid registration data.");
            }

            try
            {
                await _authService.RegisterUserAsync(account);
                return Ok("User registered successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModelView loginRequest)
        {
            if (loginRequest == null)
            {
                return BadRequest("Invalid login data.");
            }

            try
            {
                var user = await _authService.AuthenticateUserAsync(loginRequest.Username, loginRequest.Password);
                return Ok(new { Message = "Login successful", User = user });
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
