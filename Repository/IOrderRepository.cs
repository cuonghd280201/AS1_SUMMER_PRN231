using BuisinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IOrderRepository
    {
        //Task<IEnumerable<Order>> GetOrdersAsync();

        List<Order> GetAll();
        Task<IEnumerable<Order>> GetOrderById(int customerId);
        Task<Order> GetAllOrderByOrderId(int orderId);

        Task<IEnumerable<Order>> SearchOrder(DateTime start, DateTime end);

        Task<Order> UpdateOrder(Order order);
        Task<Order> AddOrder(Order order);
        Task DeleteOrder(int orderId);

    }
}
