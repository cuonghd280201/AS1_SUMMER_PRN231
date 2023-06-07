using BuisinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class OrderDAO
    {
        private static OrderDAO instance = null;
        private static readonly object instanceLock = new object();
        private OrderDAO()
        {

        }
        public static OrderDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDAO();
                    }
                    return instance;
                }
            }
        }
        public async Task<IEnumerable<Order>> GetOrders()
        {
            var db = new FUFlowerBouquetManagementContext();
            IEnumerable<Order> orders = await db.Orders
                .Include(order => order.Customer)
                .Include(order => order.OrderDetails)
                .ToListAsync();
            foreach (Order order in orders)
            {
                if (order.Customer != null)
                {
                    if (order.Customer.Orders != null && order.Customer.Orders.Any())
                    {
                        order.Customer.Orders = null;
                    }
                }
            }
            return orders;
        }

        public static List<Order> GetOrder()
        {
            var listOrder = new List<Order>();
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    listOrder = context.Orders.Include(order => order.Customer)
                .Include(order => order.OrderDetails)
                .ToList();
                }

            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listOrder;

        }

        public async Task<Order> AddOrder(Order nOrder)
        {
            var db = new FUFlowerBouquetManagementContext();
            nOrder.OrderDate = DateTime.Now;
            if(nOrder.OrderDetails != null && nOrder.OrderDetails.Any()) {
                foreach(var item in nOrder.OrderDetails)
                {
                    FlowerBouquet flowerBouquet = await db.FlowerBouquets.FindAsync(item.FlowerBouquetId);
                    if(flowerBouquet.UnitsInStock < item.Quantity)
                    {
                        throw new Exception("Order Quantity more than UnitsInStock and Please check again! ");
                    }
                    flowerBouquet.UnitsInStock -= item.Quantity;
                    db.FlowerBouquets.Update(flowerBouquet);
                }

            }
            await db.Orders.AddAsync(nOrder);
            await db.SaveChangesAsync();
            return nOrder;

        }

        public async Task<IEnumerable<Order>> GetOrderById(int customerId)
        {
            var db = new FUFlowerBouquetManagementContext();
            IEnumerable<Order> order = await db.Orders.Where(order=>order.CustomerId == customerId)
                .Include(order => order.Customer)
                .Include(order => order.OrderDetails).ToListAsync();
            foreach(Order o in order) { 
                    if(o.Customer != null)
                {
                    if(o.Customer.Orders != null && o.Customer.Orders.Any()) {

                        o.Customer.Orders = null;
                    }
                }
            
            }
            return order;
        }

        public async Task<Order> UpdateOrder(Order uOrder)
        {
            var db = new FUFlowerBouquetManagementContext();
            if(await GetOrderById(uOrder.OrderId) == null)
            {
                throw new Exception("the order with Id doesn't exist!");
            }
            db.Orders.Update(uOrder);
            await db.SaveChangesAsync();
            return uOrder;

        }
        public async Task<Order> GetOrderByOrderId(int orderId)
        {
            var db = new FUFlowerBouquetManagementContext();
            return await db.Orders.Include(c => c.Customer).Include(o => o.OrderDetails).ThenInclude(f=>f.FlowerBouquet).SingleOrDefaultAsync(order=>order.OrderId== orderId);
        }
        public async Task DeleteOrder(int orderId)
        {
            var db = new FUFlowerBouquetManagementContext();
            Order deleteOrder = await GetOrderByOrderId(orderId);
            if(deleteOrder == null)
            {
                throw new Exception("The OrderId doesn't exist here!");
            }
            db.Orders.Remove(deleteOrder);
            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> SearchOrder(DateTime start , DateTime end)
        {
            var db = new FUFlowerBouquetManagementContext();
            IEnumerable<Order> orders = await db.Orders.Where(o => DateTime.Compare(o.OrderDate, start) >= 0 &&  DateTime.Compare(o.OrderDate, end) <= 0)
                .Include(c => c.Customer)
                .Include(o => o.OrderDetails).OrderByDescending(r => r.Total)
                .ToListAsync();
            foreach(Order oder in orders)
            {
                if(oder.Customer != null)
                {
                    if(oder.Customer.Orders != null && oder.Customer.Orders.Any())
                    {
                        oder.Customer.Orders = null;
                    }
                }

            }
            return orders;
        }
    }
}
