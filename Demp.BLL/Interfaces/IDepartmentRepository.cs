using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demp.BLL.Interfaces
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        // IEnumerable => Return records to enumerat it (Reading)
        // ICollecion => Return records and enable =>  Read - Update - add 
        // IQuarable => Get records with filteration [filter in DB and return result]
        // IReadOnlyList => Read but cannot enumerate (only return records in response)
        // --------------- 
      
    }
}
