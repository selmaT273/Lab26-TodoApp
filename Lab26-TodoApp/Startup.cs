using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab26_TodoApp.Data;
using Lab26_TodoApp.Models.Services;
using Lab26_TodoApp.Models.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Lab26_TodoApp.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Lab26_TodoApp
{
    public class Startup
    {

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<TodoDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddDbContext<ApplicationUserDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("UsersConnection"));
            });

            services.AddIdentity<TodoUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationUserDbContext>()
                .AddDefaultTokenProviders();

       

            services
                .AddAuthentication(options =>
                {
                    // Avoid sending user to login page
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                // Authentication Handler
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;

                    var secret = Configuration["JWT:Secret"];
                    var secretBytes = Encoding.UTF8.GetBytes(secret);
                    var signingKey = new SymmetricSecurityKey(secretBytes);

                    // how we know this token came from us by telling us what secret to look at
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signingKey,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("todos.delete",
                    policy => policy.RequireClaim("permissions", "delete"));
            });

            services.AddTransient<ITodoManager, TodoService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

            // using (var scope = app.ApplicationServices.CreateScope())
            // {
            // var userManager = scope.ApplicationServices.GetService<UserManager<TodoUser>>();
            // SeedDatabase(userManager);
            // }


        }

        //private void SeedDatabase(UserManager<TodoUser> userManager)
        //{
        //    

        //    var hasUsers = await userManager.Users.AnyAsync();
        //    if (hasUsers)
        //    {
        //        return;
        //    }
        // 
        //    dbContext.Users.Add()
        //    Todo: add roles, initial admin user with known password, assign admin user to admin role
        //    userManager.CreateAsync(new BlogUser { ... } "p@ssw0rd");
        //    userManager.AddToRoleAsync(user, "admin");
        //}
    }
}
