using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XuongMay.Contract.Services.Interface;
using XuongMay.ModelViews.AuthModelViews;
using System;

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountById(Guid id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return Ok(account);
        }

        [Authorize(Roles = "Manager")]
        [HttpGet("role/{role}")]
        public async Task<IActionResult> GetAccountsByRole(string role)
        {
            var accounts = await _accountService.GetAccountsByRoleAsync(role);
            return Ok(accounts);
        }

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> GetAllAccounts(int pageNumber = 1, int pageSize = 5)
        {
            var accounts = await _accountService.GetAllAccountsAsync(pageNumber, pageSize);
            return Ok(accounts);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(Guid id, [FromBody] UpdateAccountModelView updatedAccount)
        {
            if (updatedAccount == null)
            {
                return BadRequest("Invalid account data.");
            }

            try
            {
                await _accountService.UpdateAccountAsync(id, updatedAccount);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpPut("{id}/delete")]
        public async Task<IActionResult> DeleteAccount(Guid id)
        {
            try
            {
                await _accountService.DeleteAccountAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPatch("{id}/role")]
        public async Task<IActionResult> UpdateAccountRole(Guid id, [FromBody] string role)
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
