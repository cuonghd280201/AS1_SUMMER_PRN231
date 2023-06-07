using BuisinessObjects.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        public Task<IEnumerable<Category>> GetCategories()
        {
             return CategoryDAO.Instance.GetCategories();
        }

        public Task<Category> GetCategory(int categoryId)
        {
            return CategoryDAO.Instance.GetCategory(categoryId);
        }
    }
}
