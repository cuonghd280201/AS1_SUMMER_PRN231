using BuisinessObjects.Models;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlowerClient.Controllers
{
    public class CategoriesController : Controller
    {
       public CategoriesController() { }


        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Index()
        {
            try
            {
                HttpResponseMessage responseMessage = await FlowerClientUtils.ApiRequest(FlowerHttpMethod.GET, FlowerClientConfiguration.DefaultBaseApiUrl + "/Categories");
                if(responseMessage.IsSuccessStatusCode)
                {
                    IEnumerable<Category> categories = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<Category>>();
                    return View(categories);
                }
                else if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Access", "Login");
                }
                else
                {
                    throw new Exception(await responseMessage.Content.ReadAsStringAsync());
                }

            }
            catch(Exception ex)
            {
                ViewData["Categories"] = ex.Message;
                return View();
            }
        }



        // Create categoryName, Description
        [Authorize(Roles = "ADMIN")]

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    HttpResponseMessage responseMessage = await FlowerClientUtils.ApiRequest(FlowerHttpMethod.POST, FlowerClientConfiguration.DefaultBaseApiUrl + "/Categories", category);
                    if(responseMessage.IsSuccessStatusCode)
                    {
                        Category newCategory = await responseMessage.Content.ReadFromJsonAsync<Category>();
                        if(newCategory != null)
                        {
                            throw new Exception("Fail to add new Category");
                        }
                    }
                    else  if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized){
                        return RedirectToAction("Access", "Login");

                    }
                    else
                    {
                        throw new Exception(await responseMessage.Content.ReadAsStringAsync());
                    }

                }catch(Exception ex)
                {
                    ViewData["Categories"] = ex.Message;
                    return View(category);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
    }
}
