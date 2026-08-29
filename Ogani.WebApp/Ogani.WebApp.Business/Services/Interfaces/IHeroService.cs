using Ogani.WebApp.DTOs.HeroDTO;

namespace Ogani.WebApp.Business.Services.Interfaces
{
    public interface IHeroService : IService<HeroReadDTO, HeroDetailReadDTO, HeroCreateDTO, HeroUpdateDTO>
    {
        Task<HeroDetailReadDTO?> GetActiveHeroAsync();
        Task SetHeroActiveAsync(int id);
    }
}
