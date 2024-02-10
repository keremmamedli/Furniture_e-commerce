using Microsoft.EntityFrameworkCore;
using MyDeal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDeal.Data
{
    public class MyDealContext : DbContext
    {
        public MyDealContext(DbContextOptions<MyDealContext> options) : base(options)
        {
        }
        public DbSet<Auction> Auctions { get; set; }
		public DbSet<Picture> Pictures { get; set; }
		public DbSet<AuctionPicture> AuctionPictures { get; set; }
	}
}