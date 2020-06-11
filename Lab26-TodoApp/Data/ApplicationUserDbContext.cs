using System;
using Lab26_TodoApp.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lab26_TodoApp.Data
{
    public class ApplicationUserDbContext : IdentityDbContext<TodoUser>
    {
        public ApplicationUserDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var admin = new IdentityRole
            {
                Id = "admin",
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR",
                ConcurrencyStamp = "17722115-21fd-4451-8db4-e99f5c602421",
            };
            var moderator = new IdentityRole
            {
                Id = "moderator",
                Name = "Moderator",
                ConcurrencyStamp = "79b16ad0-71fc-4d45-851a-c8c0544adf1d",
            };
            builder.Entity<IdentityRole>()
                .HasData(admin, moderator);

            builder.Entity<IdentityRoleClaim<string>>()
                .HasData(
                    new IdentityRoleClaim<string> { Id = 1, RoleId = "admin", ClaimType = "Permissions", ClaimValue = "delete" },
                    new IdentityRoleClaim<string> { Id = 2, RoleId = "admin", ClaimType = "Permissions", ClaimValue = "create" },
                    new IdentityRoleClaim<string> { Id = 3, RoleId = "admin", ClaimType = "Permissions", ClaimValue = "read" },
                    new IdentityRoleClaim<string> { Id = 4, RoleId = "admin", ClaimType = "Permissions", ClaimValue = "update" }
                );
        }
    }
}
