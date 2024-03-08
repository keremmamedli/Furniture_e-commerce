using AmadoApp.Business.Services.Interfaces;
using AmadoApp.Business.ViewModels.BasketVMs;
using AmadoApp.Business.ViewModels.PageVMs;
using AmadoApp.Core.Entities;
using AmadoApp.Core.Entities.Account;
using AmadoApp.DAL.Context;
using AmadoApp.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace AmadoApp.MVC.Controllers
{
    public class BasketController : Controller
    {
        private readonly IProductService _productService;
        private readonly IBasketItemRepository _basketItemRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;

        public BasketController(IProductService productService, IBasketItemRepository basketItemRepository, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor, AppDbContext context)
        {
            _productService = productService;
            _basketItemRepository = basketItemRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var productList = await (await _productService.ReadAsync()).ToListAsync();

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

        [HttpPost]
        public async Task<IActionResult> Checkout(HomeVM model)
        {
            model.Products = await (await _productService.ReadAsync()).ToListAsync();

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

            CheckoutVMValidator validations = new CheckoutVMValidator();
            var validationResult = await validations.ValidateAsync(model.CheckoutVM);

            if (!validationResult.IsValid)
            {
                ModelState.Clear();

                validationResult.Errors.ForEach(x => ModelState.AddModelError(x.PropertyName, x.ErrorMessage));

                return View(model);
            }

            Checkout newCheckout = new()
            {
                FullName = model.CheckoutVM.FullName,
                CardNumber = model.CheckoutVM.CardNumber,
                Month = model.CheckoutVM.Month,
                Year = model.CheckoutVM.Year,
                CVV = model.CheckoutVM.CVV,
                PhoneNumber = model.CheckoutVM.PhoneNumber,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
            };

            await _context.Checkouts.AddAsync(newCheckout);
            await _context.SaveChangesAsync();

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var basketItems = await GetItemsByUserIdAsync(userId);

                _context.BasketItems.RemoveRange(basketItems);
                await _context.SaveChangesAsync();
            }
            else
            {
                Response.Cookies.Delete("BasketItems");
            }


            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var productList = await (await _productService.ReadAsync()).ToListAsync();

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

        [HttpGet]
        public async Task<IActionResult> AddItem(int id, string? returnUrl)
        {
            var product = await _productService.ReadIdAsync(id);

            await AddItemToBasket(product.Id, 1, product.Price);

            if(returnUrl != null)
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteItem(int id)
        {
            await DeleteItemFromBasket(id);

            return RedirectToAction("Index", "Basket");
        }


        public async Task AddItemToBasket(int productId, int count, decimal price)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                await AddItemToCookie(productId, count, price);
            }
            else
            {
                await AddItemToDatabase(userId, productId, count, price);
            }
        }

        private async Task AddItemToCookie(int productId, int count, decimal price)
        {
            var existingItems = _httpContextAccessor.HttpContext.Request.Cookies["BasketItems"];
            var newItem = $"{productId}:{count}:{price}";

            var updatedItems = string.IsNullOrEmpty(existingItems) ? newItem : $"{existingItems}|{newItem}";

            _httpContextAccessor.HttpContext.Response.Cookies.Append("BasketItems", updatedItems, new CookieOptions
            {
                Expires = DateTime.UtcNow.AddHours(1)
            });
        }

        private async Task AddItemToDatabase(string userId, int productId, int count, decimal price)
        {
            BasketItem existingItem = await (await _basketItemRepository.ReadAsync())
                .FirstOrDefaultAsync(x => x.AppUserId == userId && x.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Count += count;
                await _basketItemRepository.UpdateAsync(existingItem);
            }
            else
            {
                var newItem = new BasketItem
                {
                    AppUserId = userId,
                    ProductId = productId,
                    Count = count,
                    Price = price
                };
                await _basketItemRepository.CreateAsync(newItem);
            }

            await _basketItemRepository.SaveChangesAsync();
        }

        public async Task DeleteItemFromBasket(int productId)
        {
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrEmpty(userId))
            {
                await RemoveItemFromCookie(productId);
            }
            else
            {
                await RemoveItemFromDatabase(userId, productId);
            }
        }

        private async Task RemoveItemFromCookie(int productId)
        {
            var existingItems = _httpContextAccessor.HttpContext.Request.Cookies["BasketItems"];

            if (!string.IsNullOrEmpty(existingItems))
            {
                var items = existingItems.Split('|').ToList();
                items.RemoveAll(x => x.Split(':')[0] == productId.ToString());
                var updatedItems = string.Join("|", items);

                _httpContextAccessor.HttpContext.Response.Cookies.Append("BasketItems", updatedItems, new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddHours(1)
                });
            }
        }

        private async Task RemoveItemFromDatabase(string userId, int productId)
        {
            BasketItem itemToRemove = await (await _basketItemRepository.ReadAsync())
                .FirstOrDefaultAsync(x => x.AppUserId == userId && x.ProductId == productId);

            if (itemToRemove != null)
            {
                await _basketItemRepository.RemoveAsync(itemToRemove.Id);
                await _basketItemRepository.SaveChangesAsync();
            }
        }


    }
}
