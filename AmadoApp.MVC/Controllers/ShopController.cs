using AmadoApp.Business.Services.Interfaces;
using AmadoApp.Business.ViewModels.PageVMs;
using AmadoApp.Core.Entities;
using AmadoApp.DAL.Repositories.Interfaces;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Security.Claims;

namespace AmadoApp.MVC.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
		private readonly IBasketItemRepository _basketItemRepository;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IColorService _colorService;

		public ShopController(IProductService productService, ICategoryService categoryService, IBrandService brandService, IColorService colorService, IBasketItemRepository basketItemRepository, IHttpContextAccessor httpContextAccessor)
		{
			_productService = productService;
			_categoryService = categoryService;
			_brandService = brandService;
			_colorService = colorService;
			_basketItemRepository = basketItemRepository;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<List<BasketItem>> GetItemsByUserIdAsync(string userId)
		{
			return await (await _basketItemRepository.ReadAsync())
				.Where(x => x.AppUserId == userId)
				.ToListAsync();
		}   

		[HttpGet]
        public async Task<IActionResult> ShopList(int page = 1)
        {
            int pageSize = 8;

            IQueryable<Product> query = await _productService.ReadAsync();

            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var products = query.Where(x=>!x.IsDeleted).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var categories = (await _categoryService.ReadAsync()).ToList();
            var brands = (await _brandService.ReadAsync()).ToList();
            var colors = (await _colorService.ReadAsync()).ToList();

            var model = new HomeVM
            {
                Products = products,
                PageIndex = page,
                TotalPages = totalPages,
                Action = "ShopList",
                Controller = "Shop",
                PageSize = pageSize,
                Brands = brands,
                Categories = categories,
                Colors = colors // Renkleri burada atayın
            };

			if (User.Identity.IsAuthenticated)
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				var basketItems = await GetItemsByUserIdAsync(userId);
				model.BasketItems = basketItems;
			}
			else
			{
				var existingItems = _httpContextAccessor.HttpContext.Request.Cookies["BasketItems"];
				if (!string.IsNullOrEmpty(existingItems))
				{
					var items = existingItems.Split('|').Select(item =>
					{
						var parts = item.Split(':');
						return new BasketItem
						{
							ProductId = int.Parse(parts[0]),
							Count = int.Parse(parts[1]),
							Price = decimal.Parse(parts[2])
						};
					}).ToList();

					var groupedItems = items
						.GroupBy(i => i.ProductId)
						.Select(g => new BasketItem
						{
							ProductId = g.Key,
							Count = g.Sum(i => i.Count),
							Price = g.First().Price
						})
						.ToList();

					model.BasketItems = groupedItems;
				}
				else
				{
					model.BasketItems = new List<BasketItem>();
				}

			}

			return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ShopList(HomeVM vm, int page = 1)
        {
            var products = await _productService.GetAllBySearchAsync(vm.SearchVM.MinValue, vm.SearchVM.MaxValue, vm.SearchVM.Filter, vm.SearchVM.Search, vm.SearchVM.Brand, vm.SearchVM.Category,vm.SearchVM.Color);

            int pageSize = 8;

            int totalItems = products.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);


            var filteredProducts = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var categories = (await _categoryService.ReadAsync()).ToList(); // Fetch categories
            var brands = (await _brandService.ReadAsync()).ToList();
            var colors = (await _colorService.ReadAsync()).ToList();



            var homeVM = new HomeVM
            {
                Products = filteredProducts,
                PageIndex = page,
                TotalPages = totalPages,
                Action = "ShopList",
                Controller = "Shop",
                PageSize = pageSize,
                Brands = brands,
                Categories = categories,
                Colors = colors // Renkleri burada atayın
            };

            return View(homeVM);
        }


        public async Task<IActionResult> ProductSingle(int id)
        {
            Product oldProduct =  await _productService.ReadIdAsync(id);

            ViewData["Product"] = oldProduct;
			
			var products = await _productService.ReadAsync();
			var productList = products.ToList();

			HomeVM model = new()
			{
				Products = productList
			};

			if (User.Identity.IsAuthenticated)
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				var basketItems = await GetItemsByUserIdAsync(userId);
				model.BasketItems = basketItems;
			}
			else
			{
				var existingItems = _httpContextAccessor.HttpContext.Request.Cookies["BasketItems"];
				if (!string.IsNullOrEmpty(existingItems))
				{
					var items = existingItems.Split('|').Select(item =>
					{
						var parts = item.Split(':');
						return new BasketItem
						{
							ProductId = int.Parse(parts[0]),
							Count = int.Parse(parts[1]),
							Price = decimal.Parse(parts[2])
						};
					}).ToList();

					var groupedItems = items
						.GroupBy(i => i.ProductId)
						.Select(g => new BasketItem
						{
							ProductId = g.Key,
							Count = g.Sum(i => i.Count),
							Price = g.First().Price
						})
						.ToList();

					model.BasketItems = groupedItems;
				}
				else
				{
					model.BasketItems = new List<BasketItem>();
				}

			}

			return View(model);
        }
	}
}
