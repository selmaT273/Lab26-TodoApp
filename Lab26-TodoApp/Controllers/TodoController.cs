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

        // Get: api/<TodoContoroller>
        [HttpGet]
        public async Task<List<Todo>> Get()
        {
            return await _todos.GetAllTodos();
        }

        [HttpGet("{id}")]
        public async Task<TodoDTO> Get(int id)
        {
            return await _todos.GetTodo(id);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] Todo todo)
        {
            todo.CreatedByUserId = GetuserId();

            await _todos.CreateTodo(todo);
             

            return Ok("complete");

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Todo todo)
        {
            await _todos.UpdateTodo(todo, id);
            return Ok("Complete");
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _todos.DeleteTodo(id);
        }

        private string GetuserId()
        {
           return ((ClaimsIdentity)User.Identity).FindFirst("UserId")?.Value;
        }
    }


}
