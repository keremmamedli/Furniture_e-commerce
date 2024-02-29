using AmadoApp.Business.ViewModels.ColorVMs;
using AmadoApp.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmadoApp.Business.Services.Interfaces
{
    public interface IColorService
    {
        Task<IQueryable<Color>> ReadAsync();
        Task<Color> ReadIdAsync(int Id); // Changed return type to Color
        Task CreateAsync(CreateColorVM entity);
        Task UpdateAsync(UpdateColorVM entity);
        Task DeleteAsync(int Id);
        Task RecoverAsync(int Id);
        Task RemoveAsync(int Id);
    }
}
