using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lab26_TodoApp.Data;
using Lab26_TodoApp.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lab26_TodoApp.Models.Services
{
    public class TodoService : ITodoManager
    {
        private TodoDbContext _context;

        public TodoService(TodoDbContext context)
        {
            _context = context;
        }

        public async Task<List<Todo>> GetAllTodos()
        {
            return await _context.Todos.ToListAsync();
        }
    }
}
