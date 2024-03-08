using AmadoApp.Business.Services.Interfaces;
using AmadoApp.Business.ViewModels.PageVMs;
using AmadoApp.Core.Entities;
using AmadoApp.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AmadoApp.MVC.Controllers
{
    public class BlogController : Controller
    {
        private readonly IProductService _service;
        private readonly IBasketItemRepository _basketItemRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BlogController(IHttpContextAccessor httpContextAccessor, IProductService service, IBasketItemRepository basketItemRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _service = service;
            _basketItemRepository = basketItemRepository;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _service.ReadAsync();
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

        public async Task<List<BasketItem>> GetItemsByUserIdAsync(string userId)
        {
            return await (await _basketItemRepository.ReadAsync())
                .Where(x => x.AppUserId == userId)
                .ToListAsync();
        }
    }
}
