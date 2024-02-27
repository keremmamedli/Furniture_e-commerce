using Microsoft.AspNetCore.Mvc;

namespace AmadoApp.MVC.Controllers
{
    public class ShopPageController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult> AccessDeniedCustom()
        {
            return View();
        }
    }
}
