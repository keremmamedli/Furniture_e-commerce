using Microsoft.AspNetCore.Mvc;
using MyDeal.Entities;
using MyDeal.Services;
using System;
using FinalSon.ViewModels;

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
            AuctionsListingViewModels model = new AuctionsListingViewModels();


            model.Auctions = _service.GetAllAuctions();

            return View(model);
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
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int ID)
        {
            var auction = _service.GetAuctionByID(ID);

            if (auction != null)
            {
                ViewBag.Title = auction.Title;
                return View(auction);
            }
            else
            {
                return Redirect("/Home/Error");
            }
        }

        [HttpPost]
        public ActionResult Edit(Auction auction)
        {
            _service.UpdateAuction(auction);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int ID)
        {

            var auction_ = _service.GetAuctionByID(ID);
            if(auction_ != null)
            {
                return View(auction_);
            }
            else
            {
                return Redirect("/Home/Error");
            }
        }

        [HttpPost]
        public ActionResult Delete(int ID, int b)
        {

            var auction_ = _service.GetAuctionByID(ID);

            _service.DeleteAuction(auction_);

            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Details(int ID)
        {
            var auction_ = _service.GetAuctionByID(ID);

            if (auction_ != null)
            {
                ViewBag.Title = auction_.Title;
                return View(auction_);
            }
            else
            {
                return Redirect("/Home/Error");
            }
        }
    }
}
