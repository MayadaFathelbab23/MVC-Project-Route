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
    public class GenericRepository<T> : IGenericRepository<T> where T : ModelBase
    {
        private protected readonly AppDbContext _dbContext;
        // Creating object from GenericRepository depands on AppDbContext object injection
        public GenericRepository(AppDbContext dbContext) // DI => Ask CLR to Create object from dbContext
                                                         // using UnitOfWork => injection is done in UnitOfWork class 
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(T entity)
             => await _dbContext.AddAsync(entity);
        
        public void Update(T entity)
            => _dbContext.Update(entity);
            

        public void Delete(T entity)
            => _dbContext.Remove(entity);
         

        public async Task<T> GetAsync(int id)
        {
            // -------------- Find Search by Id locally if not exists => search in Database
            return await _dbContext.FindAsync<T>(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Employee))
                return (IEnumerable<T>) await _dbContext.Employees.Include(E => E.Department).AsNoTracking().ToListAsync();
            else
                return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
            // To Eager load Navigational property in Employee model
        }
    }
}
