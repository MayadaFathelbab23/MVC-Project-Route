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
    public class DepartmentRepository :GenericRepository<Department> ,  IDepartmentRepository
    {
        public DepartmentRepository(AppDbContext dbContext) : base(dbContext) { }

    }
}
