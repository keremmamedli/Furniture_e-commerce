using AmadoApp.Business.Services.Interfaces;
using AmadoApp.Business.ViewModels.PageVMs;
using AmadoApp.Core.Entities;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing.Printing;

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

        [HttpGet]
        public async Task<IActionResult> ShopList(int page = 1)
        {
            int pageSize = 8;

            IQueryable<Product> query = await _productService.ReadAsync();

            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var products = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var categories = (await _categoryService.ReadAsync()).ToList();
            var brands = (await _brandService.ReadAsync()).ToList();

            var homeVM = new HomeVM
            {
                Products = products.ToList(),
                PageIndex = page,
                TotalPages = totalPages,
                Action = "ShopList",
                Controller = "Shop",
                PageSize = pageSize,
                Brands = brands
            };

            return View(homeVM);
        }

        [HttpPost]
        public async Task<IActionResult> ShopList(HomeVM vm, int page = 1)
        {
            var products = await _productService.GetAllBySearchAsync(vm.SearchVM.MinValue, vm.SearchVM.MaxValue, vm.SearchVM.Filter, vm.SearchVM.Search,vm.SearchVM.Brand);

            int pageSize = 8;

            int totalItems = products.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);


            var filteredProducts = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var categories = (await _categoryService.ReadAsync()).ToList();
            var brands = (await _brandService.ReadAsync()).ToList();

            

            var homeVM = new HomeVM
            {
                Products = filteredProducts,
                PageIndex = page,
                TotalPages = totalPages,
                Action = "ShopList",
                Controller = "Shop",
                PageSize = pageSize,
                Brands = brands
            };

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
