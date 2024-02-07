using Microsoft.VisualBasic;
using MyDeal.Data;
using MyDeal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDeal.Services
{
    public class AuctionsService
    {
        private readonly MyDealContext _context;
        public AuctionsService(MyDealContext context)
        {
            _context = context;

        }

        public void SaveAuction(Auction auction)
        {
            _context.Auctions.Add(auction);
            _context.SaveChanges();
        }

        public Auction GetAuctionByID(int ID)
        {
            return _context.Auctions.Find(ID);
        }

        public void UpdateAuction(Auction auction)
        {
            _context.Auctions.Update(auction);
            _context.SaveChanges();
        }
    }
}