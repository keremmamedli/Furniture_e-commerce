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
		public ActionResult Index()
		{
			var auctions = _service.GetAllAuctions();

			return View(auctions);
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

		[HttpGet]
		public ActionResult Edit(int ID)
		{
			var auction = _service.GetAuctionByID(ID);


			return View(auction);
		}

		[HttpPost]
		public ActionResult Edit(Auction auction)
		{
			_service.UpdateAuction(auction);
			return View(auction);
		}

		[HttpGet]
		public ActionResult Delete(int ID)
		{
			var auction_ = _service.GetAuctionByID(ID);
			return View(auction_);
		}

		[HttpPost]
		public ActionResult Delete(int ID,int b)
		{
			var auction_ = _service.GetAuctionByID(ID);

			_service.DeleteAuction(auction_);

			return RedirectToAction("Index");
		}
	}
}
