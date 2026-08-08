using Ogani.WebApp.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Services.Interfaces
{
    public interface IService<TRead, TDetailRead, TCreate, TUpdate>
        where TRead : BaseDTO<int>
        where TDetailRead : BaseDTO<int>
        where TCreate : class
        where TUpdate : BaseDTO<int>
    {
        Task<List<TRead>> GetAllAsync();
        Task<TDetailRead> GetByIdAsync(int id);
        Task<TUpdate> GetForUpdateAsync(int id);
        Task AddAsync(TCreate entity);
        Task UpdateAsync(TUpdate entity);
        Task DeleteAsync(int id);
    }
}
