using AmadoApp.Business.Exceptions.Commons;
using AmadoApp.Business.Services.Implementations;
using AmadoApp.Business.Services.Interfaces;
using AmadoApp.Business.ViewModels.CategoryVMs;
using AmadoApp.Business.ViewModels.PageVMs;
using AmadoApp.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AmadoApp.MVC.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _CategoryService;

        public CategoryController(ICategoryService CategoryService)
        {
            _CategoryService = CategoryService;
        }

        [HttpGet]
        [Authorize(Roles = "Moderator, Admin")]
        public async Task<IActionResult> Table(int page = 1)
        {
            IQueryable<Category> query = await _CategoryService.ReadAsync();
            int pageSize = 4;

            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var categories = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            if (User.IsInRole("Admin"))
            {
                var homeVM = new HomeVM
                {
                    Categories = categories,
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
                    Categories = categories.Where(x => !x.IsDeleted).ToList(),
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
                Category oldCategory = await _CategoryService.ReadIdAsync(id);

                return View(oldCategory);
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
                await _CategoryService.DeleteAsync(id);

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
                await _CategoryService.RecoverAsync(id);

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
                await _CategoryService.RemoveAsync(id);

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
        public async Task<IActionResult> Create(CreateCategoryVM vm)
        {
            try
            {
                await _CategoryService.CreateAsync(vm);

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
                Category oldCategory = await _CategoryService.ReadIdAsync(id);

                UpdateCategoryVM vm = new()
                {
                    Name = oldCategory.Name,
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
        public async Task<IActionResult> Update(UpdateCategoryVM vm)
        {
            try
            {
                await _CategoryService.UpdateAsync(vm);

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
