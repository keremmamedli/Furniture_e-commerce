using AmadoApp.Core.Entities.Account;
using AmadoApp.Core.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmadoApp.Core.Entities
{
    public class BasketItem : BaseAuditableEntity
    {
        public int Count { get; set; }
        public decimal Price { get; set; }
        public string AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
