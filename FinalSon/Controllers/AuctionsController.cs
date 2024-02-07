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

<<<<<<< HEAD
		[HttpGet]
		public ActionResult Index()
		{
			var auctions = _service.GetAllAuctions();
			if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
			{
				return PartialView(auctions);
			}
			else
			{
				return View(auctions);
			}
		}

		[HttpGet]
		public ActionResult Create()
		{
			return PartialView();
		}

		[HttpPost]
		public ActionResult Create(Auction auction)
		{
			_service.SaveAuction(auction);
			return RedirectToAction("Index");
		}

		[HttpGet]
		public ActionResult Edit(int ID)
		{
			var auction = _service.GetAuctionByID(ID);
=======
        [HttpPost]
        public ActionResult Create(Auction auction)
        {
            _service.SaveAuction(auction);
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int ID)
        {
            var auction = _service.GetAuctionByID(ID);
>>>>>>> parent of 7f0fe53 (Basic Auction CRUD(3) Updated Code)


            return View(auction);
        }

        [HttpPost]
        public ActionResult Edit(Auction auction)
        {
            _service.UpdateAuction(auction);
            return View(auction);
        }
    }
}
