using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Repository;
using System.Diagnostics.Metrics;
using System.Security.Claims;
using FlowerManagementAPI.Model;
using BuisinessObjects.Models;
using Microsoft.AspNetCore.Authorization;

namespace FlowerManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository customerRepository;
        public CustomersController(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }





        [HttpPost("login")]
        [ProducesResponseType(typeof(CustomerWithRole), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [AllowAnonymous]
        public async Task<IActionResult> Login(CustomerLogin loginInfo)
        {
            try
            {
                if (string.IsNullOrEmpty(loginInfo.Email) ||
                    string.IsNullOrEmpty(loginInfo.Password))
                {
                    throw new ApplicationException("Login Fail!");
                }

                Customer loginCustomer = await customerRepository.Login(loginInfo.Email, loginInfo.Password);
                if (loginCustomer == null)
                {
                    throw new ApplicationException("Fail Login!!");
                }
                CustomerRole customerRole = CustomerRole.USER;
                Customer defaultCustomer = customerRepository.GetDefaultCustomer();
                if (loginCustomer.Email.Equals(defaultCustomer.Email))
                {
                    customerRole = CustomerRole.ADMIN;
                }
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, loginCustomer.Email),
                    new Claim(ClaimTypes.NameIdentifier, loginCustomer.CustomerId.ToString()),
                    new Claim(ClaimTypes.Role, customerRole.ToString())
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = false,
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), authProperties);

                //loginMember.MemberId = 0;
                loginCustomer.Password = "";
                loginCustomer.Orders = null;

                CustomerWithRole reCustomer = new CustomerWithRole(loginCustomer);
                reCustomer.CustomerRoleSring = customerRole.ToString();

                return StatusCode(200, reCustomer);
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
   
        


        // api/Customers
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Customer>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetCustomer()
        {
            try
            {
                return StatusCode(200, await customerRepository.GetCustomer());
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

        // api/Customer/1
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Customer), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize]

        public async Task<IActionResult> GetCustomer(int id)
        {
            try
            {
                CustomerRole role = HttpContext.User.GetCustomerRole();
                if (role == CustomerRole.USER)
                {
                    if (id != HttpContext.User.GetCustomerId())
                    {
                        return Unauthorized();
                    }
                }
                Customer member = await customerRepository.GetCustomer(id);
                if (member == null)
                {
                    return StatusCode(404, "CustomerId is not existed!!");
                }
                return StatusCode(200, member);
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

        // Papi/Customer/5
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return StatusCode(400, "Not the same ID!");
            }

            try
            {
                CustomerRole role = HttpContext.User.GetCustomerRole();
                if (role == CustomerRole.USER)
                {
                    if (id != HttpContext.User.GetCustomerId())
                    {
                        return Unauthorized();
                    }
                }
                await customerRepository.UpdateCustomer(customer);
                return StatusCode(204, "Update successfully!");
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

        // api/Customer
        [HttpPost]
        [ProducesResponseType(typeof(Customer), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> PostCustomer(Customer customer)
        {
            try
            {
                Customer createdMember = await customerRepository.AddCustomer(customer);
                return StatusCode(201, createdMember);
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

        // api/Customer/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                await customerRepository.DeleteCustomer(id);    
                return StatusCode(204, "Delete successfully!");
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
    

