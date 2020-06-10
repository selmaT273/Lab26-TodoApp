using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab26_TodoApp.Models.Interfaces
{
    public interface ITodoManager
    {
        Task<List<Todo>> GetAllTodos();
        Task CreateTodo(Todo todo);
        Task<TodoDTO> GetTodo(int id);
        Task<Todo> UpdateTodo(Todo todo, int id);
        Task DeleteTodo(int id);
        Task<List<Todo>> GetAllTodosByMe(string userId);
    }
}
