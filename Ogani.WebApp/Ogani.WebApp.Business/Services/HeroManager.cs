using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Ogani.WebApp.Business.Exceptions;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DataAccess.Interfaces;
using Ogani.WebApp.DataAccess.UnitOfWork;
using Ogani.WebApp.DTOs.HeroDTO;
using Ogani.WebApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Services
{
    public class HeroManager : GenericManager<Hero, HeroReadDTO, HeroDetailReadDTO, HeroCreateDTO, HeroUpdateDTO>, IHeroService
    {
        private readonly IFileService _fileService;

        public HeroManager(IUoW uoW, IMapper mapper, IValidator<HeroCreateDTO> createValidator, IValidator<HeroUpdateDTO> updateValidator, IFileService fileService)
            : base(uoW, mapper, createValidator, updateValidator)
        {
            _fileService = fileService;
        }

        public override async Task<int> AddAsync(HeroCreateDTO heroDto)
        {
            ValidationResult validationResult = await _createValidator.ValidateAsync(heroDto);

            if (!validationResult.IsValid)
                throw new BusinessValidationException(validationResult.Errors);

            Hero hero = _mapper.Map<Hero>(heroDto);

            hero.ImageUrl = await _fileService.UploadAsync(heroDto.Image, "heros");

            await _uoW.HeroRepository.AddAsync(hero);
            await _uoW.SaveChangesAsync();

            return hero.Id;
        }

        public override async Task UpdateAsync(HeroUpdateDTO heroDto)
        {
            ValidationResult validationResult = await _updateValidator.ValidateAsync(heroDto);

            if (!validationResult.IsValid)
                throw new BusinessValidationException(validationResult.Errors);

            Hero hero = await _uoW.HeroRepository.GetByIdAsync(heroDto.Id, tracking: true)
                ?? throw new NotFoundException(nameof(Hero), heroDto.Id);

            string oldImagePath = hero.ImageUrl;

            _mapper.Map(heroDto, hero);

            if (heroDto.Image is not null)
            {
                hero.ImageUrl = await _fileService.UploadAsync(heroDto.Image, "heros");
            }
            else
            {
                hero.ImageUrl = oldImagePath;
            }

            await _uoW.SaveChangesAsync();

            if (heroDto.Image is not null && !string.IsNullOrEmpty(oldImagePath))
            {
                await _fileService.DeleteAsync(oldImagePath);
            }
        }

        public override async Task DeleteAsync(int id)
        {
            Hero hero = await _uoW.HeroRepository.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Hero), id);

            await _fileService.DeleteAsync(hero.ImageUrl);

            _uoW.HeroRepository.Delete(hero);
            await _uoW.SaveChangesAsync();
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
