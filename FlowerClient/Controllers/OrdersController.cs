using BuisinessObjects.Models;
using DataAccess;
using FlowerClient.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics.Metrics;
using System.Security.Claims;

namespace FlowerClient.Controllers
{

    [Authorize]
    public class OrdersController : Controller
    {
        public OrdersController()
        {
        }

  

        // Orders
        public async Task<IActionResult> Index()
        {
            try
            {
                HttpResponseMessage response;
                string fetchUrl = FlowerClientConfiguration.DefaultBaseApiUrl + "/Orders";

                if (!FlowerClientUtils.IsAdmin(User))
                {
                    string customerId = User.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.NameIdentifier)).Value;
                    fetchUrl += "/customer/" + customerId;
                }

                response = await FlowerClientUtils.ApiRequest(
                    FlowerHttpMethod.GET,
                    fetchUrl);

                if (response.IsSuccessStatusCode)
                {
                    IEnumerable<Order> orders =
                        await response.Content.ReadFromJsonAsync<IEnumerable<Order>>();
                    return View(orders);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Access", "Login");
                }
                else
                {
                    throw new Exception(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                ViewData["Orders"] = ex.Message;
                return View();
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public async Task<IActionResult> Report([FromQuery, BindRequired] DateTime? start, [FromQuery, BindRequired] DateTime? end)
        {
            try
            {
                if ((!start.HasValue || !end.HasValue)
                    && !(!start.HasValue && !end.HasValue)
                    )
                {
                    ViewData["SearchError"] = "Please input both start date and end date to generate a report!!";
                }

                ViewData["Start"] = start.Value.ToString("yyyy-MM-ddTHH:mm:ss");
                ViewData["End"] = end.Value.ToString("yyyy-MM-ddTHH:mm:ss");
                HttpResponseMessage response;
                string fetchUrl = FlowerClientConfiguration.DefaultBaseApiUrl +
                    "/Orders/search?start=" + start + "&end=" + end;


                response = await FlowerClientUtils.ApiRequest(
                    FlowerHttpMethod.GET,
                    fetchUrl);

                if (response.IsSuccessStatusCode)
                {
                    IEnumerable<Order> orders =
                        await response.Content.ReadFromJsonAsync<IEnumerable<Order>>();
                    return View(orders);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("AccessDenied", "Login");
                }
                else
                {
                    throw new Exception(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                ViewData["Orders"] = ex.Message;
                return View();
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if(id == null)
                {
                    throw new Exception("Id not the exits!");
                }
                HttpResponseMessage responseMessage = await FlowerClientUtils.ApiRequest(FlowerHttpMethod.GET, FlowerClientConfiguration.DefaultBaseApiUrl + "/Orders/" + id);
                if(responseMessage.IsSuccessStatusCode)
                {
                    Order order = await responseMessage.Content.ReadFromJsonAsync<Order>();
                    return View(order);
                }else if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Access", "Login");
                }
                else
                {
                    throw new Exception(await responseMessage.Content.ReadAsStringAsync());
                }
            }catch(Exception ex)
            {
                ViewData["Orders"] = ex.Message;
                return View();
            }

        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> CreateByCustomer([Bind("CustomerId,OrderDate,ShippedDate,Freight,Total,OrderStaus")] CartItem cart)
        //{
        //    if(ModelState.IsValid)
        //    {
        //        try
        //        {
        //            IEnumerable<Customer> customers = await GetCustomer();
        //            IEnumerable<FlowerBouquet> flowerBouquets= await GetFlowerBouquets();
        //            ViewData["CustomerId"]= new SelectList(customers, "CustomerId", "Email", cart.CustomerId);
        //            ViewData["FlowerList"] = new SelectList(flowerBouquets, "FlowerBouquetId", "FlowerBouquetName");
        //            ViewData["OrderDetails"] = HttpContext.Session.GetData<List<OrderDetail>>("OrderCart");

        //            IEnumerable<OrderDetail> details = HttpContext.Session.GetData<List<OrderDetail>>("OrderCart");
        //            if(details == null)
        //            {
        //                throw new Exception("You not orderdetail in here");
        //            }
        //            cart.OrderDetails = details.ToList();
        //            foreach(var item in cart.OrderDetails)
        //            {
        //                item.FlowerBouquet = null;
        //            }
        //            HttpResponseMessage responseMessage = await FlowerClientUtils.ApiRequest(FlowerHttpMethod.POST, FlowerClientConfiguration.DefaultBaseApiUrl + "/Orders", cart);
        //            if (responseMessage.IsSuccessStatusCode)
        //            {
        //                Order newOrder = await responseMessage.Content.ReadFromJsonAsync<Order>();
        //                if(newOrder == null)
        //                {
        //                    throw new Exception("Fail to new order");
        //                }
        //            }
        //            else if(responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized) {
        //                return RedirectToAction("Access", "Login");
        //            }
        //            else
        //            {
        //                throw new Exception(await responseMessage.Content.ReadAsStringAsync());
        //            }

        //        }
        //        catch(Exception ex)
        //        {
        //            ViewData["Orders"] = ex.Message;
        //            return View();
        //        }
        //        HttpContext.Session.SetData("OrderCart", null);
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(cart);
        //}

        //private bool ProductExistCart(IEnumerable<OrderDetail>cart, int flowerBouquetId)
        //{
        //    return cart.Any(c => c.FlowerBouquetId.Equals(flowerBouquetId));
        //}


        //private async Task<FlowerBouquet> GetFlowerBouquet(int flowerBouquetId)
        //{
        //    HttpResponseMessage responseMessage = await FlowerClientUtils.ApiRequest(FlowerHttpMethod.GET, FlowerClientConfiguration.DefaultBaseApiUrl + "/FlowerBouquets" + flowerBouquetId);
        //    if (responseMessage.IsSuccessStatusCode)
        //    {
        //        FlowerBouquet flowerBouquet = await responseMessage.Content.ReadFromJsonAsync<FlowerBouquet>();
        //        return flowerBouquet;
        //    }else if(responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized) {
        //        throw new Exception("Fails to get Flower");
        //    }
        //    else
        //    {
        //        throw new Exception(await responseMessage.Content.ReadAsStringAsync());
        //    }
            
        //}

        //[HttpPost]
        //[Authorize(Roles = "ADMIN")]
        //public async Task<IActionResult> AddToCart([Bind("FlowerBouquetId", "Quantity", "Discount")] CartItem cartModel)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            FlowerBouquet flower = await GetFlowerBouquet(cartModel.FlowerBouquetId);
        //            if (cartModel.Quantity > flower.UnitsInStock)
        //            {
        //                throw new Exception("Quantity must < " + flower.UnitsInStock);
        //            }
        //            List<OrderDetail> cart = HttpContext.Session.GetData<List<OrderDetail>>("OrderCart");
        //            if (cart == null)
        //            {
        //                cart = new List<OrderDetail>();
        //                cart.Add(new OrderDetail
        //                {
        //                    FlowerBouquetId = cartModel.FlowerBouquetId,
        //                    FlowerBouquet = flower,
        //                    UnitPrice = flower.UnitPrice,
        //                    Quantity = cartModel.Quantity,
        //                    Discount = cartModel.Discount
        //                });
        //            }
        //            else
        //            {
        //                if (ProductExistCart(cart, cartModel.FlowerBouquetId))
        //                {
        //                    int index = cart.FindIndex(od => od.FlowerBouquetId.Equals(cartModel.FlowerBouquetId));
        //                    cart[index].Quantity += cartModel.Quantity;
        //                }
        //                else
        //                {
        //                    cart.Add(new OrderDetail
        //                    {
        //                        FlowerBouquetId = cartModel.FlowerBouquetId,
        //                        FlowerBouquet = flower,
        //                        UnitPrice = flower.UnitPrice,
        //                        Quantity = cartModel.Quantity,
        //                        Discount = cartModel.Discount
        //                    });
        //                }
        //            }
        //            HttpContext.Session.SetData("OrderCart", cart);
                    
        //        }
        //        return RedirectToAction(nameof(CreateByCustomer));
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Orders"] = ex.Message;
        //        return RedirectToAction(nameof(CreateByCustomer));
        //    }

        //}


        private async Task<IEnumerable<Customer>> GetCustomer()
        {
            HttpResponseMessage response = await FlowerClientUtils.ApiRequest(FlowerHttpMethod.GET,FlowerClientConfiguration.DefaultBaseApiUrl + "/Customers");

            if (response.IsSuccessStatusCode)
            {
                IEnumerable<Customer> customers =
                    await response.Content.ReadFromJsonAsync<IEnumerable<Customer>>();
                return customers;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new Exception("Failed to get customer!");
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }


        private async Task<IEnumerable<FlowerBouquet>> GetFlowerBouquets()
        {
            HttpResponseMessage response = await FlowerClientUtils.ApiRequest(FlowerHttpMethod.GET,FlowerClientConfiguration.DefaultBaseApiUrl + "/FlowerBouquets");

            if (response.IsSuccessStatusCode)
            {
                IEnumerable<FlowerBouquet> flowerBouquets =
                    await response.Content.ReadFromJsonAsync<IEnumerable<FlowerBouquet>>();
                return flowerBouquets;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new Exception("Failed to get flowers!");
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }


    }
}
