using Microsoft.AspNetCore.Mvc;

namespace AmadoApp.MVC.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
