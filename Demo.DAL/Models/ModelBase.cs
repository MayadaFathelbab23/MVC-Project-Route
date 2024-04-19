using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
    // All model classes will inherit from it
    public abstract class ModelBase
    {
        // Add common features | Columns between models [Id , CreatedAte , UpdatedAt , ..]
        public int Id { get; set; }
    }
}
