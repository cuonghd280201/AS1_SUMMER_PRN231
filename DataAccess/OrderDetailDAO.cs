using BuisinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class OrderDetailDAO
    {
        private static OrderDetailDAO instance; 
        private readonly static object instanceLock = new object();

        private OrderDetailDAO() { }
        public static OrderDetailDAO Instance
        {
            get
            {
                lock(instanceLock)
                {
                    if(instance == null)
                    {
                        instance = new OrderDetailDAO();
                    }
                    return instance;
                }
            }
            
        }
        public async Task<IEnumerable<OrderDetail>> GetOrderDetails(int orderId)
        {
            var db = new FUFlowerBouquetManagementContext();
            return await db.OrderDetails.Where(order=>order.OrderId == orderId).Include(f => f.FlowerBouquet).ToListAsync();
        }
    }
}
