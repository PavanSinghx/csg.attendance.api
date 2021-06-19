using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSG.Attendance.Api.Middleware;
using CSG.Attendance.Api.Models;
using CSG.Attendance.Api.Models.Mappings;
using CSG.Attendance.Api.Repositories;
using CSG.Attendance.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CSG.Attendance.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment)
        {
            var builder = new ConfigurationBuilder()
                              .SetBasePath(environment.ContentRootPath)
                              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                              .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AttendanceContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:DBConnectionString"]));

            services.AddSingleton<IMemoryCache, MemoryCache>();
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
            services.Configure<ClassSettings>(Configuration.GetSection("ClassSettings"));

            services.AddScoped<DbContext, AttendanceContext>();
            services.AddHttpContextAccessor();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IMemoryCacheService, MemoryCacheService>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
