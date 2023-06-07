using BuisinessObjects.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository;
using System.Data;

namespace FlowerManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository orderRepository = new OrderRepository();

        public OrdersController(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        // api/Orders
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Order>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "ADMIN")]

        // Bat dong bo : xu ly cac yeu cau khac trong khi cho dong bo hoan thanh

        //public async Task<IActionResult> GetOrders()
        //{
        //    try
        //    {
        //        return StatusCode(200, await orderRepository.GetOrdersAsync());
        //    }
        //    catch (ApplicationException ae)
        //    {
        //        return StatusCode(400, ae.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}

        // Ma dong bo
        public ActionResult<IEnumerable<Order>> GetOrders() => orderRepository.GetAll();

        //api/Orders/1

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Order), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetOrder(int id)
        {
            try
            {
                Order order = await orderRepository.GetAllOrderByOrderId(id);
                if (order == null)
                {
                    return StatusCode(404, "Order is not existed!!");
                }

                CustomerRole customerRole = HttpContext.User.GetCustomerRole();
                if (customerRole == CustomerRole.USER)
                {
                    if (HttpContext.User.GetCustomerId() != order.CustomerId)
                    {
                        return StatusCode(404, "Order is not existed!!");
                    }
                }
                return StatusCode(200, order);
            }
            catch (ApplicationException ae)
            {
                return StatusCode(400, ae.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        //api/Orders(By CustomerId)

        [HttpGet("customer/{customerId}")]
        [ProducesResponseType(typeof(IEnumerable<Order>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize]
        public async Task<IActionResult> GetOrdet(int customerId)
        {
            try
            {
                CustomerRole customerRole = HttpContext.User.GetCustomerRole();
                if(customerRole == CustomerRole.USER)
                {
                    if (customerId != HttpContext.User.GetCustomerId()) 
                    {
                        return Unauthorized();
                    }
                }
                return StatusCode(200, await orderRepository.GetOrderById(customerId));
            }
            catch (ApplicationException ae)
            {
                return StatusCode(400, ae.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        //api/Orders

        [HttpPost]
        [ProducesResponseType(typeof(Order), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "ADMIN")]

        public async Task<IActionResult> PostOrder(Order order)
        {
            try
            {
                Order newOrder = await orderRepository.AddOrder(order);
                
                return StatusCode(201, newOrder);
            }
            catch (ApplicationException ae)
            {
                return StatusCode(400, ae.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }



        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "ADMIN")]

        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                   await orderRepository.DeleteOrder(id);
                return StatusCode(204, "Delete Successfully");
            }
            catch (ApplicationException ae)
            {
                return StatusCode(400, ae.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
        }


        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<Order>),200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "ADMIN")]

        public async Task<IActionResult> SearchOrder(DateTime start, DateTime end)
        {
            try
            {
                return StatusCode(200, await orderRepository.SearchOrder(start, end));
            }
            catch (ApplicationException ae)
            {
                return StatusCode(400, ae.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }





    }
}
