using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmadoApp.Business.ViewModels.PageVMs
{
    public class SearchVM
    {
        public string? MinValue { get; set; }
        public string? MaxValue { get; set; }
        public string? Filter { get; set; }
        public string? Search { get; set; }
        public string? Brand { get; set; }
    }
}
