using Microsoft.AspNetCore.Mvc;

namespace AmadoApp.MVC.Controllers
{
    public class ServicesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
