using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab26_TodoApp.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lab26_TodoApp.Data
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<TodoUser> userManager;
        private readonly SignInManager<TodoUser> signInManager;

        public UsersController(UserManager<TodoUser> userManager, SignInManager<TodoUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
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
            });

        }
    }
}
