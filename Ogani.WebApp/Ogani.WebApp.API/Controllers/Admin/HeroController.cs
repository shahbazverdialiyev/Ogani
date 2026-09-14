using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DTOs.HeroDTO;

namespace Ogani.WebApp.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class HeroesController : ControllerBase
    {
        private readonly IHeroService _heroService;

        public HeroesController(IHeroService heroService) =>
            _heroService = heroService;

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _heroService.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetForUpdate(int id) =>
            Ok(await _heroService.GetForUpdateAsync(id));

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveHero() =>
            Ok(await _heroService.GetActiveHeroAsync());

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] HeroCreateDTO heroDto)
        {
            var heroId = await _heroService.AddAsync(heroDto);

            return CreatedAtAction(
                nameof(GetForUpdate),
                new { id = heroId },
                new { id = heroId, message = $"Hero \"{heroDto.Title}\" was created successfully." }
            );
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] HeroUpdateDTO heroDto)
        {
            heroDto.Id = id;
            await _heroService.UpdateAsync(heroDto);
            return NoContent();
        }

        [HttpPatch("{id:int}/set-active")]
        public async Task<IActionResult> SetActive(int id)
        {
            await _heroService.SetHeroActiveAsync(id);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _heroService.DeleteAsync(id);
            return NoContent();
        }
    }
}