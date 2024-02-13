using Microsoft.AspNetCore.Mvc;
using MyDeal.Entities;
using MyDeal.Services;
using System;
using FinalSon.ViewModels;

namespace FinalSon.Controllers
{
    public class AuctionsController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly AuctionsService _service;

        public AuctionsController(AuctionsService service, IWebHostEnvironment webHostEnvironment)
        {
            _service = service;
            _webHostEnvironment = webHostEnvironment;
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
            CreateAuction auction = new CreateAuction();
            return View(auction);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateAuction auction)
        {
            foreach (var picture in auction.formFiles)
            {
                if (picture.CheckFile(3))
                {
                    Picture UploadPicture = new() { URL = await picture.UploadFile(_webHostEnvironment.WebRootPath,"Upload") };
                    AuctionPicture auctionPicture = new() {Auction = auction.auction, Picture = UploadPicture};
                    auction.auction.AuctionPictures.Add(auctionPicture);
                }
            }
            _service.SaveAuction(auction.auction);
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
