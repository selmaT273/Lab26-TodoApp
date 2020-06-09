using System;
using Lab26_TodoApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab26_TodoApp.Data
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Todo>().HasData(
                new Todo
                {
                    Id = 1,
                    Title = "Taxes",
                    DueDate = new DateTime(2020, 06, 20),
                    Assignee = "Stacey",
                    Difficulty = 5
                },
                new Todo
                {
                    Id = 2,
                    Title = "Mail gift",
                    DueDate = new DateTime(2020, 06, 10),
                    Assignee = "Stacey2",
                    Difficulty = 2
                });

        }

        public DbSet<Todo> Todos { get; set; }
    }
}
