using DataAccess;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlowerClient.Controllers
{
    public class LogoutController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            FlowerClientUtils.ApiRequest(FlowerHttpMethod.POST, FlowerClientConfiguration.DefaultBaseApiUrl + "/Customers/logout");
            HttpContext.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}
