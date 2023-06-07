using BuisinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class FlowerBouquestDAO
    {
        private static FlowerBouquestDAO instance = null;
        private static readonly object instanceLock = new object();
        private FlowerBouquestDAO() { }

        public static FlowerBouquestDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new FlowerBouquestDAO();
                    }
                    return instance;
                }
            }
        }
        //public async Task<IEnumerable<FlowerBouquet>> GetFlowers()
        //{
        //    var db = new FUFlowerBouquetManagementContext();
        //    return await db.FlowerBouquets.Include(f => f.Category).ToListAsync();

        //}

        public async Task<IEnumerable<FlowerBouquet>> GetFlowers()
        {
            var db = new FUFlowerBouquetManagementContext();
            return await db.FlowerBouquets.Include(f=> f.Category).Include(f=>f.Supplier).ToListAsync();
        }
        public async Task<IEnumerable<FlowerBouquet>> GetFlowers(int categoryId)
        {
            var db = new FUFlowerBouquetManagementContext();
            return await db.FlowerBouquets
               .Where(pro => pro.CategoryId == categoryId)
               .Include(pro => pro.Category)
               .ToListAsync();
        }

        public async Task<IEnumerable<FlowerBouquet>> GetFlowersSupplier(int supplierId)
        {
            var db = new FUFlowerBouquetManagementContext();
            return await db.FlowerBouquets.Where(sp => sp.SupplierId == supplierId).Include(sp => sp.Category).ToListAsync();
        }

        public async Task<FlowerBouquet> GetFlowersById(int flowerId)
        {
            var db = new FUFlowerBouquetManagementContext();
            return await db.FlowerBouquets.Include(c => c.Category).Include(s=> s.Supplier).SingleOrDefaultAsync(f => f.FlowerBouquetId == flowerId);
        }
        

        public async Task<FlowerBouquet> AddFlowerBouquest(FlowerBouquet flowerBouquet)
        {
            var db = new FUFlowerBouquetManagementContext();
            await db.FlowerBouquets.AddAsync(flowerBouquet);
            await db.SaveChangesAsync();
            return flowerBouquet;
        }

        public async Task<FlowerBouquet> UpdateFlowerBouquest(FlowerBouquet updatedFlowers)
        {
            if (await GetFlowersById(updatedFlowers.FlowerBouquetId) == null)
            {
                throw new Exception($"Product with the ID doesn't exist");
            }
            var database = new FUFlowerBouquetManagementContext();
            database.Entry<FlowerBouquet>(updatedFlowers).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await database.SaveChangesAsync();
            return updatedFlowers;
        }

        public async Task DeleteFlowerBouquest(int flowerId)
        {
            var db = new FUFlowerBouquetManagementContext();
            FlowerBouquet deleteFlower =   await GetFlowersById(flowerId);
            if(deleteFlower == null)
            {
                throw new Exception("Flower with the Id doesn't exist!");
            }
            db.FlowerBouquets.Remove(deleteFlower);
            await db.SaveChangesAsync();   

        }
    }
}
