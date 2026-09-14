using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Services.Interfaces;

namespace Ogani.WebApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HeroesController : ControllerBase
    {
        private readonly IHeroService _heroService;

        public HeroesController(IHeroService heroService)
        {
            _heroService = heroService;
        }

        [HttpGet]
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
    }
}