using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Services.Interfaces;

namespace Ogani.WebApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HeroesController : ControllerBase
    {
        private readonly IHeroService _heroService;

        public HeroesController(IHeroService heroService) =>
            _heroService = heroService;

        [HttpGet]
        public async Task<IActionResult> GetActiveHero() => Ok(await _heroService.GetActiveHeroAsync());
    }
}