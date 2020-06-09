using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab26_TodoApp.Models;
using Lab26_TodoApp.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lab26_TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private ITodoManager _todos;

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
    }


}
