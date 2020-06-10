using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab26_TodoApp.Models.Interfaces
{
    public interface ITodoManager
    {
        Task<List<Todo>> GetAllTodos();
        Task CreateTodo(Todo todo);
    }
}
