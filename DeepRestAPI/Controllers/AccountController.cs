using DeepRestAPI.Data.Models;
using DeepRestAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DeepRestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<APPUser> _userManager;

        public AccountController(UserManager<APPUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterNewUser(DtoNewUser user)
        {
            if (ModelState.IsValid)
            {
                APPUser appUser = new()
                {
                    UserName = user.userName,
                    Email = user.email,
                };
                IdentityResult result = await _userManager.CreateAsync(appUser, user.password);
                if (result.Succeeded) 
                {
                    return Ok(new { message = "User created successfully" });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LogIn(DtoLogin login)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(login.userName);
                if (user != null && await _userManager.CheckPasswordAsync(user, login.password))
                {
                    return Ok(new { message = "Login successful" });
                }
                else
                {
                    ModelState.AddModelError("InvalidLogin", "Invalid username or password");
                }
            }

            return BadRequest(ModelState);
        }
    }
}
