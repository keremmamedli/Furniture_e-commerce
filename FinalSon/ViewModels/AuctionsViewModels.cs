using MyDeal.Entities;

namespace FinalSon.ViewModels
{

    public class AuctionsListingViewModels
    {
        public List<Auction> Auctions { get; set; }
    }



    public class AuctionsViewModels 
	{
        public List<Auction> AllAuctions { get; set; }
		public List<Auction> PromotedAuctions { get; set; }

	}
}
