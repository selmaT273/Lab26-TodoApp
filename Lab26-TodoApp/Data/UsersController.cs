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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lab26_TodoApp.Data
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<TodoUser> userManager;
        private readonly IConfiguration configuration;

        public UsersController(UserManager<TodoUser> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }

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

            return Ok(new
            {
                UserId = user.Id,
                Token = CreateToken(user)
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
                Token = CreateToken(user)

            });

        }

        private string CreateToken(TodoUser user)
        {
            var secret = configuration["JWT:Secret"];
            var secretBytes = Encoding.UTF8.GetBytes(secret);
            var signingKey = new SymmetricSecurityKey(secretBytes);

            var tokenClaims = new[]
            {
                new Claim("UserId", user.Id),
                new Claim("FullName", $"{user.FirstName} {user.LastName}"),
            };

            var token = new JwtSecurityToken(
                claims: tokenClaims,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                ) ;

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }
    }
}
