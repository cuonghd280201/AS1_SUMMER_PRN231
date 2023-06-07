using BuisinessObjects.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class SupplierRepository : ISupplierRepository
    {
        public async Task<IEnumerable<Supplier>> GetSupplier()
        {
            return await SupplierDAO.Instance.GetSuppliers();
        }

        public async Task<Supplier> GetSupplier(int id)
        {
            return await SupplierDAO.Instance.GetSuppliers(id);
        }
    }
}
