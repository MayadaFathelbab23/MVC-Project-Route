using Demo.DAL.Data;
using Demp.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demp.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        //Set all Repositories properties
        public IEmployeeRepository EmployeeRepository { get ; set ; }
        public IDepartmentRepository DepartmentRepository { get ; set; }

        public UnitOfWork(AppDbContext dbContext) 
        {
            _dbContext = dbContext;
            // Initialize Repository 
            EmployeeRepository = new EmployeeRepository(_dbContext);
            DepartmentRepository = new DepartmentRepository(_dbContext);
        }
        public async Task<int> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose(); // To close database connection after request
                                  // called by CLR as IUniteOfWork inheritr from IDisposable
        }
    }
}
