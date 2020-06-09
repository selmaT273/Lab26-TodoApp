using System;
using Lab26_TodoApp.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lab26_TodoApp.Data
{
    public class ApplicationUserDbContext : IdentityDbContext<TodoUser>
    {
        public ApplicationUserDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
