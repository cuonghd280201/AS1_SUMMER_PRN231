using BuisinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface ISupplierRepository
    {
        Task<IEnumerable<Supplier>> GetSupplier();
        Task<Supplier> GetSupplier(int id);
    }
}
