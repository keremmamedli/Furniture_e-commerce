using AmadoApp.Business.ViewModels.AccountVMs;
using AmadoApp.Business.ViewModels.BasketVMs;
using AmadoApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmadoApp.Business.ViewModels.PageVMs
{
	public class HomeVM
	{
		public SubscribeVM? SubscribeVM { get; set; }
        public List<Product>? Products { get; set; }
        public List<Brand>? Brands { get; set; }
        public List<Category>? Categories { get; set; }
        public List<Color>? Colors { get; set; }
		public SearchVM? SearchVM { get; set; }
        public List<BasketItem>? BasketItems { get; set; }
        public CheckoutVM? CheckoutVM { get; set; }

        // Pagination Section
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public int PageSize { get; set; }
    }
}
