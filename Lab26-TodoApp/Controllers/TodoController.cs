using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Lab26_TodoApp.Models;
using Lab26_TodoApp.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lab26_TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private ITodoManager _todos;

        public object ClaimType { get; private set; }

        public TodoController(ITodoManager todos)
        {
            _todos = todos;
        }

        [AllowAnonymous]
        [Authorize]
        // Get: api/<TodoContoroller>
        [HttpGet]
        public async Task<List<Todo>> Get()
        {
            var user = User;
            return await _todos.GetMyTodos(GetuserId());
        }


        [Authorize]
        [HttpGet("GetMyTodos")]
        public async Task<List<Todo>> GetMyTodos()
        {
            return await _todos.GetMyTodos(GetuserId());
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoDTO>> Get(int id)
        {
            var todo = await _todos.GetTodo(id, GetuserId());
            if(todo == null)
            {
                return NotFound();
            }

            return todo;
        }


        [HttpPost]
        [Authorize(Policy = "todos.create")]
        public async Task<IActionResult> Post([FromBody] Todo todo)
        {
            todo.CreatedByUserId = GetuserId();
            //todo.CreatedByTimestamp = DateTime.UtcNow;

            await _todos.CreateTodo(todo);
             

            return Ok("complete");

        }


        [HttpPut("{id}")]
        [Authorize(Policy = "todos.update")]
        public async Task<IActionResult> Put(int id, [FromBody] Todo todo)
        {
            //todo.ModifiedByUserId = GetuserId();
            //todo.ModifiedByTimestamp = DateTime.UtcNow;

            await _todos.UpdateTodo(todo, id);
            return Ok("Complete");
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "todos.delete")]
        public async Task Delete(int id)
        {
            await _todos.DeleteTodo(id);
        }

        private string GetuserId()
        {
            ClaimsIdentity identity = (ClaimsIdentity)User.Identity;
            Claim userIdClaim = identity.FindFirst("UserId");
            return userIdClaim?.Value;
        }
    }


}
