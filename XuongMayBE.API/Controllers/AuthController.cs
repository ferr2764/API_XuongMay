using Microsoft.AspNetCore.Mvc;
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

        // POST api/auth/register
        /// <summary>
        /// Register a new user account.
        /// </summary>
        /// <param name="account">The account details for registration.</param>
        /// <returns>A success message if the registration is successful.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModelView registerModel)
        {
            if (registerModel == null)
            {
                return BadRequest("Invalid registration data.");
            }

            try
            {
                await _authService.RegisterUserAsync(registerModel);
                return Ok("User registered successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/auth/login
        /// <summary>
        /// Log in a user with username and password.
        /// </summary>
        /// <param name="loginRequest">The login details (username and password).</param>
        /// <returns>A token and user details if login is successful.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModelView loginRequest)
        {
            if (loginRequest == null)
            {
                return BadRequest("Invalid login data.");
            }

            try
            {
                var (token, user) = await _authService.AuthenticateUserAsync(loginRequest.Username, loginRequest.Password);
                return Ok(new { Message = "Login successful", Token = token, user.Name, user.Username, user.Status, user.Salary});
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
