using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BuisinessObjects.Models;
using DataAccess;

namespace FlowerClient.Controllers
{
    public class LoginController : Controller
    {
        // LoginController
        [AllowAnonymous]
        public IActionResult Index([FromQuery] string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                if (FlowerClientUtils.IsAdmin(User))
                {
                    return RedirectToAction("Index", "Customers");
                }
                else
                {
                    return RedirectToAction("Index", "FlowerBouquets");
                }
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm, Bind("Email", "Password")] Customer customer,
                                                [FromForm, Bind("ReturnUrl")] string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                if (FlowerClientUtils.IsAdmin(User))
                {
                    return RedirectToAction("Index", "Customers");
                }
                else
                {
                    return RedirectToAction("Index", "FlowerBouquets");
                }
            }
            try
            {
                HttpResponseMessage response = await FlowerClientUtils.ApiRequest(
                FlowerHttpMethod.POST,
                FlowerClientConfiguration.DefaultBaseApiUrl + "/Customers/login",
                customer);

                if (response.IsSuccessStatusCode)
                {
                    CustomerWithRole loginMember = await response.Content.ReadFromJsonAsync<CustomerWithRole>();
                    if (loginMember == null)
                    {
                        throw new Exception("Failed to login! Please check again...");
                    }
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, loginMember.Email),
                        new Claim(ClaimTypes.NameIdentifier, loginMember.CustomerId.ToString()),
                        new Claim(ClaimTypes.Role, loginMember.CustomerRoleSring)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var memberPrincipal = new ClaimsPrincipal(new[] { claimsIdentity });

                    await HttpContext.SignInAsync(memberPrincipal);

                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        if (FlowerClientUtils.IsAdmin(User))
                        {
                            return RedirectToAction("Index", "Customers");
                        }
                        else
                        {
                            return RedirectToAction("Index", "FlowerBouquets");
                        }
                    }
                    return Redirect(returnUrl);

                }
                else
                {
                    throw new Exception(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                ViewData["Login"] = ex.Message;
                return View();
            }
        }

        [AllowAnonymous]
        public IActionResult AccessDenied([FromQuery] string returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }
    }
}
