using Microsoft.AspNetCore.Mvc;
using XuongMay.Contract.Repositories.Entity;
using XuongMay.Contract.Services.Interface;

namespace XuongMayBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // GET api/account/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountById(string id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return Ok(account);
        }

        // GET api/account/role/{role}
        [HttpGet("role/{role}")]
        public async Task<IActionResult> GetAccountsByRole(string role)
        {
            var accounts = await _accountService.GetAccountsByRoleAsync(role);
            return Ok(accounts);
        }

        // GET api/account
        [HttpGet]
        public async Task<IActionResult> GetAllAccounts()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            return Ok(accounts);
        }

        // PATCH api/account/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAccount(string id, [FromBody] Account updatedAccount)
        {
            if (updatedAccount == null)
            {
                return BadRequest("Invalid account data.");
            }

            try
            {
                await _accountService.UpdateAccountByIdAsync(id, updatedAccount);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
