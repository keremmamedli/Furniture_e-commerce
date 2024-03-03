using AmadoApp.Business.Exceptions.Commons;
using AmadoApp.Business.Services.Implementations;
using AmadoApp.Business.Services.Interfaces;
using AmadoApp.Business.ViewModels.CategoryVMs;
using AmadoApp.Business.ViewModels.ColorVMs;
using AmadoApp.Business.ViewModels.PageVMs;
using AmadoApp.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AmadoApp.MVC.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ColorController : Controller
    {
        private readonly IColorService _ColorService;

        public ColorController(IColorService ColorService)
        {
            _ColorService = ColorService;
        }

        [HttpGet]
        [Authorize(Roles = "Moderator, Admin")]
        public async Task<IActionResult> Table(int page = 1)
        {

            IQueryable<Color> query = await _ColorService.ReadAsync();
            int pageSize = 4;

            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var colors = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            if (User.IsInRole("Admin"))
            {
                var homeVM = new HomeVM
                {
                    Colors = colors,
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
                    Colors = colors.Where(x => !x.IsDeleted).ToList(),
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
                Color oldColor = await _ColorService.ReadIdAsync(id);

                return View(oldColor);
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
                await _ColorService.DeleteAsync(id);

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
                await _ColorService.RecoverAsync(id);

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
                await _ColorService.RemoveAsync(id);

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
        public async Task<IActionResult> Create(CreateColorVM vm)
        {
            try
            {
                await _ColorService.CreateAsync(vm);

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
                Color oldColor = await _ColorService.ReadIdAsync(id);

                UpdateColorVM vm = new()
                {
                    Name = oldColor.Name,
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
        public async Task<IActionResult> Update(UpdateColorVM vm)
        {
            try
            {
                await _ColorService.UpdateAsync(vm);

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
