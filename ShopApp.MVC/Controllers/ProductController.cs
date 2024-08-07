using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopApp.MVC.ViewModels.ProductVMs;
using System.Net.Http.Headers;

namespace ShopApp.MVC.Controllers
{
    public class ProductController : Controller
    {
        public async Task<IActionResult> Index()
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);
            HttpResponseMessage response = await client.GetAsync("http://localhost:5036/api/Product?page=1");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ProductListVM>(data);
                return View(result);

            }
            return BadRequest("error");
        }

    }
}
