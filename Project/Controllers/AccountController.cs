using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Project.Utilities;
using Project.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AccountController(SignInManager<User> _signInManager, UserManager<User> _userManager)
        {
            this._signInManager = _signInManager;
            this._userManager = _userManager;
        }

        [Route("google-login")]
        public IActionResult GoogleLogin()
        {
            var authenticationProperties = _signInManager.ConfigureExternalAuthenticationProperties("Google", Url.Action(nameof(HandleCallback)));
            return Challenge(authenticationProperties, "Google");
        }

        public async Task<IActionResult> HandleCallback()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);

            if ( !result.Succeeded ) //user does not exist yet
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var newUser = new User
                {
                    Email = email,
                    EmailConfirmed = true,
                    UserName = email,
                    Name = info.Principal.FindFirstValue(ClaimTypes.GivenName),
                    Surname = info.Principal.FindFirstValue(ClaimTypes.Surname),
                    Address = ""
                };
                var createResult = await _userManager.CreateAsync(newUser);
                if ( !createResult.Succeeded )
                    throw new Exception(createResult.Errors.Select(e => e.Description).Aggregate((errors, error) => $"{errors}, {error}"));

                await _userManager.AddLoginAsync(newUser, info);
                await _userManager.AddClaimsAsync(newUser, info.Principal.Claims);
                await _signInManager.SignInAsync(newUser, isPersistent: false);
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            }

            return Request.IsHttps ? Redirect("https://localhost:5001") : Redirect("http://localhost:5000");
        }

        [Authorize]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await this._signInManager.SignOutAsync();
            return Request.IsHttps ? Redirect("https://localhost:5001") : Redirect("http://localhost:5000");
        }

        [Route("/forbidden")]
        public async void Forbidden(string ReturnUrl)
        {
            Response.StatusCode = StatusCodes.Status403Forbidden;
            await Response.WriteAsync("You are not authorized!");    
        }
    }
}