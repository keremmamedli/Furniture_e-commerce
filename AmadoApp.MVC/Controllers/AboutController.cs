using Microsoft.AspNetCore.Mvc;

namespace AmadoApp.MVC.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
