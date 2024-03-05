using Microsoft.AspNetCore.Mvc;
using AmadoApp.Business.Services.Interfaces;
using AmadoApp.Business.ViewModels.PageVMs;
using System.Threading.Tasks;
using System.Collections.Generic; // List tipini kullanmak için gerekli using directive

namespace AmadoApp.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _service;

        public HomeController(IProductService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _service.ReadAsync(); // ProductService'de bulunan ReadAsync metodu çağrılıyor
            var productList = products.ToList(); // IQueryable<Product> tipindeki veriyi List<Product> tipine dönüştürüyoruz
            var model = new HomeVM
            {
                Products = productList
            };
            return View(model);
        }
        public async Task<IActionResult> AccessDeniedCustom()
        {
            return View();
        }
        public IActionResult ErrorPage()
        {
            // ERR_CACHE_MISS hatası olduğunda Index sayfasına yönlendir
            return RedirectToAction("Index", "Home");
        }
    }
}
