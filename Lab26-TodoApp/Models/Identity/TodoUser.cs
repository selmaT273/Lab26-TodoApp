using System;
using Microsoft.AspNetCore.Identity;

namespace Lab26_TodoApp.Models.Identity
{
    public class TodoUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

    }
}
