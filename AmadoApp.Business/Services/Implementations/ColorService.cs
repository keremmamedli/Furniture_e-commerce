using AmadoApp.Business.Exceptions.Commons;
using AmadoApp.Business.Services.Interfaces;
using AmadoApp.Business.ViewModels.CategoryVMs;
using AmadoApp.Business.ViewModels.CategoryVMs;
using AmadoApp.Business.ViewModels.ColorVMs;
using AmadoApp.Core.Entities;
using AmadoApp.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmadoApp.Business.Services.Implementations
{
    public class ColorService : IColorService
    {
        private readonly IColorRepository _rep;

        public ColorService(IColorRepository rep)
        {
            _rep = rep;
        }

        public async Task<IQueryable<Color>> ReadAsync()
        {
            IQueryable<Color> colors = await _rep.ReadAsync();

            return colors;
        }

        public async Task<Color> ReadIdAsync(int Id)
        {
            return await CheckColor(Id);
        }

        public async Task CreateAsync(CreateColorVM entity)
        {
            if (entity.Name is null) throw new ObjectParamsNullException("Object parameters is required!", nameof(entity.Name));

            var existsSameName = await (await _rep.ReadAsync(expression: x => x.Name == entity.Name))
                .FirstOrDefaultAsync() is null;

            if (!existsSameName) throw new ObjectSameParamsException("There is same name category in data!", nameof(entity.Name));

            Color newColor = new()
            {
                Name = entity.Name,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
            };

            await _rep.CreateAsync(newColor);
            await _rep.SaveChangesAsync();
        }

        public async Task DeleteAsync(int Id)
        {
            await CheckColor(Id);

            await _rep.DeleteAsync(Id);
            await _rep.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateColorVM entity)
        {
            var oldColor = await CheckColor(entity.Id);

            var existsSameName = await (await _rep.ReadAsync(expression: x => x.Name == entity.Name && x.Id != entity.Id))
                .FirstOrDefaultAsync() is null;

            if (!existsSameName) throw new ObjectSameParamsException("There is same name category in data!", nameof(entity.Name));

            oldColor.Name = entity.Name;
            oldColor.UpdatedDate = DateTime.Now;

            await _rep.UpdateAsync(oldColor);
            await _rep.SaveChangesAsync();
        }

        public async Task<Color> CheckColor(int id)
        {
            if (id <= 0) throw new IdNegativeOrZeroException("Id must be over than and not equal to zero!", nameof(id));
            var oldColor = await _rep.ReadIdAsync(id);
            if (oldColor is null) throw new ObjectNullException("There is no like that object in Data!", nameof(oldColor));

            return oldColor;
        }

        public async Task RecoverAsync(int Id)
        {
            await CheckColor(Id);

            await _rep.RecoverAsync(Id);
            await _rep.SaveChangesAsync();
        }

        public async Task RemoveAsync(int Id)
        {
            await CheckColor(Id);

            await _rep.RemoveAsync(Id);
            await _rep.SaveChangesAsync();
        }
    }
}
