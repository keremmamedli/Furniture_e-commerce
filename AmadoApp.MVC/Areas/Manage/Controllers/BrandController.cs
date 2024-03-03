using AmadoApp.Business.Exceptions.Commons;
using AmadoApp.Business.Services.Implementations;
using AmadoApp.Business.Services.Interfaces;
using AmadoApp.Business.ViewModels.BrandVMs;
using AmadoApp.Business.ViewModels.PageVMs;
using AmadoApp.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmadoApp.MVC.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class BrandController : Controller
    {
        private readonly IBrandService _BrandService;

        public BrandController(IBrandService BrandService)
        {
            _BrandService = BrandService;
        }

        [HttpGet]
        [Authorize(Roles = "Moderator, Admin")]
        public async Task<IActionResult> Table(int page = 1)
        {
            IQueryable<Brand> query = await _BrandService.ReadAsync();
            int pageSize = 4;

            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var brands = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            
            if (User.IsInRole("Admin"))
            {
                var homeVM = new HomeVM
                {
                    Brands = brands,
                    PageIndex = page,
                    TotalPages = totalPages,
                    Action = "ShopList",
                    Controller = "Shop",
                    PageSize = pageSize,
                };

                return View(homeVM);
            }
            else
            {
                var homeVM = new HomeVM
                {
                    Brands = brands.Where(x => !x.IsDeleted).ToList(),
                    PageIndex = page,
                    TotalPages = totalPages,
                    Action = "ShopList",
                    Controller = "Shop",
                    PageSize = pageSize,
                };

                return View(homeVM);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Moderator, Admin")]
        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                Brand oldBrand = await _BrandService.ReadIdAsync(id);

                return View(oldBrand);
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
                await _BrandService.DeleteAsync(id);

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
                await _BrandService.RecoverAsync(id);

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
                await _BrandService.RemoveAsync(id);

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
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateBrandVM vm)
        {
            try
            {
                await _BrandService.CreateAsync(vm);

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
                Brand oldBrand = await _BrandService.ReadIdAsync(id);

                UpdateBrandVM vm = new()
                {
                    Name = oldBrand.Name,
                };

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
        public async Task<IActionResult> Update(UpdateBrandVM vm)
        {
            try
            {
                await _BrandService.UpdateAsync(vm);

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
