using AutoMapper;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Demo.PL.Helper;
using Demo.PL.MappingProfiles;
using Demo.PL.Settings;
using Demp.BLL.Interfaces;
using Demp.BLL.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; } 

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(); // Register built-in MVC Services to the container
            
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                options.EnableSensitiveDataLogging();
            }); 
          
            // ------------------ Only Enable DI to IUniOfWork
            services.AddScoped<IUnitOfWork , UnitOfWork>();
            // Allow DI to IMapper
            services.AddAutoMapper(M => M.AddProfiles(new List<Profile> { new EmployeeProfile(), new UserProfile() , new RoleProfile() , new DepartmentProfile()}));
            // Enable DI to Identity Services
            services.AddIdentity<ApplicationUser , IdentityRole>(config => // TUser , TRole
			{
                config.Password.RequiredLength = 5; 
                config.Password.RequireDigit = true; // 1254
                config.Password.RequireLowercase = true; // abc
                config.Password.RequireNonAlphanumeric = true; // @#$
                config.Password.RequireUppercase = true; // ABC
                config.User.RequireUniqueEmail = true;
                config.Lockout.MaxFailedAccessAttempts = 3;
                config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            }
                    ).AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();
			services.AddAuthentication(options =>
            {
				options.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
			})
            .AddGoogle(option =>
            {
                option.ClientId = Configuration.GetSection("ExternalAuth:Google")["ClientID"];
                option.ClientSecret = Configuration.GetSection("ExternalAuth:Google")["ClientSecret"];
            });
            // Configure mail settings -  MailKit
            services.Configure<EmailConfigurations>(Configuration.GetSection("EmailConfigurations"));
            services.AddScoped<IEmailSettings, EmailSettings>();
		}
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection(); // redirect from http to https request
            app.UseStaticFiles(); // to serve static files requests in wwwroot

            app.UseRouting(); // take url path , then detect it match which route
            // Security Middlewares
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
