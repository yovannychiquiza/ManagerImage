﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlazorAuthenticationSample.Client.Features.Accounts.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IDataProtector _dataProtector;

        public AccountController(IDataProtectionProvider dataProtectionProvider, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _dataProtector = dataProtectionProvider.CreateProtector("SignIn");
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet("account/signinactual")]
        public async Task<IActionResult> SignInActual(string t)
        {
            var data = _dataProtector.Unprotect(t);

            var parts = data.Split('|');

            var identityUser = await _userManager.FindByIdAsync(parts[0]);

            var isTokenValid = await _userManager.VerifyUserTokenAsync(identityUser, TokenOptions.DefaultProvider, "SignIn", parts[1]);

            if (isTokenValid)
            {
                await _signInManager.SignInAsync(identityUser, true);
                return Redirect("~/");
            }
            else
            {
                return Unauthorized("STOP!");
            }
        }

        [Authorize]
        [HttpGet("account/signout")]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();

            return Redirect("~/");
        }
    }
}
