using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MyDeal.Entities
{
    public class Auction : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal ActualPrice { get; set; }
        public DateTime StartingTime { get; set; }
        public DateTime EndingTime { get; set; }
        public TypeOfCar TypeOfCar { get; set; }
        public  List<AuctionPicture> AuctionPictures { get; set; }
    }
}
