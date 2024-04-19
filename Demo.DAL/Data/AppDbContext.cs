using Demo.DAL.Data.Configuration;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Data
{
    public class AppDbContext :  IdentityDbContext<ApplicationUser> 
    {
        // IdentityDbContext has 7 tables/DbSets in Identity and also inherit from DbContext
        // IdentityDbContext Use IdentityUser , IdentityRole , Key is string [Default behavior]

        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) 
        {
            
        }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Need To call Fluent Api Configurations of identity tables
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); 
            
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
