using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Services.Interfaces
{
    public interface IService<TRead,TCreate,TUpdate>
        where TRead : class
        where TCreate : class
        where TUpdate : class
    {
        Task<List<TRead>> GetAllAsync();
        Task<TRead> GetByIdAsync(int id);
        Task AddAsync(TCreate entity);
        Task UpdateAsync(TUpdate entity);
        Task DeleteAsync(int id);
    }
}
