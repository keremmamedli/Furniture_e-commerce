using AmadoApp.Business.Services.Interfaces;
using AmadoApp.Business.ViewModels.PageVMs;
using AmadoApp.Core.Entities;
using Humanizer;
using Microsoft.AspNetCore.Mvc;

namespace AmadoApp.MVC.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;

        public ShopController(IProductService productService, ICategoryService categoryService, IBrandService brandService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _brandService = brandService;
        }

        public async Task<IActionResult> ShopList(decimal? min, decimal? max, string? order)
        {
            var query = (await _productService.ReadAsync()).ToList();
            ViewData["Categories"] = await _categoryService.ReadAsync();
            ViewData["Brands"] = await _brandService.ReadAsync();
            ViewData["Products"] = await _productService.ReadAsync();

            var products = new List<Product>();
			if (min == null && max == null && order == null)
			{
				products.AddRange(query);
			}
			else
			{
				products.AddRange(await _productService.GetAllBySearchAsync(min, max, order));
			}
            HomeVM homeVM = new HomeVM();
            homeVM.Products = products;


			return View(homeVM);
		}


        public async Task<IActionResult> ProductSingle(int id)
        {
            Product oldProduct =  await _productService.ReadIdAsync(id);

            ViewData["Product"] = oldProduct;
            return View();
        }
	}
}
