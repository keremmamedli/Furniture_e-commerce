using MyDeal.Entities;

namespace FinalSon.ViewModels
{
	public class CreateAuction
	{
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal ActualPrice { get; set; }
        public DateTime StartingTime { get; set; }
        public DateTime EndingTime { get; set; }
        public TypeOfCar TypeOfCar { get; set; }
        public List<Category> Categories { get; set; }
        public IFormFile[] formFiles { get; set; }
	}
}
