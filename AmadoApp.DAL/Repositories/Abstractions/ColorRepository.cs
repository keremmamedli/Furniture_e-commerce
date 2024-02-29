using AmadoApp.Core.Entities;
using AmadoApp.DAL.Context;
using AmadoApp.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmadoApp.DAL.Repositories.Abstractions
{
    public class ColorRepository : Repository<Color>, IColorRepository
    {
        public ColorRepository(AppDbContext context) : base(context) { }
    }
}
