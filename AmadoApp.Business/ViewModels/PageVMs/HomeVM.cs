using AmadoApp.Business.ViewModels.AccountVMs;
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
	}
}
