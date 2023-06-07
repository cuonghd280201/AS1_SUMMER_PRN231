using BuisinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class SupplierDAO
    {
        private static SupplierDAO instance = null;
        private static readonly object instanceLock = new object();
        private SupplierDAO()
        {

        }

        public static SupplierDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new SupplierDAO();
                    }
                    return instance;
                }
            }
        }
        public async Task<IEnumerable<Supplier>> GetSuppliers()
        {
            var db = new FUFlowerBouquetManagementContext();
            return await db.Suppliers.ToListAsync();
        }



        public async Task<Supplier> GetSuppliers(int supplierId)
        {
            var db = new FUFlowerBouquetManagementContext();
            return await db.Suppliers.SingleOrDefaultAsync(s => s.SupplierId == supplierId);
        }
    }
}
