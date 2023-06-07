using BuisinessObjects.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class OrderRepository : IOrderRepository
    {
        public async Task<Order> AddOrder(Order order)
        {
            return  await OrderDAO.Instance.AddOrder(order);
        }

        public async Task DeleteOrder(int orderId)
        {
             await OrderDAO.Instance.DeleteOrder(orderId);
        }

        public List<Order> GetAll()
        {
            return OrderDAO.GetOrder();
        }

        public async Task<Order> GetAllOrderByOrderId(int orderId)
        {
            return await OrderDAO.Instance.GetOrderByOrderId(orderId);
        }

        public async Task<IEnumerable<Order>> GetOrderById(int customerId)
        {
            return await OrderDAO.Instance.GetOrderById(customerId);
        }

        //public  Task<IEnumerable<Order>> GetOrdersAsync()
        //{
        //    return  OrderDAO.Instance.GetOrders();
        //}

        public async Task<IEnumerable<Order>> SearchOrder(DateTime start, DateTime end)
        {
            return await OrderDAO.Instance.SearchOrder(start, end);
        }

        public async Task<Order> UpdateOrder(Order order)
        {
            return await OrderDAO.Instance.UpdateOrder(order);
        }
    }
}
