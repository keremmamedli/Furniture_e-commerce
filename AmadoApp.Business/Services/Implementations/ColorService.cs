using AmadoApp.Business.Exceptions.Commons;
using AmadoApp.Business.Services.Interfaces;
using AmadoApp.Business.ViewModels.BrandVMs;
using AmadoApp.Core.Entities;
using AmadoApp.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AmadoApp.Business.Services.Implementations
{
    public class ColorService : IColorRepository
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
            return await CheckBrand(Id);
        }

        public async Task CreateAsync(CreateBrandVM entity)
        {
            if(entity.Name is null) throw new ObjectParamsNullException("Object parameters is required!", nameof(entity.Name));

            var existsSameName = await (await _rep.ReadAsync(expression: x => x.Name == entity.Name))
                .FirstOrDefaultAsync() is null;

            if (!existsSameName) throw new ObjectSameParamsException("There is same name brand in data!", nameof(entity.Name));


            Color newBrand = new()
            {
                Name = entity.Name,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
            };

            await _rep.CreateAsync(newBrand);
            await _rep.SaveChangesAsync();
        }

        public async Task DeleteAsync(int Id)
        {
            await CheckBrand(Id);

            await _rep.DeleteAsync(Id);
            await _rep.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateBrandVM entity)
        {
            var oldBrand = await CheckBrand(entity.Id);

            var existsSameName = await (await _rep.ReadAsync(expression: x => x.Name == entity.Name && x.Id != entity.Id))
                .FirstOrDefaultAsync() is null;

            if (!existsSameName) throw new ObjectSameParamsException("There is same name brand in data!", nameof(entity.Name));


            oldBrand.Name = entity.Name;
            oldBrand.UpdatedDate = DateTime.Now;

            await _rep.UpdateAsync(oldBrand);
            await _rep.SaveChangesAsync();
        }

        public async Task<Color> CheckBrand(int id)
        {
            if (id <= 0) throw new IdNegativeOrZeroException("Id must be over than and not equal to zero!", nameof(id));
            var oldBrand = await _rep.ReadIdAsync(id);
            if (oldBrand is null) throw new ObjectNullException("There is no like that object in Data!", nameof(oldBrand));

            return oldBrand;
        }

        public async Task RecoverAsync(int Id)
        {
            await CheckBrand(Id);

            await _rep.RecoverAsync(Id);
            await _rep.SaveChangesAsync();
        }

        public async Task RemoveAsync(int Id)
        {
            await CheckBrand(Id);

            await _rep.RemoveAsync(Id);
            await _rep.SaveChangesAsync();
        }

        public Task<IQueryable<Color>> ReadAsync(Expression<Func<Color, bool>>? expression = null, Expression<Func<Color, object>>? expressionOrder = null, bool isDescending = false, params string[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<Color> ReadIdAsync(int Id = 0, params string[] entityIncludes)
        {
            throw new NotImplementedException();
        }

        public Task<Color> CreateAsync(Color entity)
        {
            throw new NotImplementedException();
        }

        public Task<Color> UpdateAsync(Color entity)
        {
            throw new NotImplementedException();
        }

        Task<Color> IRepository<Color>.DeleteAsync(int Id)
        {
            throw new NotImplementedException();
        }

        Task<Color> IRepository<Color>.RecoverAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
