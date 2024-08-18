using Microsoft.AspNetCore.Authorization;
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
        /// <summary>
        /// Get an account by ID.
        /// </summary>
        /// <param name="id">The ID of the account.</param>
        /// <returns>The account details.</returns>
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
        /// <summary>
        /// Get accounts by role.
        /// Only accessible by Manager.
        /// </summary>
        /// <param name="role">The role of the accounts to retrieve.</param>
        /// <returns>A list of accounts with the specified role.</returns>
        [Authorize(Roles = "Manager")]
        [HttpGet("role/{role}")]
        public async Task<IActionResult> GetAccountsByRole(string role)
        {
            var accounts = await _accountService.GetAccountsByRoleAsync(role);
            return Ok(accounts);
        }

        // GET api/account
        /// <summary>
        /// Get all accounts.
        /// Only accessible by Manager.
        /// </summary>
        /// <returns>A list of all accounts.</returns>
        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> GetAllAccounts()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            return Ok(accounts);
        }

        // PUT api/account/{id}
        /// <summary>
        /// Update an account by ID.
        /// </summary>
        /// <param name="id">The ID of the account to update.</param>
        /// <param name="updatedAccount">The updated account details.</param>
        /// <returns>No content if the update is successful.</returns>
        [HttpPut("{id}")]
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

        // PATCH api/account/{id}/role
        /// <summary>
        /// Update the role of an account.
        /// </summary>
        /// <param name="id">The ID of the account to update.</param>
        /// <param name="role">The new role to assign to the account.</param>
        /// <returns>No content if the role update is successful.</returns>
        [HttpPatch("{id}/role")]
        public async Task<IActionResult> UpdateAccountRole(string id, [FromBody] string role)
        {
            if (string.IsNullOrEmpty(role))
            {
                return BadRequest("Role cannot be empty.");
            }

            try
            {
                await _accountService.UpdateAccountRoleAsync(id, role);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
