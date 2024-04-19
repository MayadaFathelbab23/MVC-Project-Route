using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Data.Configuration
{
    internal class EmployeeConfigurations : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(E => E.Name).HasColumnType("varchar").HasMaxLength(50).IsRequired();
            builder.Property(E => E.Address).IsRequired();
            builder.Property(E => E.Salary).HasColumnType("decimal(12 , 2)");
            builder.Property(E => E.EmployeeGender)
                .HasConversion(
                    (Gender) => Gender.ToString(), // Save EmployeeGender in DB as string
                    (StringGender) => (Gender) Enum.Parse(typeof(Gender) , StringGender  , true) // Get from DB as Enum
                    );
            builder.Property(E => E.EmployeeType)
                .HasConversion(
                (EmployeeType) => EmployeeType.ToString(),
                (StringEmployeeType) => (EmployeeType) Enum.Parse(typeof(EmployeeType) , StringEmployeeType , true)
                );

            builder.HasOne(E => E.Department)
                .WithMany(D => D.Employees)
                .HasForeignKey(E => E.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
