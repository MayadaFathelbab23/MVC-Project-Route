using Demo.DAL.Data;
using Demo.DAL.Models;
using Demp.BLL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demp.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext dbContext) : base(dbContext) { }
       
        public IQueryable<Employee> GetEmployeesByAddress(string address)
        {
            return _dbContext.Employees.Where(E => E.Address.ToLower() == address.ToLower());
        }

        public IQueryable<Employee> SearchByName(string name)
        {
            return _dbContext.Employees.Where(E => E.Name.ToLower().Contains(name)); 
        }
    }
}
