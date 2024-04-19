using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demp.BLL.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        // NOTE => Dont make IQueryable to work Async ??
        // As it filter in Database then return result => Database does not work Asynchronously
        IQueryable<Employee> GetEmployeesByAddress(string address);
        IQueryable<Employee> SearchByName(string name);
    }
}
