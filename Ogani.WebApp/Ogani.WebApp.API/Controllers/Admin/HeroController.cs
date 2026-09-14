using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Exceptions;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DTOs.HeroDTO;

namespace Ogani.WebApp.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class HeroesController : ControllerBase
    {
        private readonly IHeroService _heroService;

        public HeroesController(IHeroService heroService)
        {
            _heroService = heroService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var heroes = await _heroService.GetAllAsync();
            return Ok(heroes);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetForUpdate(int id)
        {
            try
            {
                var hero = await _heroService.GetForUpdateAsync(id);
                return Ok(hero);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] HeroCreateDTO heroDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _heroService.AddAsync(heroDto);
                return StatusCode(StatusCodes.Status201Created, new { message = $"Hero \"{heroDto.Title}\" was created successfully." });
            }
            catch (BusinessValidationException ex)
            {
                foreach (var error in ex.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                return BadRequest(ModelState);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromForm] HeroUpdateDTO heroDto)
        {
            if (id != heroDto.Id)
                return BadRequest(new { message = "ID mismatch in request URL and body." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _heroService.UpdateAsync(heroDto);
                return Ok(new { message = $"Hero \"{heroDto.Title}\" was updated successfully." });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (BusinessValidationException ex)
            {
                foreach (var error in ex.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                return BadRequest(ModelState);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveHero()
        {
            try
            {
                var activeHero = await _heroService.GetActiveHeroAsync();

                if (activeHero == null)
                {
                    return NotFound(new { message = "No active hero found." });
                }

                return Ok(activeHero);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An unexpected error occurred while fetching the active hero.",
                    detail = ex.Message
                });
            }
        }

        [HttpPatch("{id:int}/set-active")]
        public async Task<IActionResult> SetActive(int id)
        {
            try
            {
                await _heroService.SetHeroActiveAsync(id);
                return Ok(new { message = "Hero set as active successfully." });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _heroService.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}