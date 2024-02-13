using MyDeal.Entities;

namespace FinalSon.ViewModels
{
    public class CreateAuction
    {
        public Auction auction { get; set; }
        public IFormFile[] formFiles { get; set; }
    }
}
