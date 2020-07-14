using System;
using Lab26_TodoApp.Models.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lab26_TodoApp.Data;
using Lab26_TodoApp.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace Lab26_TodoApp.Models.Services
{
    public class TodoService : ITodoManager
    {
        private TodoDbContext _context;
        private readonly UserManager<TodoUser> userManager;

        public TodoService(TodoDbContext context, UserManager<TodoUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        public async Task CreateTodo(Todo todo)
        {
            _context.Entry(todo).State = EntityState.Added;
            await _context.SaveChangesAsync();

        }

        public async Task DeleteTodo(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            _context.Entry(todo).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        public async Task<List<Todo>> GetAllTodos()
        {
            return await _context.Todos.ToListAsync();
        }

        public async Task<TodoDTO> GetTodo(int id, string userId)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null || todo.CreatedByUserId != userId) return null;

            var user = await userManager.FindByIdAsync(todo.CreatedByUserId);

            return new TodoDTO
            {
                Id = todo.Id,
                Title = todo.Title,
                CreatedBy = user == null ? null : $"{user.FirstName} {user.LastName}",
            };
        }

        public async Task<Todo> UpdateTodo(Todo todo, int id)
        {
            if (todo.Id == id)
            {
                _context.Entry(todo).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            return todo;
        }

        public Task<List<Todo>> GetMyTodos(string userId)
        {
            return _context.Todos
                .Where(t => t.CreatedByUserId != null && t.CreatedByUserId == userId)
                .ToListAsync();
        }
    }
}
