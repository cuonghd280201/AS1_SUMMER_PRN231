using BuisinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CategoryDAO
    {
        private static CategoryDAO instance = null;
        private static readonly object instanceLock = new object();
        private CategoryDAO()
        {

        }

        public static CategoryDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CategoryDAO();
                    }
                    return instance;
                }
            }
        }
        public async Task<IEnumerable<Category>> GetCategories()
        {
            var database = new FUFlowerBouquetManagementContext();
            return await database.Categories.ToListAsync();
        }

       

        public async Task<Category> GetCategory(int categoryId)
        {
            var database = new FUFlowerBouquetManagementContext();
            return await database.Categories.SingleOrDefaultAsync(member => member.CategoryId == categoryId);
        }
    }
}
