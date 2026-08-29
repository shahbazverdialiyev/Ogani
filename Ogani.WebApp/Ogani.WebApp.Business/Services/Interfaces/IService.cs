using Ogani.WebApp.DTOs.Base;

namespace Ogani.WebApp.Business.Services.Interfaces
{
    public interface IService<TRead, TDetailRead, TCreate, TUpdate>
        where TRead : BaseDTO<int>
        where TDetailRead : BaseDTO<int>
        where TCreate : class
        where TUpdate : BaseDTO<int>
    {
        Task<IReadOnlyCollection<TRead>> GetAllAsync();
        Task<TDetailRead> GetByIdAsync(int id);
        Task<TUpdate> GetForUpdateAsync(int id);
        Task<int> AddAsync(TCreate entity);
        Task UpdateAsync(TUpdate entity);
        Task DeleteAsync(int id);
    }
}
