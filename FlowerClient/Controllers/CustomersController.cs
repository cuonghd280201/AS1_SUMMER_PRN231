using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics.Metrics;
using DataAccess;
using BuisinessObjects.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Text.Json;

using System.Net.Http;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;

namespace FlowerClient.Controllers
{
    public class CustomersController : Controller
    {
        private readonly HttpClient client = null;
        private string ProductApiUrl = "";
        public CustomersController()
        {

            //client = new HttpClient();
            //var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            //client.DefaultRequestHeaders.Accept.Add(contentType);
            //ProductApiUrl = "https://localhost:7074/api/Customers";
        }

        // Customer
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Index()
        {
            try
            {
               // HttpResponseMessage response = await client.GetAsync(ProductApiUrl);


                HttpResponseMessage response = await FlowerClientUtils.ApiRequest(FlowerHttpMethod.GET,FlowerClientConfiguration.DefaultBaseApiUrl + "/Customers");

                if (response.IsSuccessStatusCode)
                {
                    string strData = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    List<Customer> customers = System.Text.Json.JsonSerializer.Deserialize<List<Customer>>(strData, options);
                   // IEnumerable<Customer> customers = await response.Content.ReadFromJsonAsync<IEnumerable<Customer>>();
                    return View(customers);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    throw new Exception(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                ViewData["Customers"] = ex.Message;
                return View();
            }
        }
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    HttpResponseMessage response = await FlowerClientUtils.ApiRequest(FlowerHttpMethod.POST,FlowerClientConfiguration.DefaultBaseApiUrl + "/Customers", customer);
                    if (response.IsSuccessStatusCode)
                    {
                        Customer createCustomer = await response.Content.ReadFromJsonAsync<Customer>();
                        if(createCustomer == null)
                        {
                            throw new Exception("Failed to create customer");
                        }
                    }
                    else if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
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
                    ViewData["Customers"] = ex.Message;
                    return View(customer);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new Exception("Customer is not exist!");
                }
                string role = User.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.Role)).Value;
                if (role.Equals(CustomerRole.USER.ToString()))
                {
                    if (id != int.Parse(User.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.NameIdentifier)).Value))
                    {
                        return RedirectToAction("Access", "Login");
                    }
                }

                HttpResponseMessage response = await FlowerClientUtils.ApiRequest(FlowerHttpMethod.GET,FlowerClientConfiguration.DefaultBaseApiUrl + "/Customers/" + id);

                if (response.IsSuccessStatusCode)
                {
                    Customer customer = await response.Content.ReadFromJsonAsync<Customer>();
                    return View(customer);
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
                ViewData["Customers"] = ex.Message;
                return View();
            }
        }

        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if(id == null)
                {
                    throw new Exception("Custormer is not exist!");
                }
                HttpResponseMessage response = await FlowerClientUtils.ApiRequest(FlowerHttpMethod.GET, FlowerClientConfiguration.DefaultBaseApiUrl + "/Customers/" + id);
                if (response.IsSuccessStatusCode)
                {
                    Customer customer = await response.Content.ReadFromJsonAsync<Customer>();
                    return View(customer);
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
            catch(Exception ex) {
                ViewData["Customers"] = ex.Message;
                return View();
            }
        }
        [Authorize(Roles = "ADMIN")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(int id)
        {
            try
            {
                HttpResponseMessage response = await FlowerClientUtils.ApiRequest(FlowerHttpMethod.DELETE, FlowerClientConfiguration.DefaultBaseApiUrl + "/Customers/" + id);

                if (response.IsSuccessStatusCode)
                {

                    return RedirectToAction(nameof(Index));
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
                ViewData["Customers"] = ex.Message;
                return View();
            }
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new Exception("Customer is not specified!");
                }
                string role = User.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.Role)).Value;
                if (role.Equals(CustomerRole.USER.ToString()))
                {
                    if (id != int.Parse(User.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.NameIdentifier)).Value))
                    {
                        return RedirectToAction("Access", "Login");
                    }
                }

                HttpResponseMessage response = await FlowerClientUtils.ApiRequest(FlowerHttpMethod.GET,FlowerClientConfiguration.DefaultBaseApiUrl + "/Customers/" + id);

                if (response.IsSuccessStatusCode)
                {
                    Customer customer = await response.Content.ReadFromJsonAsync<Customer>();
                    return View(customer);
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
                ViewData["Customers"] = ex.Message;
                return View();
            }
        }

        // Edit Customer
        
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,Email,CustomerName,City,Country,Password,Birthday")] Customer customer)
        {
            bool isAdmin = false;
            if (ModelState.IsValid)
            {
                try
                {
                    string role = User.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.Role)).Value;
                    if (role.Equals(CustomerRole.USER.ToString()))
                    {
                        if (id != int.Parse(User.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.NameIdentifier)).Value))
                        {
                            return RedirectToAction("Access", "Login");
                        }
                    }
                    else
                    {
                        isAdmin = true;
                    }

                    HttpResponseMessage response = await FlowerClientUtils.ApiRequest(FlowerHttpMethod.PUT,FlowerClientConfiguration.DefaultBaseApiUrl + "/Customers/" + id, customer);

                    if (response.IsSuccessStatusCode)
                    {
                        return isAdmin ? RedirectToAction(nameof(Index)) : RedirectToAction(nameof(Details), new { id = id });
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
                    ViewData["Customers"] = ex.Message;
                    return View(customer);
                }
            }
            return View(customer);
        }
        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }
    }
}
