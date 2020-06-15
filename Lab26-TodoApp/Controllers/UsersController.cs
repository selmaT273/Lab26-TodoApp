using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab26_TodoApp.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lab26_TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<TodoUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IUserClaimsPrincipalFactory<TodoUser> userClaimsPrincipalFactory;

        public UsersController(UserManager<TodoUser> userManager,
            IUserClaimsPrincipalFactory<TodoUser> userClaimsPrincipalFactory,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            this.configuration = configuration;
        }

        [Authorize]
        [HttpGet("Self")]
        public async Task<IActionResult> Self()
        { 
            if (HttpContext.User.Identity is ClaimsIdentity claimsIdentity)
            {
                var usernameClaim = claimsIdentity.FindFirst("UserId");
                var userId = usernameClaim.Value;

                var user = await userManager.FindByIdAsync(userId);

                return Ok(new
                {
                    userId = user.Id,
                    user.Email,
                    user.FirstName,
                    user.LastName,
                });
            }

            return Unauthorized();
        }

        //[AllowAnonymous]
        //[Authorize]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterData register)
        {
            var user = new TodoUser
            {
                Email = register.Email,
                UserName = register.Email,

                FirstName = register.FirstName,
                LastName = register.LastName,
            };

            var result = await userManager.CreateAsync(user, register.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    message = "Registration did not succeed",
                    errors = result.Errors,
                });
            }

            //if (User.IsInRole("Administrator") || !await userManager.Users.AnyAsync())
            //{
            //    await userManager.AddToRolesAsync(user, register.Roles);
            //}

            return Ok(new
            {
                UserId = user.Id,
                Token = await CreateTokenAsync(user)
            });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginData login)
        {
            var user = await userManager.FindByNameAsync(login.Username);
            if(user == null)
            {
                return Unauthorized();
            }
            var result = await userManager.CheckPasswordAsync(user, login.Password);
            if (!result)
            {
                return Unauthorized();
            }

            return Ok(new
            {
                userId = user.Id,
                Token = await CreateTokenAsync(user)

            });

        }

        private async Task<string> CreateTokenAsync(TodoUser user)
        {
            var secret = configuration["JWT:Secret"];
            var secretBytes = Encoding.UTF8.GetBytes(secret);
            var signingKey = new SymmetricSecurityKey(secretBytes);

            var userPrincipal = await userClaimsPrincipalFactory.CreateAsync(user);
            var tokenClaims = new[]
            {
                new Claim("UserId", user.Id),
                new Claim("FullName", $"{user.FirstName} {user.LastName}"),
            }.Concat(userPrincipal.Claims);


            var token = new JwtSecurityToken(
                expires: DateTime.UtcNow.AddMonths(4),
                claims: tokenClaims,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }
    }
}
