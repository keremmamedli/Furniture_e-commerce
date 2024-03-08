using AmadoApp.Business.Exceptions.Commons;
using AmadoApp.Business.Services.Interfaces;
using AmadoApp.Business.ViewModels.ProductVMs;
using AmadoApp.Core.Entities;
using AmadoApp.DAL.Context;
using AmadoApp.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList;
using PagedList.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;
using AmadoApp.Business.Services.Implementations;
using System.Linq;
using AmadoApp.Business.ViewModels.PageVMs;

namespace AmadoApp.MVC.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ProductController : Controller
    {
        private readonly IProductService _ProductService;
        private readonly IWebHostEnvironment _env;

        private readonly IBrandRepository _repBrand;
        private readonly ICategoryRepository _repCat;
        private readonly AppDbContext _context;

        public ProductController(IProductService ProductService, IWebHostEnvironment env, IBrandRepository repBrand, ICategoryRepository repCat, AppDbContext context)
        {
            _ProductService = ProductService;
            _env = env;
            _repBrand = repBrand;
            _repCat = repCat;
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Moderator, Admin")]
        public async Task<IActionResult> Table(int page = 1)
        {
            IQueryable<Product> query = await _ProductService.ReadAsync();
            int pageSize = 4;

            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var products = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var homeVM = new HomeVM
            {
                Products = products,
                PageIndex = page,
                TotalPages = totalPages,
                Action = "ShopList",
                Controller = "Shop",
                PageSize = pageSize,
            };

            return View(homeVM);
        }
        [HttpGet]
        [Authorize(Roles = "Moderator, Admin")]
        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                Product oldProduct = await _ProductService.ReadIdAsync(id);

                return View(oldProduct);
            }
            catch (IdNegativeOrZeroException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                return RedirectToAction(nameof(Table));
            }
            catch (ObjectNullException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                return RedirectToAction(nameof(Table));
            }
        }

        [HttpGet]
        [Authorize(Roles = "Moderator, Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _ProductService.DeleteAsync(id);

                return RedirectToAction(nameof(Table));
            }
            catch (IdNegativeOrZeroException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                return RedirectToAction(nameof(Table));
            }
            catch (ObjectNullException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                return RedirectToAction(nameof(Table));
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Recover(int id)
        {
            try
            {
                await _ProductService.RecoverAsync(id);

                return RedirectToAction(nameof(Table));
            }
            catch (IdNegativeOrZeroException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                return RedirectToAction(nameof(Table));
            }
            catch (ObjectNullException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                return RedirectToAction(nameof(Table));
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                await _ProductService.RemoveAsync(id);

                return RedirectToAction(nameof(Table));
            }
            catch (IdNegativeOrZeroException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                return RedirectToAction(nameof(Table));
            }
            catch (ObjectNullException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                return RedirectToAction(nameof(Table));
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            CreateProductVM vm = new()
            {
                Brands = await _repBrand.ReadAsync(),
                Categories = await _repCat.ReadAsync(),
                Colors = await _context.Colors.ToListAsync()
            };

            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateProductVM vm)
        {
            vm.Brands = await _repBrand.ReadAsync();
            vm.Categories = await _repCat.ReadAsync();
            vm.Colors = await _context.Colors.ToListAsync();

            try
            {
                await _ProductService.CreateAsync(vm, _env.WebRootPath);

                return RedirectToAction(nameof(Table));
            }
            catch (ObjectParamsNullException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                return View(vm);
            }
            catch (ObjectSameParamsException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                return View(vm);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                Product oldProduct = await _ProductService.ReadIdAsync(id);

                UpdateProductVM vm = new()
                {
                    Title = oldProduct.Title,
                    Description = oldProduct.Description,
                    Price = oldProduct.Price,
                    IsInStock = oldProduct.IsInStock,
                    BrandId = oldProduct.BrandId,
                    ProductImageVMs = new List<ProductImageVM>(),
                    Brands = await _repBrand.ReadAsync(),
                    Categories = await _repCat.ReadAsync(),
                    CategoryIds = oldProduct.ProductCategories.Select(x => x.CategoryId).ToList(),
                    Colors = await _context.Colors.ToListAsync(),
                    ColorIds = oldProduct.ProductColors.Select(x => x.ColorId).ToList(),
                };

                foreach (var item in oldProduct.ProductImages)
                {
                    ProductImageVM productImageVM = new()
                    {
                        Id = item.Id,
                        ImageUrl = item.ImageUrl,
                    };

                    vm.ProductImageVMs.Add(productImageVM);
                }


                return View(vm);
            }
            catch (IdNegativeOrZeroException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                return RedirectToAction(nameof(Table));
            }
            catch (ObjectNullException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                return RedirectToAction(nameof(Table));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UpdateProductVM vm)
        {
            vm.Brands = await _repBrand.ReadAsync();
            vm.Categories = await _repCat.ReadAsync();
            vm.Colors = await _context.Colors.ToListAsync();

            try
            {
                await _ProductService.UpdateAsync(vm, _env.WebRootPath);

                return RedirectToAction(nameof(Table));
            }
            catch (IdNegativeOrZeroException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                return RedirectToAction(nameof(Table));
            }
            catch (ObjectNullException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                return RedirectToAction(nameof(Table));
            }
            catch (ObjectParamsNullException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                return View(vm);
            }
            catch (ObjectSameParamsException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                return View(vm);
            }
        }
    }
}
