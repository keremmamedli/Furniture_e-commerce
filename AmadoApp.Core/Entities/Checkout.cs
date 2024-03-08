using AmadoApp.Core.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmadoApp.Core.Entities
{
    public class Checkout : BaseAuditableEntity
    {
        public string FullName { get; set; }
        public string CardNumber { get; set; }
        public string PhoneNumber { get; set; } 
        public string Month { get; set; }
        public string Year { get; set; }
        public string CVV { get; set; }
    }
}
