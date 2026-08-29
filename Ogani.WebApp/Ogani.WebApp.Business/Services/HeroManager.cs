using AutoMapper;
using FluentValidation;
using Ogani.WebApp.Business.Exceptions;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DataAccess.UnitOfWork;
using Ogani.WebApp.DTOs.HeroDTO;
using Ogani.WebApp.Entities;

namespace Ogani.WebApp.Business.Services
{
    public class HeroManager : WithImageGenericManager<Hero, HeroReadDTO, HeroDetailReadDTO, HeroCreateDTO, HeroUpdateDTO>, IHeroService
    {
        public HeroManager(IUoW uoW, IMapper mapper, IValidator<HeroCreateDTO> createValidator, IValidator<HeroUpdateDTO> updateValidator, IFileService fileService)
            : base(uoW, mapper, createValidator, updateValidator, fileService, imageFolderName: "heros")
        {
        }

        public async Task<HeroDetailReadDTO?> GetActiveHeroAsync()
        {
            Hero? hero = await _uoW.HeroRepository.GetActiveHeroAsync();

            if (hero == null)
                return null;

            return _mapper.Map<HeroDetailReadDTO>(hero);
        }

        public async Task SetHeroActiveAsync(int id)
        {
            Hero hero = await _uoW.HeroRepository.GetByIdAsync(id, tracking: true)
                ?? throw new NotFoundException(nameof(Hero), id);

            Hero? activeHero = await _uoW.HeroRepository.GetActiveHeroAsync(tracking: true);

            if (activeHero != null)
            {
                activeHero.IsActive = false;
                await _uoW.SaveChangesAsync();
            }

            hero.IsActive = true;
            await _uoW.SaveChangesAsync();
        }
    }
}
