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
			CreateAuction model = new CreateAuction();
			CreateAuction auction = new CreateAuction();

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateAuction auction)
        {
            Auction createdAuction = new Auction() {
                Title = auction.Title,
                Description = auction.Description,
                ActualPrice = auction.ActualPrice,
                StartingTime = auction.StartingTime,
                EndingTime = auction.EndingTime,
                TypeOfCar = auction.TypeOfCar,
            };
            createdAuction.AuctionPictures = new List<AuctionPicture>();
            foreach (var item in auction.formFiles)
            {
                if (item.CheckFile(3))
                {
                    AuctionPicture picture = new AuctionPicture() {
                        Auction = createdAuction,
                        Picture = new Picture()
                        {
                            URL = await item.UploadFile(_webHostEnvironment.WebRootPath,"Upload")
                        }
                    };
                    createdAuction.AuctionPictures.Add(picture);
                }
            }
            _service.SaveAuction(createdAuction);
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
