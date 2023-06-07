using BuisinessObjects.Models;
using DataAccess;
using FlowerClient.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;



namespace FlowerClient.Controllers
{
    [Authorize]

    public class FlowerBouquetsController : Controller
    {
        public FlowerBouquetsController() { }


        
        public async Task<IActionResult> Index()
        {
            try
            {
                //HttpResponseMessage response = await FlowerClientUtils.ApiRequest(
                //   FlowerHttpMethod.GET,
                //   FlowerClientConfiguration.DefaultBaseApiUrl + "/FlowerBouquets");
                HttpResponseMessage responseMessage;
                string fetchUrl = FlowerClientConfiguration.DefaultBaseApiUrl + "/FlowerBouquets";


                if (!FlowerClientUtils.IsAdmin(User))
                {
                    string customerId = User.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.NameIdentifier)).Value;
                    fetchUrl += "/customer/" + customerId;
                }

                responseMessage = await FlowerClientUtils.ApiRequest(
                    FlowerHttpMethod.GET,
                    fetchUrl);

                if (responseMessage.IsSuccessStatusCode)
                {
                    //string strData = await responseMessage.Content.ReadAsStringAsync();
                    //var options = new JsonSerializerOptions
                    //{
                    //    PropertyNameCaseInsensitive = true,
                    //};
                    //IEnumerable<FlowerBouquet> flowers = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<FlowerBouquet>>(strData, options);
                    IEnumerable<FlowerBouquet> flowers = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<FlowerBouquet>>();
                    return View(flowers);
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
                ViewData["FlowerBouquets"] = ex.Message;
                return View();
            }
        }
        private async Task<IEnumerable<Category>> GetCategories()
        {
            HttpResponseMessage response = await FlowerClientUtils.ApiRequest(
                FlowerHttpMethod.GET,
                FlowerClientConfiguration.DefaultBaseApiUrl + "/Categories");

            if (response.IsSuccessStatusCode)
            {
                IEnumerable<Category> categories =
                    await response.Content.ReadFromJsonAsync<IEnumerable<Category>>();
                return categories;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new Exception("Failed to get categories!");
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        private async Task<IEnumerable<Supplier>> GetSupplier()
        {
            HttpResponseMessage response = await FlowerClientUtils.ApiRequest(
                FlowerHttpMethod.GET,
                FlowerClientConfiguration.DefaultBaseApiUrl + "/Suppliers");

            if (response.IsSuccessStatusCode)
            {
                IEnumerable<Supplier> suppliers =
                    await response.Content.ReadFromJsonAsync<IEnumerable<Supplier>>();
                return suppliers;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new Exception("Failed supplier!");
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        [Authorize(Roles = "ADMIN")]


        // Flowers/Create by category and supplier
        public async Task<IActionResult> Create()
        {
            try
            {
                IEnumerable<Category> categories = await GetCategories();
                ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "CategoryName");
                
                IEnumerable<Supplier> suppliers =  await GetSupplier();
                ViewData["SupplierId"] = new SelectList(suppliers, "SupplierId", "SupplierName");
                return View();
            }
            catch (Exception ex)
            {
                ViewData["FlowerBouquets"] = ex.Message;
                return View();
            }
        }



        // create flowerboquets 
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("CategoryId,SupplierId,FlowerBouquetName,Description,UnitPrice,UnitsInStock,FlowerBouquetStatus")]FlowerBouquet flowerBouquet)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    IEnumerable<Category> categories = await GetCategories();
                    ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "CategoryName");

                    IEnumerable<Supplier> suppliers = await GetSupplier();
                    ViewData["SupplierId"] = new SelectList(suppliers, "SupplierId", "SupplierName");
                   
                    HttpResponseMessage responseMessage = await FlowerClientUtils.ApiRequest(FlowerHttpMethod.POST, FlowerClientConfiguration.DefaultBaseApiUrl + "/FlowerBouquets", flowerBouquet);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        //string responBody = await responseMessage.Content.ReadAsStringAsync();
                        //FlowerBouquet newFlower = JsonConvert.DeserializeObject<FlowerBouquet>(responBody);
                        FlowerBouquet newFlower = await responseMessage.Content.ReadFromJsonAsync<FlowerBouquet>();
                        if (newFlower == null)
                        {
                            throw new Exception("Failed to create flower!!");
                        }
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
                    ViewData["FlowerBouquets"] = ex.Message; 
                    return View(flowerBouquet);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(flowerBouquet);
        }


        [Authorize(Roles = "ADMIN")]

        // Flowers/edit/1

        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if(id == null)
                {
                    throw new Exception("id doesn't sepicial");
                }
                HttpResponseMessage responseMessage = await FlowerClientUtils.ApiRequest(FlowerHttpMethod.GET, FlowerClientConfiguration.DefaultBaseApiUrl + "/FlowerBouquets/" + id);
                if (responseMessage.IsSuccessStatusCode)
                {
                    FlowerBouquet flowerBouquet = await responseMessage.Content.ReadFromJsonAsync<FlowerBouquet>();

                    //string responBody = await responseMessage.Content.ReadAsStringAsync();
                    //FlowerBouquet flowerBouquet = JsonConvert.DeserializeObject<FlowerBouquet>(responBody);
                    ViewData["CategoryId"] = new SelectList(await GetCategories(), "CategoryId", "CategoryName", flowerBouquet.CategoryId);
                    ViewData["SupplierId"] = new SelectList(await GetSupplier(), "SupplierId", "SupplierName", flowerBouquet.SupplierId);
                    return View(flowerBouquet);
                }
                else if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized) 
                {
                    return RedirectToAction("Access", "Login");
                }else
                {
                    throw new Exception(await responseMessage.Content.ReadAsStringAsync());
                }
            }catch(Exception ex)
            {
                ViewData["FlowerBouquets"] = ex.Message;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, [Bind("FlowerBouquetId,CategoryId,FlowerBouquetName,Description,UnitPrice,UnitsInStock,FlowerBouquetStatus,SupplierId")] FlowerBouquet bouquet)
        {
            if (ModelState.IsValid)
            {
               
                try
                {
                    HttpResponseMessage responseMessage = await FlowerClientUtils.ApiRequest(FlowerHttpMethod.PUT, FlowerClientConfiguration.DefaultBaseApiUrl + "/FlowerBouquets/" + id, bouquet);

                    ViewData["CategoryId"] = new SelectList(await GetCategories(), "CategoryId", "CategoryName", bouquet.CategoryId);
                    ViewData["SupplierId"] = new SelectList(await GetSupplier(), "SupplierId", "SupplierName", bouquet.SupplierId);

                    if (responseMessage.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
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
                    ViewData["FlowerBouquets"] = ex.Message;
                    return View(bouquet);
                }
                
            }
            return View(bouquet);
        }

        // get flower by id
        [Authorize(Roles = "ADMIN")]

        public async Task<IActionResult> Delete(int? id)
        {
            try
            {

                if(id == null)
                {
                    throw new Exception("Flower ID doest not exist");
                }
                HttpResponseMessage responseMessage = await FlowerClientUtils.ApiRequest(FlowerHttpMethod.GET, FlowerClientConfiguration.DefaultBaseApiUrl + "/FlowerBouquets/" + id);
                if (responseMessage.IsSuccessStatusCode)
                {
                    FlowerBouquet flowerBouquet = await responseMessage.Content.ReadFromJsonAsync<FlowerBouquet>(); 
                    return View(flowerBouquet);
                }else if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Access", "Login");
                }
                else
                {
                    throw new Exception(await responseMessage.Content.ReadAsStringAsync());
                }
            }catch (Exception ex)
            {
                ViewData["FlowerBouquets"] = ex.Message;
                return View();
            }
        }

        // Delete flower by ID

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            try
            {
                HttpResponseMessage responseMessage = await FlowerClientUtils.ApiRequest(FlowerHttpMethod.DELETE, FlowerClientConfiguration.DefaultBaseApiUrl + "/FlowerBouquets/" + id);
                if (responseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                
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
                ViewData["FlowerBouquets"] =  ex.Message;
                return View();

            }
        }



        //public const string CARTKEY = "cart";

        //// Lấy cart từ Session (danh sách CartItem)
        //List<CartModel> GetCartItems()
        //{

        //    var session = HttpContext.Session;
        //    string jsoncart = session.GetString(CARTKEY);
        //    if (jsoncart != null)
        //    {
        //        return JsonConvert.DeserializeObject<List<CartModel>>(jsoncart);
        //    }
        //    return new List<CartModel>();
        //}

        //// Xóa cart khỏi session
        //void ClearCart()
        //{
        //    var session = HttpContext.Session;
        //    session.Remove(CARTKEY);
        //}

        //// Lưu Cart (Danh sách CartItem) vào session
        //void SaveCartSession(List<CartModel> ls)
        //{
        //    var session = HttpContext.Session;
        //    string jsoncart = JsonConvert.SerializeObject(ls);
        //    session.SetString(CARTKEY, jsoncart);
        //}


        //[Route("addcart/{productid:int}", Name = "addcart")]
        //public IActionResult AddToCart([FromRoute] int productid)
        //{

        //    var product = _context.Products
        //        .Where(p => p.ProductId == productid)
        //        .FirstOrDefault();
        //    if (product == null)
        //        return NotFound("Không có sản phẩm");

        //    // Xử lý đưa vào Cart ...
        //    var cart = GetCartItems();
        //    var cartitem = cart.Find(p => p.product.ProductId == productid);
        //    if (cartitem != null)
        //    {
        //        // Đã tồn tại, tăng thêm 1
        //        cartitem.quantity++;
        //    }
        //    else
        //    {
        //        //  Thêm mới
        //        cart.Add(new CartItem() { quantity = 1, product = product });
        //    }

        //    // Lưu cart vào Session
        //    SaveCartSession(cart);
        //    // Chuyển đến trang hiện thị Cart
        //    return RedirectToAction(nameof(Cart));
        //}
    }
}
