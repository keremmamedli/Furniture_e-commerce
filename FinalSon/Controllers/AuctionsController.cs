using Microsoft.AspNetCore.Mvc;
using MyDeal.Entities;
using MyDeal.Services;

namespace FinalSon.Controllers
{
    public class AuctionsController : Controller
    {
        private readonly AuctionsService _service;
        public AuctionsController(AuctionsService service)
        {
            _service = service;
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Auction auction)
        {
            _service.SaveAuction(auction);
            return View();
        }
    }
}
