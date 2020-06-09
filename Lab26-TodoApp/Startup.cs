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
                .AddEntityFrameworkStores<ApplicationUserDbContext>();

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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
